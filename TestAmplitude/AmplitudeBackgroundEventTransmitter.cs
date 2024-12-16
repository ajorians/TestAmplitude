using System;
using System.Collections.Generic;
using System.Diagnostics;
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
      private bool _beginShutdown = false;

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
            Debug.Assert( false, "You must have already called startup.  You can Shutdown and Startup again; but I probably would make a new instance each time" );
            return;
         }

         _beginShutdown = false;
         _keepRunning = true;
         _workerThread = new Thread( () => DoAnalytics() );
         _workerThread.Start();
      }

      public void BeginShutdown()
      {
         _beginShutdown = true;
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
         if( _beginShutdown )
         {
            //We call BeginShutdown() on application closing.  There shouldn't be any more user events as the application is closing.
            //But say there was.  It gets discarded.  If it wasn't it would get sent after the sesson_end event.
            Debug.Assert( false, "Just to let you know your event was not added" );
            return;
         }

         _amplitudeEventQueue.AddEventToQueue( amplitudeEvent );
      }

      private void DoAnalytics()
      {
         while ( _keepRunning )
         {
            //I put the sleep as the first thing in the loop so after session_end the hope is we'll transmit it before joining the thread.
            //Of course this isn't guaranteed (especially if we reduce the sleep time or if there is a lot of events going through)
            Thread.Sleep( TimeSpan.FromMilliseconds( 1_000 ) );

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
         }
      }
   }
}
