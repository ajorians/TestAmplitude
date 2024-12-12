using System.ComponentModel;
using System.Windows.Input;

namespace TestAmplitude
{
   public class MainWindowViewModel : INotifyPropertyChanged
   {
      private IAmplitude _amplitude;

      public MainWindowViewModel()
      {
         APIKey = "";

         AmplitudeOutput = "Output:" + Environment.NewLine;

         TrackEventNameText = "Sample Event Name :)";

         TrackEventWithNumberNameText = "Sample Event W/Num Name :)";
         TrackEventWithNumberNumberText = $"{0}";
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
            if (_amplitude is null)
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
            AmplitudeNetworkCalls amlitudeNetworkCalls = new( APIKey, amplitudeUserIDCreator.GetUserID(), amplitudeDeviceIDCreator.GetDeviceID() );
            _amplitude = new Amplitude( amlitudeNetworkCalls );
            AmplitudeOutput += $"New session with Key ({APIKey}){Environment.NewLine}";

            _amplitude.StartSession();

            _amplitude.OnTrackedEvent += OnTrackedEvent;
         }
         else
         {
            _amplitude.StopSession();
            _amplitude = null;
            AmplitudeOutput += $"Cleared session{Environment.NewLine}";
         }

         OnPropertyChanged( nameof( SessionButtonText ) );
         OnPropertyChanged( nameof( APIKeyEnabled ) );
         OnPropertyChanged( nameof( IsTrackingOptionsVisible ) );
      }

      private void OnTrackedEvent( object sender, TrackedEventArgs e )
      {
         if (e.NumberValue.HasValue)
         {
            AmplitudeOutput += $"Tracked Event: '{e.EventName}' with number: '{e.NumberValue.Value}'{Environment.NewLine}";
         }
         else
         {
            AmplitudeOutput += $"Tracked Event: '{e.EventName}'{Environment.NewLine}";
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

      private void OnTrackEvent()
      {
         _amplitude.TrackEvent( TrackEventNameText );
      }

      private ICommand _trackEventCommand;
      public ICommand TrackEventCommand => _trackEventCommand ??= new RelayCommand( () => OnTrackEvent() );

      //Event With Number

      private string _trackEventWithNumberNameText;
      public string TrackEventWithNumberNameText
      {
         get => _trackEventWithNumberNameText;
         set
         {
            if (_trackEventWithNumberNameText != value)
            {
               _trackEventWithNumberNameText = value;
               OnPropertyChanged( nameof( TrackEventWithNumberNameText ) );
               OnPropertyChanged( nameof( TrackEventWithNumberNameEnabled ) );
            }
         }
      }

      private string _trackEventWithNumberNumberText;
      public string TrackEventWithNumberNumberText
      {
         get => _trackEventWithNumberNumberText;
         set
         {
            if (_trackEventWithNumberNumberText != value)
            {
               _trackEventWithNumberNumberText = value;
               OnPropertyChanged( nameof( TrackEventWithNumberNumberText ) );
               OnPropertyChanged( nameof( TrackEventWithNumberNameEnabled ) );
            }
         }
      }

      private void OnTrackEventWithNumber()
      {
         _amplitude.TrackEventWithNumber( TrackEventWithNumberNameText, int.Parse( TrackEventWithNumberNumberText ) );
      }

      private ICommand _trackEventWithNumberCommand;
      public ICommand TrackEventWithNumberCommand => _trackEventWithNumberCommand ??= new RelayCommand( () => OnTrackEventWithNumber() );

      public bool TrackEventWithNumberNameEnabled
      {
         get
         {
            if (string.IsNullOrEmpty( TrackEventWithNumberNameText ))
            {
               return false;
            }
            if (string.IsNullOrEmpty( TrackEventWithNumberNumberText ))
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

      private void OnPropertyChanged( string propertyName )
      {
         PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
      }
      public event PropertyChangedEventHandler PropertyChanged;
   }
}
