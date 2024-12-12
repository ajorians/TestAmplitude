using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TestAmplitude
{
   public class AmplitudeUserIDCreator : IAmplitudeUserIDCreator
   {
      private readonly string _userID;

      public AmplitudeUserIDCreator()
      {
         const string userRoot = "HKEY_CURRENT_USER";
         const string subkey = "TestAmplitude";
         const string keyName = userRoot + "\\" + $"Software\\{subkey}";

         string userID = (string) Registry.GetValue(keyName, "UserID", string.Empty);
         if( string.IsNullOrWhiteSpace(userID) )
         {
            userID = GenerateNewUserID();
            Registry.SetValue( keyName, "UserID", userID );
         }

         _userID = userID;
      }

      public string GetUserID()
      {
         return _userID;
      }

      private string GenerateNewUserID()
      {
         string guidDigits = new string(Guid.NewGuid().ToString("N").Where(char.IsDigit).Take(10).ToArray());
         return guidDigits;
      }
   }
}
