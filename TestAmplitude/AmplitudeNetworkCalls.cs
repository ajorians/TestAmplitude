using System.Net.Http;
using System.Text;

namespace TestAmplitude
{
   //Purpose: Do Network calls
   //Network rate-limiting ought to be done by another class.
   //Checking/Limiting number of items per network request ought to be done by another class
   //Validating event names are not greater than 1024 characters/whatever ought to be done by another class
   //Offline serializing/deserializing ought to be done by another class.
   //Offloading to a thread was intended to be in another class.  Though maybe async is OK?
   //This is intended that you can stop and start as many times as needed.
   //And you could have multiple instances of this class if need (though shouldn't need).
   public class AmplitudeNetworkCalls : IAmplitudeNetworkCalls
   {
      private const string Endpoint = "https://api2.amplitude.com/2/httpapi";
      private readonly string _apiKey;
      private readonly string _userID;//Might remove this if start/end session could be an AmplitudeEvent
      private readonly string _deviceID;
      private readonly IAmplitudeUserPropertiesProvider _userPropertiesProvider;
      private readonly IPretendOffline _pretendOffline;

      public AmplitudeNetworkCalls( string apiKey, string userID, string deviceID, IAmplitudeUserPropertiesProvider userPropertiesProvider, IPretendOffline pretendOffline )
      {
         _apiKey = apiKey;
         _userID = userID;
         _deviceID = deviceID;
         _userPropertiesProvider = userPropertiesProvider ?? throw new ArgumentNullException( nameof( userPropertiesProvider ) );
         _pretendOffline = pretendOffline ?? throw new ArgumentNullException( nameof ( pretendOffline ) );
      }

      public async Task<bool> TrackEvents( IEnumerable<AmplitudeEvent> events )
      {
         using var client = new HttpClient();

#pragma warning disable CS0168 // Variable is declared but never used
         try
         {
            Uri url = new( Endpoint );

            AmplitudeSimpleRequest request = new()
            {
               APIKey = _apiKey,
               Events = events
            };

            string jsonData = request.GetJson();

            using HttpContent content = new StringContent( jsonData, Encoding.UTF8, "application/json" );

            if ( _pretendOffline.IsOffline() )
            {
               throw new HttpRequestException();
            }

            using HttpResponseMessage response = await client.PostAsync( url, content );

            return response.IsSuccessStatusCode;
         }
         catch ( Exception ex )
         {
            //_errorLogger.LogException( ex )
         }
#pragma warning restore CS0168 // Variable is declared but never used

         return false;
      }
   }
}
