using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAmplitude
{
   public class AmplitudeBackgroundEventTransmitter : IAmplitudeBackgroundEventTransmitter
   {
      private readonly IAmplitudeNetworkCalls _amplitudeNetworkCalls;
      private readonly IAmplitudeEventQueue _amplitudeEventQueue;

      private Thread _workerThread;
      private bool _keepRunning = false;

      public AmplitudeBackgroundEventTransmitter( IAmplitudeNetworkCalls amplitudeNetworkCalls,
                                                  IAmplitudeEventQueue amplitudeEventQueue )
      {
         _amplitudeNetworkCalls = amplitudeNetworkCalls ?? throw new ArgumentNullException( nameof( amplitudeNetworkCalls ) );
         _amplitudeEventQueue = amplitudeEventQueue ?? throw new ArgumentNullException( nameof( amplitudeEventQueue ) );
      }

      public void Startup()
      {
         if ( _workerThread is not null )
         {
            return;
         }

         _keepRunning = true;
         _workerThread = new Thread( () => DoAnalytics() );
         _workerThread.Start();
      }

      public void Shutdown()
      {
         _keepRunning = false;
         if (_workerThread is not null)
         {
            _workerThread.Join();
            _workerThread = null;
         }
      }

      public void AddEvent( AmplitudeEvent amplitudeEvent )
      {
         _amplitudeEventQueue.AddEventToQueue( amplitudeEvent );
      }

      private void DoAnalytics()
      {
         while ( _keepRunning )
         {
            //Ask the rate limiter for how many events we can handle for this request.  TODO: Add a rate limiter
            int numEventsCanHandle = int.MaxValue;

            //Now get some work, if any, to do
            IEnumerable<AmplitudeEvent> eventsToProcess = _amplitudeEventQueue.GetEvents( numEventsCanHandle );

            if ( eventsToProcess.Any() )
            {
               if ( _amplitudeNetworkCalls.TrackEvents( eventsToProcess ).Result )
               {
                  int numEventsCompleted = eventsToProcess.Count();
                  _amplitudeEventQueue.CompletedEvents( numEventsCompleted );
               }
            }

               Thread.Sleep( TimeSpan.FromMilliseconds( 1_000 ) );
            }
         }
      }
}
