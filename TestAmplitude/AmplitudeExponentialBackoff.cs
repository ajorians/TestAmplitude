namespace TestAmplitude
{
   public class AmplitudeExponentialBackoff : IAmplitudeExponentialBackoff
   {
      private readonly TimeSpan _maxDelayTimeSpan;
      private DateTime _startTime;
      private int _retryCount = 0;
      private int _pow = 1;
      private TimeSpan _currentDelay = TimeSpan.Zero;

      public AmplitudeExponentialBackoff()
      {
         _maxDelayTimeSpan = TimeSpan.FromHours( 1d );

         Reset();
      }

      public bool HaveWaitedEnoughTime()
      {
         var now = DateTime.Now;

         if ( now >= _startTime + _currentDelay )
         {
            return true;
         }

         return false;
      }

      public void IncrementFailure()
      {
         _retryCount++;
         if ( _retryCount < 31 )
         {
            _pow = _pow << 1;
         }

         TimeSpan delay = TimeSpan.FromSeconds( 1d * _pow );
         _currentDelay = delay > _maxDelayTimeSpan ? _maxDelayTimeSpan : delay;
      }

      public void Reset()
      {
         _startTime = DateTime.Now;
         _retryCount = 0;
         _pow = 1;
         _currentDelay = TimeSpan.Zero;
      }
   }
}
