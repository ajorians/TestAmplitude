namespace TestAmplitude
{
   public interface IAmplitudeEventFactory
   {
      AmplitudeEvent CreateEvent( string eventName );
   }
}