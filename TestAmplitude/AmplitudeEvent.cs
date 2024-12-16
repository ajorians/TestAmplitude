using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestAmplitude
{
   public record AmplitudeEvent
   {
      [JsonProperty( "insert_id" )]
      public string InsertID { get; set; } = Guid.NewGuid().ToString();

      [JsonProperty( "user_id" )]
      public string UserID { get; set; }

      [JsonProperty( "device_id" )]
      public string DeviceID { get; set; }

      [JsonProperty( "event_type" )]
      public string EventType { get; set; }

      [JsonProperty( "event_properties")]
      public IDictionary<string, string> EventProperties { get; set; }

      [JsonProperty( "user_properties" )]
      public IDictionary<string, string> UserProperties { get; set; }

      [JsonProperty( "platform" )]
      public string Platform { get; set; }

      [JsonProperty( "os_name" )]
      public string OS { get; set; }

      [JsonProperty( "os_version" )]
      public string OSVersion { get; set; }

      [JsonProperty( "language" )]
      public string Language { get; set; }

      [JsonProperty( "time" )]
      [JsonConverter( typeof( UnixDateTimeConverter ) )]
      public DateTime Time { get; set; } = DateTime.Now;
   }
}