using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace SyncMan
{
    internal class LogBox
    {
        internal static void Add(String text = null, SolidColorBrush foreground = null, SolidColorBrush background = null, Boolean scrollToEnd = true, FontWeight fontWeight = default)
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
