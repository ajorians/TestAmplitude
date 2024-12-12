using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TestAmplitude
{
   public class AmplitudeDeviceIDCreator : IAmplitudeDeviceIDCreator
   {
      private readonly string _deviceID;

      public AmplitudeDeviceIDCreator()
      {
         const string userRoot = "HKEY_CURRENT_USER";
         const string subkey = "TestAmplitude";
         const string keyName = userRoot + "\\" + $"Software\\{subkey}";

         string deviceID = (string) Registry.GetValue(keyName, "DeviceID", string.Empty);
         if( string.IsNullOrWhiteSpace( deviceID ) )
         {
            deviceID = GenerateNewDeviceID();
            Registry.SetValue( keyName, "DeviceID", deviceID );
         }

         _deviceID = deviceID;
      }

      public string GetDeviceID()
      {
         return _deviceID;
      }

      private string GenerateNewDeviceID()
      {
         return Guid.NewGuid().ToString();
      }
   }
}
