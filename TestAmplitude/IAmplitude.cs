namespace TestAmplitude
{
   public interface IAmplitude
   {
      //Emitted when tracked an event.  Unrelated to network call occurred or if successfully tracked (could have been persisted)
      event EventHandler<TrackedEventArgs> OnTrackedEvent;

      void StartSession();
      void StopSession();

      //void return on purpose.  You assume it'll be successfully tracked.
      void TrackEventWithProperties( string eventName, IDictionary<string, string> properties );
   }
}