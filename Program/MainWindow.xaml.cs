using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace SyncMan
{
    public sealed partial class MainWindow : Window
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

            Util.GetAccentColors();
            Application.Current.Resources["DarkIdleAccentColor"] = new SolidColorBrush(Color.FromArgb(0xff, State.AccentColor[0], State.AccentColor[1], State.AccentColor[2]));

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

            InputBox.InputBox messageBox = new("Machine Alias", "Global machine identifier Tag", "<name>", State.Alias, "Set");
            messageBox.ShowDialog();

            if (messageBox.Result == null)
            {
                return;
            }
            else
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Sync Manager", "Alias", messageBox.Result, RegistryValueKind.String);
                LogBox.Add($"Set machine alias to: {messageBox.Result}\n");
            }
        }
    }
}