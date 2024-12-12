namespace TestAmplitude
{
   public interface IAmplitude
   {
      event EventHandler<TrackedEventArgs> OnTrackedEvent;

      void StartSession();
      void StopSession();

      void TrackEvent( string eventName );
      void TrackEventWithNumber( string eventName, int number );
   }
}