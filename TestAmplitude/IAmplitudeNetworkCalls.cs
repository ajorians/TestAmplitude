namespace TestAmplitude
{
   public interface IAmplitudeNetworkCalls
   {
      //Returns true if started a session
      Task<bool> StartSession();

      //Returns true if stopped session
      Task<bool> StopSession();

      //Returns true if tracked
      //Be sure to check Amplitude rate limits.  Requests per second/events per request
      Task<bool> TrackEvents( IEnumerable<AmplitudeEvent> events );

      //TODO: Going to remove these
      Task<bool> TrackEvent( string eventName );
      void TrackEventWithNumber( string eventName, int number );
   }
}