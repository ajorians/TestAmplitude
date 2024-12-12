namespace TestAmplitude
{
   //I might change this interface to work better
   public interface IAmplitudeEventQueue
   {
      bool AnyEventsToTransmit();
      void AddEventToQueue( AmplitudeEvent amplitudeEvent );
      IEnumerable<AmplitudeEvent> GetEvents( int numRequestedEvents );
      void CompletedEvents( int numEventsCompleted );
   }
}