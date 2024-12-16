namespace TestAmplitude
{
   public interface IAmplitudeEventFactory
   {
      AmplitudeEvent CreateEvent( string eventName );

      AmplitudeEvent CreateEventWithProperties( string eventName, IDictionary<string, string> eventProperties );
   }
}