using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAmplitude
{
   public class Amplitude : IAmplitude
   {
      public event EventHandler<TrackedEventArgs> OnTrackedEvent;
      public Amplitude( string apiKey )
      {
      }

      public void TrackEvent( string eventName )
      {
         OnTrackedEvent?.Invoke( this, new() { EventName = eventName } );
      }

      public void TrackEventWithNumber( string eventName, int number )
      {
         OnTrackedEvent?.Invoke( this, new() { EventName = eventName, NumberValue = number } );
      }
   }
}
