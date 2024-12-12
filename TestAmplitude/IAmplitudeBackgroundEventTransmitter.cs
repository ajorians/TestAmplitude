namespace TestAmplitude
{
   public interface IAmplitudeBackgroundEventTransmitter
   {
      void Startup();
      void Shutdown();
      void AddEvent( AmplitudeEvent amplitudeEvent );
   }
}