using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace SyncMan
{
    internal class LogBox
    {
        internal enum LogLevel : Byte
        {
            Message = 0,
            Info = 1,
            Warn = 2,
            Error = 3
        }

        internal static void Add(String text, LogLevel messageType = LogLevel.Message)
        {
            SolidColorBrush foreground = messageType switch
            {
                LogLevel.Message => Brushes.White,
                LogLevel.Info => Brushes.LightCyan,
                LogLevel.Warn => Brushes.LightGoldenrodYellow,
                LogLevel.Error => Brushes.Red,
                _ => null,
            };

            UI.Dispatcher.Invoke(new Action(() => MainWindow.LogBoxAdd(text, foreground, null, true, default)));
        }

        internal static void Add(String text, SolidColorBrush foreground, SolidColorBrush background = null, Boolean scrollToEnd = true, FontWeight fontWeight = default)
        {
            UI.Dispatcher.Invoke(new Action(() => MainWindow.LogBoxAdd(text, foreground, background, scrollToEnd, fontWeight)));
        }

        internal static void Remove(UInt32 amount = 1)
        {
            UI.Dispatcher.Invoke(new Action(() => MainWindow.LogBoxRemoveLine(amount)));
        }
    }

    public sealed partial class MainWindow
    {
        internal static readonly Paragraph paragraph = new()
        {
            KeepWithNext = true,
            Margin = new Thickness(0, 0, 0, 0)
        };

        internal static void LogBoxAdd(String text, SolidColorBrush foreground, SolidColorBrush background, Boolean scrollToEnd, FontWeight fontWeight)
        {
            Run run = new(text)
            {
                Foreground = foreground ??= Brushes.LightGray,
                Background = background,
                FontWeight = fontWeight
            };

            paragraph.Inlines.Add(run);
 
            if (scrollToEnd == true)
            {
                UI.MainWindow.LogScrollViewer.ScrollToEnd();
            }
        }

        internal static void LogBoxRemoveLine(UInt32 amount = 1)
        {
            for (UInt32 i = 0; i < amount; i++)
            {
                paragraph.Inlines.Remove(paragraph.Inlines.LastInline);
            }
        }
    }
}
