namespace TestAmplitude
{
   public interface IAmplitudeBackgroundEventTransmitter
   {
      void Startup();
      //On application close you need to close quicker; but you also want to transmit session end.
      void BeginShutdown();
      void Shutdown();
      void AddEvent( AmplitudeEvent amplitudeEvent );
   }
}