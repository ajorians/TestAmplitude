namespace TestAmplitude
{
   //Intention that whomever calls into this is in a thread
   //This is suppose to serialize/deserialize to file(s) (not implemented yet)
   public class AmplitudeEventQueue : IAmplitudeEventQueue
   {
      private readonly List<AmplitudeEvent> _pendingEvents = new();
      private readonly object _lockPendingEvents = new();

      public AmplitudeEventQueue()
      {
         //TODO: Perhaps see if have pending event file(s) first
         //If so set a member variable or something so we get those events first
      }

      public bool AnyEventsToTransmit()
      {
         lock (_lockPendingEvents)
         {
            return _pendingEvents.Any();
         }
      }

      public void AddEventToQueue( AmplitudeEvent amplitudeEvent )
      {
         lock (_lockPendingEvents)
         {
            _pendingEvents.Add( amplitudeEvent );
         }
      }

      public IEnumerable<AmplitudeEvent> GetEvents( int numRequestedEvents )
      {
         //TODO: Will need more smarts

         lock (_lockPendingEvents)
         {
            return _pendingEvents.Take( Math.Min( numRequestedEvents, _pendingEvents.Count ) );
         }
      }

      public void CompletedEvents( int numEventsCompleted )
      {
         lock (_lockPendingEvents)
         {
            _pendingEvents.RemoveRange( 0, numEventsCompleted );
         }
      }
   }
}
