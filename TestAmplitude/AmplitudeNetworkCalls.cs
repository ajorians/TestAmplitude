using System.Net;
using System.Net.Http;
using System.Text;

namespace TestAmplitude
{
   //Purpose: Do Network calls
   //Network rate-limiting ought to be done by another class.
   //Checking/Limiting number of items per network request ought to be done by another class
   //Validating event names are not greater than 1024 characters/whateve ought to be done by another class
   //Offline serializing/deserializing ought to be done by another class.
   //Offloading to a thread was intended to be in another class.  Though maybe async is OK?
   //This is intended that you can stop and start as many times as needed.
   //And you could have multiple instances of this class if need (though shouldn't need).
   public class AmplitudeNetworkCalls : IAmplitudeNetworkCalls
   {
      private const string Endpoint = "https://api2.amplitude.com/2/httpapi";
      private readonly string _apiKey;
      private readonly string _userID;
      private readonly string _deviceID;

      private bool _sessionStarted;

      public AmplitudeNetworkCalls( string apiKey, string userID, string deviceID )
      {
         _apiKey = apiKey;
         _userID = userID;
         _deviceID = deviceID;
      }

      public async Task<bool> StartSession()
      {
         if (_sessionStarted)
         {
            throw new InvalidOperationException( "Cannot start a session without stopping it first" );
         }

         using var client = new HttpClient();

         try
         {
            Uri url = new( Endpoint );

            AmplitudeSimpleRequest request = new()
            {
               APIKey = _apiKey,
               Events = [new AmplitudeEvent(){ EventType= "[Amplitude] Start Session", Time = DateTime.Now, UserID=_userID, DeviceID=_deviceID}]
            };

            string jsonData = request.GetJson();

            using HttpContent content = new StringContent( jsonData, Encoding.UTF8, "application/json" );

            using HttpResponseMessage response = await client.PostAsync( url, content );

            _sessionStarted = response.IsSuccessStatusCode;
         }
         catch (Exception ex)
         {
            //_errorLogger.LogException( ex );
            return false;
         }

         return _sessionStarted;
      }

      public async Task<bool> StopSession()
      {
         if (!_sessionStarted)
         {
            throw new InvalidOperationException( "Cannot stop a session that hasn't been started" );
         }

         using var client = new HttpClient();

         try
         {
            Uri url = new( Endpoint );

            AmplitudeSimpleRequest request = new()
            {
               APIKey = _apiKey,
               Events = [new AmplitudeEvent(){ EventType= "[Amplitude] End Session", Time = DateTime.Now, UserID=_userID, DeviceID = _deviceID }]
            };

            string jsonData = request.GetJson();

            using HttpContent content = new StringContent( jsonData, Encoding.UTF8, "application/json" );

            using HttpResponseMessage response = await client.PostAsync( url, content );

            _sessionStarted = !response.IsSuccessStatusCode;
         }
         catch (Exception ex)
         {
            //_errorLogger.LogException( ex );
            return false;
         }

         return !_sessionStarted;
      }

      public async Task<bool> TrackEvent( string eventName )
      {
         if (!_sessionStarted)
         {
            throw new InvalidOperationException( "Session needs to be started first" );
         }

         using var client = new HttpClient();

         try
         {
            Uri url = new( Endpoint );

            AmplitudeSimpleRequest request = new()
            {
               APIKey = _apiKey,
               Events = [new AmplitudeEvent(){ EventType= eventName, Time = DateTime.Now, UserID=_userID, DeviceID = _deviceID }]
            };

            string jsonData = request.GetJson();

            using HttpContent content = new StringContent( jsonData, Encoding.UTF8, "application/json" );

            using HttpResponseMessage response = await client.PostAsync( url, content );

            return response.IsSuccessStatusCode;
         }
         catch (Exception ex)
         {
            //_errorLogger.LogException( ex )
         }

         return false;
      }

      public void TrackEventWithNumber( string eventName, int number )
      {
         if (!_sessionStarted)
         {
            throw new InvalidOperationException( "Session needs to be started first" );
         }
      }
   }
}
