namespace TestAmplitude
{
   public interface IAmplitudeEventFactory
   {
      AmplitudeEvent CreateEvent( string eventName );
      AmplitudeEvent CreateEventWithNumber( string eventName, int number );
   }
}