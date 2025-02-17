﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SyncMan
{
    public sealed partial class MainWindow : Window
    {
        private static Boolean IsBussy = false;

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
            DWMAPI.SetCaptionColor(UI.MainWindowHandle, Util.COLORREFFromRGB(0x20u, 0x20u, 0x20u));
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.COLORREFFromRGB(127, 127, 127));

            ButtonAnimator.Initialize();
            ButtonAnimator.SecondaryButton.Hook(UploadButton);
            ButtonAnimator.SecondaryButton.Hook(DownloadButton);
            ButtonAnimator.SecondaryButton.Hook(AliasButton);

            LogTextBox.Document = new();
            UI.MainWindow.LogTextBox.Document.Blocks.Add(paragraph);

            await ObtainConfiguration().ConfigureAwait(true);

            Util.GetAccentColors();
            Application.Current.Resources["DarkIdleAccentColor"] = new SolidColorBrush(Color.FromArgb(0xff, State.AccentColor[0], State.AccentColor[1], State.AccentColor[2]));
            Application.Current.Resources["TextSelectionColor"] = new SolidColorBrush(Color.FromArgb(0xff, State.TextSelectionColor[0], State.TextSelectionColor[1], State.TextSelectionColor[2]));

            UploadButton.IsEnabled = true;
            DownloadButton.IsEnabled = true;
            AliasButton.IsEnabled = true;
        }

        // ###############################################################

        private async void Upload(object sender, RoutedEventArgs e)
        {
            if (IsBussy) return;
            IsBussy = true;

            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.COLORREFFromRGB(96, 125, 146));

            await Task.Run(static() => Backend.Upload()).ConfigureAwait(false);

            IsBussy = false;
        }

        private async void Download(object sender, RoutedEventArgs e)
        {
            if (IsBussy) return;
            IsBussy = true;

            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.COLORREFFromRGB(110, 134, 104));

            await Task.Run(static() => Backend.Download()).ConfigureAwait(false);

            IsBussy = false;
        }

        private void SetLocalAlias(object sender, RoutedEventArgs e)
        {
            DWMAPI.SetBorderColor(UI.MainWindowHandle, Util.COLORREFFromRGB(127, 127, 127));

            Backend.SetLocalAlias();
        }
    }
}