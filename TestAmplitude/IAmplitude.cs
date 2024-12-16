namespace TestAmplitude
{
   public interface IAmplitude
   {
      //Emitted when tracked an event.  Unrelated to network call occurred or if successfully tracked (could have been persisted)
      //Note session_start and session_end are "events".
      event EventHandler<TrackedEventArgs> OnTrackedEvent;

      //Call this right after instantiation
      void StartSession();

      //Call this as soon as you know you are closing the application
      void StopSession();

      //Call this as late as you can while closing as this shuts the whole thing down
      void Shutdown();

      //void return on purpose.  You assume it'll be successfully tracked.
      void TrackEventWithProperties( string eventName, IDictionary<string, string> properties );
   }
}