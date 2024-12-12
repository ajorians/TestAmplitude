namespace TestAmplitude
{
   public class Amplitude : IAmplitude
   {
      private readonly IAmplitudeNetworkCalls _amplitudeNetworkCalls;

      public event EventHandler<TrackedEventArgs> OnTrackedEvent;
      public Amplitude( IAmplitudeNetworkCalls amplitudeNetworkCalls )
      {
         _amplitudeNetworkCalls = amplitudeNetworkCalls ?? throw new ArgumentNullException( nameof( amplitudeNetworkCalls ) );
      }

      public void StartSession()
      {
         _amplitudeNetworkCalls.StartSession();
      }

      public void StopSession()
      {
         _amplitudeNetworkCalls.StopSession();
      }

      public void TrackEvent( string eventName )
      {
         _amplitudeNetworkCalls.TrackEvent( eventName );

         OnTrackedEvent?.Invoke( this, new() { EventName = eventName } );
      }

      public void TrackEventWithNumber( string eventName, int number )
      {
         OnTrackedEvent?.Invoke( this, new() { EventName = eventName, NumberValue = number } );
      }
   }
}
