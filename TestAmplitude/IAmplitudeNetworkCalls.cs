namespace TestAmplitude
{
   public interface IAmplitudeNetworkCalls
   {
      //Returns true if tracked
      //Be sure to check Amplitude rate limits.  Requests per second/events per request
      Task<bool> TrackEvents( IEnumerable<AmplitudeEvent> events );
   }
}