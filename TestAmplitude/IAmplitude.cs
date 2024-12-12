namespace TestAmplitude
{
   public interface IAmplitude
   {
      event EventHandler<TrackedEventArgs> OnTrackedEvent;

      void TrackEvent( string eventName );
      void TrackEventWithNumber( string eventName, int number );
   }
}