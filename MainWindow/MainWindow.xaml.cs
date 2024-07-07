using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SyncMan
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            UI.MainWindow = this;
            UI.Dispatcher = Dispatcher;

            State.FileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            Title += $"v{State.FileVersionInfo.FileVersion}";

            DWMAPI.Initialize();
            Loaded += Initialize;
        }

        private async void Initialize(object sender, RoutedEventArgs e)
        {
            UI.MainWindowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            DWMAPI.SetTheme(UI.MainWindowHandle, true);
            DWMAPI.SetCaptionColor(UI.MainWindowHandle, Util.RGB_To_COLORREF(0x20u, 0x20u, 0x20u));
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.RGB_To_COLORREF(127, 127, 127));

            ButtonAnimator.Initialize();
            ButtonAnimator.SecondaryButton.Hook(UploadButton);
            ButtonAnimator.SecondaryButton.Hook(DownloadButton);
            ButtonAnimator.SecondaryButton.Hook(AliasButton);

            LogTextBox.Document = new();
            UI.MainWindow.LogTextBox.Document.Blocks.Add(paragraph);

            await ObtainConfiguration().ConfigureAwait(true);

            UploadButton.IsEnabled = true;
            DownloadButton.IsEnabled = true;
            AliasButton.IsEnabled = true;
        }

        // ###############################################################









        private void Upload(object sender, RoutedEventArgs e)
        {
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.RGB_To_COLORREF(96, 125, 146));
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.RGB_To_COLORREF(110, 134, 104));
        }

        private void SetLocalAlias(object sender, RoutedEventArgs e)
        {
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.RGB_To_COLORREF(127, 127, 127));
        }
    }
}