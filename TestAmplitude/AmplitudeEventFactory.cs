namespace TestAmplitude
{
   public class AmplitudeEventFactory : IAmplitudeEventFactory
   {
      private readonly string _userID;
      private readonly string _deviceID;

      public AmplitudeEventFactory( string userID, string deviceID )
      {
         _userID = userID;
         _deviceID = deviceID;
      }

      public AmplitudeEvent CreateEvent( string eventName )
      {
         AmplitudeEvent result = new()
         {
            EventType = eventName,
            UserID = _userID,
            DeviceID = _deviceID,
            Time = DateTime.Now
         };

         return result;
      }

      public AmplitudeEvent CreateEventWithProperties( string eventName, IDictionary<string, string> eventProperties )
      {
         AmplitudeEvent result = new()
         {
            EventType = eventName,
            EventProperties = eventProperties,
            UserID = _userID,
            DeviceID = _deviceID,
            Time = DateTime.Now
         };

         return result;
      }
   }
}
