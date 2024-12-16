namespace TestAmplitude
{
   public class Amplitude : IAmplitude
   {
      private readonly IAmplitudeEventFactory _amplitudeEventFactory;
      private readonly IAmplitudeNetworkCalls _amplitudeNetworkCalls;
      private readonly IAmplitudeBackgroundEventTransmitter _amplitudeBackgroundEventTransmitter;

      public event EventHandler<TrackedEventArgs> OnTrackedEvent;

      public Amplitude( IAmplitudeEventFactory amplitudeEventFactory,
                        IAmplitudeNetworkCalls amplitudeNetworkCalls,
                        IAmplitudeBackgroundEventTransmitter amplitudeBackgroundEventTransmitter )
      {
         _amplitudeEventFactory = amplitudeEventFactory ?? throw new ArgumentNullException( nameof( amplitudeEventFactory ) );
         _amplitudeNetworkCalls = amplitudeNetworkCalls ?? throw new ArgumentNullException( nameof( amplitudeNetworkCalls ) );
         _amplitudeBackgroundEventTransmitter = amplitudeBackgroundEventTransmitter ?? throw new ArgumentNullException( nameof( amplitudeBackgroundEventTransmitter ) );

         _amplitudeBackgroundEventTransmitter.Startup();
      }

      public void Shutdown()
      {
         _amplitudeBackgroundEventTransmitter.Shutdown();
      }

      public void StartSession()
      {
         string eventName = "session_start";
         Dictionary<string, string> eventProperties = new();

         //Add this event to something that is responsible for handling it on the background such that this function can quickly return
         _amplitudeBackgroundEventTransmitter.AddEvent( _amplitudeEventFactory.CreateEventWithProperties( eventName, eventProperties ) );

         OnTrackedEvent?.Invoke( this, new() { EventName = eventName, EventProperties = eventProperties } );
      }

      public void StopSession()
      {
         string eventName = "session_end";
         Dictionary<string, string> eventProperties = new();

         //Add this event to something that is responsible for handling it on the background such that this function can quickly return
         _amplitudeBackgroundEventTransmitter.AddEvent( _amplitudeEventFactory.CreateEventWithProperties( eventName, eventProperties ) );

         OnTrackedEvent?.Invoke( this, new() { EventName = eventName, EventProperties = eventProperties } );

         _amplitudeBackgroundEventTransmitter.BeginShutdown();
      }

      public void TrackEventWithProperties( string eventName, IDictionary<string, string> eventProperties )
      {
         //Add this event to something that is responsible for handling it on the background such that this function can quickly return
         _amplitudeBackgroundEventTransmitter.AddEvent( _amplitudeEventFactory.CreateEventWithProperties( eventName, eventProperties ) );

         OnTrackedEvent?.Invoke( this, new () { EventName = eventName, EventProperties = eventProperties } );
      }
   }
}
