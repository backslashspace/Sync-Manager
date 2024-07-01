using System.Windows;

namespace SyncMan
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DWMAPI.Initialize();
            
            UI.MainWindow = this;
            UI.Dispatcher = Dispatcher;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ButtonAnimator.Initialize();

            UI.MainWindowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            DWMAPI.SetTheme(UI.MainWindowHandle, true);

            ButtonAnimator.SecondaryButton.Hook(ref UploadButton);
        }

        private void Upload(object sender, RoutedEventArgs e)
        {

        }

        private void Download(object sender, RoutedEventArgs e)
        {
            xGuid guid = xGuid.CreateGuid();
       

        }

        private void SetLocalAlias(object sender, RoutedEventArgs e)
        {

        }
    }
}