namespace TestAmplitude
{
   public interface IAmplitudeEventFactory
   {
      AmplitudeEvent CreateEventWithProperties( string eventName, IDictionary<string, string> eventProperties );
   }
}