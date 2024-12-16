namespace TestAmplitude
{
   public class AmplitudeEventFactory : IAmplitudeEventFactory
   {
      private readonly string _userID;
      private readonly string _deviceID;
      private readonly IAmplitudeUserPropertiesProvider _userPropertiesProvider;

      public AmplitudeEventFactory( string userID, string deviceID, IAmplitudeUserPropertiesProvider userPropertiesProvider )
      {
         _userID = userID;
         _deviceID = deviceID;
         _userPropertiesProvider = userPropertiesProvider ?? throw new ArgumentNullException( nameof( userPropertiesProvider ) );
      }

      public AmplitudeEvent CreateEventWithProperties( string eventName, IDictionary<string, string> eventProperties )
      {
         AmplitudeEvent result = new()
         {
            InsertID = Guid.NewGuid().ToString(),
            UserID = _userID,
            DeviceID = _deviceID,
            EventType = eventName,
            EventProperties = eventProperties,
            UserProperties = _userPropertiesProvider.GetUserProperties(),
            Platform = "Win32",
            OS = "Windows",
            OSVersion = System.Environment.OSVersion.ToString(),
            Time = DateTime.Now
         };

         return result;
      }
   }
}
