namespace TestAmplitude
{
   //I might change this interface to work better
   public interface IAmplitudeEventQueue
   {
      void AddEventToQueue( AmplitudeEvent amplitudeEvent );
      IEnumerable<AmplitudeEvent> GetEvents( int numRequestedEvents );
      void CompletedEvents( int numEventsCompleted );
      void PersistAllEvents();
   }
}