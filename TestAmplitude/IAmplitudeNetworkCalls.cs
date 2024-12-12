namespace TestAmplitude
{
   public interface IAmplitudeNetworkCalls
   {
      //Returns true if started a session
      Task<bool> StartSession();

      //Return true if stopped session
      Task<bool> StopSession();

      Task<bool> TrackEvent( string eventName );
      void TrackEventWithNumber( string eventName, int number );
   }
}