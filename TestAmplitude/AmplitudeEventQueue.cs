using System.IO;
using Newtonsoft.Json;

namespace TestAmplitude
{
   //Intention that whomever calls into this is in a thread
   //This is suppose to serialize/deserialize to file(s)
   public class AmplitudeEventQueue : IAmplitudeEventQueue
   {
      private readonly List<AmplitudeEvent> _pendingEvents = new();
      private readonly object _lockPendingEvents = new();

      //This is the root path.  To make sure it doesn't grow infinitely; each days event are a separate file
      //I suppose may be empty if you are OK with offline events disappearing
      private readonly string _persistEventPath;

      private bool _isUsingSerializedEvents = false;
      private List<AmplitudeEvent> _serializedEvents = new();
      private string _serializedFilename;

      public AmplitudeEventQueue( string persistEventPath )
      {
         _persistEventPath = persistEventPath;

         _isUsingSerializedEvents = true;
         _serializedFilename = string.Empty;
         CheckFiles();
      }

      private void CheckFiles()
      {
         if (_isUsingSerializedEvents && string.IsNullOrEmpty( _serializedFilename ))
         {
            IEnumerable<string> eventFiles = Directory.EnumerateFiles( _persistEventPath, "*.dat" );
            if (eventFiles.Any())
            {
               string existingEventsFilePath = string.Empty;
               try
               {
                  List<AmplitudeEvent> existingObjects = new();
                  existingEventsFilePath = eventFiles.First();

                  if (File.Exists( existingEventsFilePath ))
                  {
                     string existingJson = File.ReadAllText( existingEventsFilePath );
                     if (!string.IsNullOrWhiteSpace( existingJson ))
                     {
                        existingObjects = JsonConvert.DeserializeObject<List<AmplitudeEvent>>( existingJson ) ?? new List<AmplitudeEvent>();
                     }
                  }

                  _isUsingSerializedEvents = true;
                  _serializedEvents = existingObjects;
                  _serializedFilename = existingEventsFilePath;
               }
               catch (Exception ex)
               {
                  //_errorLogger.LogException( ex )

                  //Something went wrong with loading up this events file.  Let's remove the file so not to see it again.
                  if (string.IsNullOrEmpty( existingEventsFilePath ))
                  {
                     File.Delete( existingEventsFilePath );
                  }
               }
            }
            else
            {
               _isUsingSerializedEvents = false;
               _serializedEvents = null;
               _serializedFilename = string.Empty;
            }
         }
      }

      public void AddEventToQueue( AmplitudeEvent amplitudeEvent )
      {
         lock (_lockPendingEvents)
         {
            _pendingEvents.Add( amplitudeEvent );
         }
      }

      public IEnumerable<AmplitudeEvent> GetEvents( int numRequestedEvents )
      {
         if( _isUsingSerializedEvents )
         {
            CheckFiles();

            if( _serializedEvents is not null )
            {
               return _serializedEvents.Take( Math.Min( numRequestedEvents, _serializedEvents.Count ) );
            }
         }

         lock (_lockPendingEvents)
         {
            return _pendingEvents.Take( Math.Min( numRequestedEvents, _pendingEvents.Count ) );
         }
      }

      public void CompletedEvents( int numEventsCompleted )
      {
         if ( _isUsingSerializedEvents )
         {
            _serializedEvents.RemoveRange( 0, numEventsCompleted );
            if ( !_serializedEvents.Any() )
            {
               File.Delete( _serializedFilename );
               _serializedFilename = string.Empty;
            }

            return;
         }

         lock (_lockPendingEvents)
         {
            _pendingEvents.RemoveRange( 0, numEventsCompleted );
         }
      }

      public void PersistAllEvents()
      {
         lock (_lockPendingEvents)
         {
            if ( !string.IsNullOrEmpty( _persistEventPath ) )
            {
               string filename = Path.ChangeExtension( DateTime.Now.ToString("yyyy-MM-dd"), ".dat" );
               string filePath = Path.Combine( _persistEventPath, filename );

               //There might be events from today.  Would like to preserve them
               List<AmplitudeEvent> existingObjects = new();

               if (File.Exists( filePath ))
               {
                  string existingJson = File.ReadAllText(filePath);
                  if (!string.IsNullOrWhiteSpace( existingJson ))
                  {
                     existingObjects = JsonConvert.DeserializeObject<List<AmplitudeEvent>>( existingJson ) ?? new List<AmplitudeEvent>();
                  }
               }

               // Append new objects
               existingObjects.AddRange( _pendingEvents );

               string jsonString = JsonConvert.SerializeObject( existingObjects, Formatting.Indented );

               File.WriteAllText( filePath, jsonString );
            }

            _pendingEvents.Clear();
         }
      }
   }
}
