using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      public AmplitudeEvent CreateEventWithNumber( string eventName, int number )
      {
         Dictionary<string, string> properties = new(){ ["Number"] = number.ToString() };

         AmplitudeEvent result = new()
         {
            EventType = eventName,
            Properties = properties,
            UserID = _userID,
            DeviceID = _deviceID,
            Time = DateTime.Now
         };

         return result;
      }
   }
}
