using System.Windows;

namespace TestAmplitude
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         DataContext = new MainWindowViewModel();
         InitializeComponent();

         Closing += MainWindow_Closing;
      }

      private void MainWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e )
      {
         (DataContext as MainWindowViewModel).OnClosing( sender, e );
      }
   }
}