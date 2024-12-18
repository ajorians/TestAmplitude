﻿using Newtonsoft.Json;

namespace TestAmplitude
{
   public record AmplitudeSimpleRequest
   {
      [JsonProperty( "api_key" )]
      public string APIKey { get; set; }

      [JsonProperty( "events" )]
      public IEnumerable<AmplitudeEvent> Events { get; set; }

      public string GetJson() => JsonConvert.SerializeObject( this );
   }
}
