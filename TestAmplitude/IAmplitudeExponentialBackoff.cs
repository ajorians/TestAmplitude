namespace TestAmplitude
{
   public interface IAmplitudeExponentialBackoff
   {
      bool HaveWaitedEnoughTime();
      void IncrementFailure();
      void Reset();
   }
}