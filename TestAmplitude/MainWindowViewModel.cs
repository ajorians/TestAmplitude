using System.ComponentModel;
using System.Windows.Input;

namespace TestAmplitude
{
   public class MainWindowViewModel : INotifyPropertyChanged
   {
      private IAmplitude _amplitude;

      public MainWindowViewModel()
      {
         APIKey = AmplitudeAPIKey.APIKey;

         AmplitudeOutput = "Output:" + Environment.NewLine;

         TrackEventNameText = "Sample Event Name :)";

         SourceText = "fuse";
         DurationInSecondsText = "0";
      }

      public void OnClosing( object sender, CancelEventArgs e )
      {
         if (_amplitude is not null)
         {
            _amplitude.StopSession();
            //This sleep is just a little bit in case it helps make it so the stop session.
            //This should be small enough not to be noticable; but something
            Thread.Sleep( TimeSpan.FromMilliseconds( 25 ) );
            _amplitude.Shutdown();
            _amplitude = null;
         }
      }

      private string _apiKey;
      public string APIKey
      {
         get => _apiKey;
         set
         {
            if (_apiKey != value)
            {
               _apiKey = value;
               OnPropertyChanged( nameof( APIKey ) );
               OnPropertyChanged( nameof( SessionEnabled ) );
            }
         }
      }

      public bool APIKeyEnabled
      {
         get
         {
            return _amplitude is null;
         }
      }

      public string SessionButtonText
      {
         get
         {
            if ( _amplitude is null )
            {
               return "Start Session";
            }

            return "Stop Session";
         }
      }

      public bool SessionEnabled
      {
         get
         {
            if ( string.IsNullOrEmpty( APIKey ) )
            {
               return false;
            }
            return true;
         }
      }

      private void OnSession()
      {
         if (_amplitude is null)
         {
            AmplitudeUserIDCreator amplitudeUserIDCreator = new();
            AmplitudeDeviceIDCreator amplitudeDeviceIDCreator = new();

            AmplitudeUserPropertiesProvider amplitudeUserPropertiesProvider = new();

            AmplitudeNetworkCalls amplitudeNetworkCalls = new( APIKey, amplitudeUserIDCreator.GetUserID(), amplitudeDeviceIDCreator.GetDeviceID(), amplitudeUserPropertiesProvider );
            AmplitudeEventQueue amplitudeEventQueue = new();
            AmplitudeEventFactory amplitudeEventFactory = new( amplitudeUserIDCreator.GetUserID(), amplitudeDeviceIDCreator.GetDeviceID(), amplitudeUserPropertiesProvider );
            AmplitudeBackgroundEventTransmitter amplitudeBackgroundEventTransmitter = new( amplitudeNetworkCalls, amplitudeEventQueue );
            _amplitude = new Amplitude( amplitudeEventFactory, amplitudeNetworkCalls, amplitudeBackgroundEventTransmitter );

            AmplitudeOutput += $"New session with Key ({APIKey}){Environment.NewLine}";

            _amplitude.StartSession();

            _amplitude.OnTrackedEvent += OnTrackedEvent;
         }
         else
         {
            _amplitude.StopSession();
            //This sleep is just a little bit in case it helps make it so the stop session.
            //This should be small enough not to be noticable; but something
            Thread.Sleep( TimeSpan.FromMilliseconds( 25 ) );
            _amplitude.Shutdown();
            _amplitude = null;
            AmplitudeOutput += $"Cleared session{Environment.NewLine}";
         }

         OnPropertyChanged( nameof( SessionButtonText ) );
         OnPropertyChanged( nameof( APIKeyEnabled ) );
         OnPropertyChanged( nameof( IsTrackingOptionsVisible ) );
      }

      private void OnTrackedEvent( object sender, TrackedEventArgs e )
      {
         //Session started/ended don't have event properties; but I figure no reason to display anything.
         if ( e.EventProperties is not null )
         {
            static string propertiesToText( IDictionary<string, string> eventProperties )
            {
               string result = string.Empty;
               foreach( var item in eventProperties)
               {
                  result += $"[{item.Key}] = '{item.Value}'; ";
               }
               return result;
            }

            AmplitudeOutput += $"Tracked Event: '{e.EventName}' with event properties: \"{propertiesToText( e.EventProperties )}\"{Environment.NewLine}";
         }
      }

      private ICommand _sessionCommand;
      public ICommand SessionCommand => _sessionCommand ??= new RelayCommand( () => OnSession() );

      public bool IsTrackingOptionsVisible
      {
         get
         {
            return _amplitude is not null;
         }
      }

      //Event

      private string _trackEventNameText;
      public string TrackEventNameText
      {
         get => _trackEventNameText;
         set
         {
            if ( _trackEventNameText != value )
            {
               _trackEventNameText = value;
               OnPropertyChanged( nameof( TrackEventNameText ) );
               OnPropertyChanged( nameof( TrackEventNameEnabled ) );
            }
         }
      }

      public bool TrackEventNameEnabled
      {
         get
         {
            if (string.IsNullOrEmpty( TrackEventNameText ))
            {
               return false;
            }
            return true;
         }
      }

      private void OnTrackEvent() => _amplitude.TrackEventWithProperties( TrackEventNameText, new Dictionary<string, string>() );

      private ICommand _trackEventCommand;
      public ICommand TrackEventCommand => _trackEventCommand ??= new RelayCommand( () => OnTrackEvent() );

      //Media Import Example

      private string _sourceText;
      public string SourceText
      {
         get => _sourceText;
         set
         {
            if (_sourceText != value)
            {
               _sourceText = value;
               OnPropertyChanged( nameof( SourceText ) );
               OnPropertyChanged( nameof( TrackMediaImportEnabled ) );
            }
         }
      }

      private string _durationInSecondsText;
      public string DurationInSecondsText
      {
         get => _durationInSecondsText;
         set
         {
            if (_durationInSecondsText != value)
            {
               _durationInSecondsText = value;
               OnPropertyChanged( nameof( DurationInSecondsText ) );
               OnPropertyChanged( nameof( TrackMediaImportEnabled ) );
            }
         }
      }

      private void OnTrackMediaImport()
      {
         Dictionary<string, string> eventProperties = new Dictionary<string, string>()
         {
            ["aspect ratio"] = "wide",
            ["content type"] = "video",
            ["duration"] = DurationInSecondsText,
            ["format"] = "tiff",
            ["import origin"] = "powerpoint",
            ["import source"] = SourceText,
            ["new project"] = "true",
         };

         _amplitude.TrackEventWithProperties( "media imported", eventProperties );
      }

      private ICommand _trackMediaImportEventCommand;
      public ICommand TrackMediaImportEventCommand => _trackMediaImportEventCommand ??= new RelayCommand( () => OnTrackMediaImport() );

      public bool TrackMediaImportEnabled
      {
         get
         {
            if (string.IsNullOrEmpty( SourceText ))
            {
               return false;
            }
            if (string.IsNullOrEmpty( DurationInSecondsText ) )
            {
               return false;
            }
            return true;
         }
      }

      private string _amplitudeOutput;
      public string AmplitudeOutput
      {
         get => _amplitudeOutput;
         set
         {
            if (_amplitudeOutput != value)
            {
               _amplitudeOutput = value;
               OnPropertyChanged( nameof( AmplitudeOutput ) );
            }
         }
      }

      private void OnPropertyChanged( string propertyName ) => PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

      public event PropertyChangedEventHandler PropertyChanged;
   }
}
