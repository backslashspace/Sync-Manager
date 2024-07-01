using System.Windows;

namespace SyncMan
{
    internal static partial class DWMAPI
    {
        // workaround for render artifacts on win 10
        private static void UpdateWindow()
        {
            if (UI.MainWindow.WindowStyle != WindowStyle.ToolWindow && UI.MainWindow.WindowStyle != WindowStyle.None)
            {
                WindowStyle current = UI.MainWindow.WindowStyle;

                UI.MainWindow.WindowStyle = current switch
                {
                    WindowStyle.SingleBorderWindow => WindowStyle.ThreeDBorderWindow,
                    WindowStyle.ThreeDBorderWindow => WindowStyle.SingleBorderWindow,
                    WindowStyle.ToolWindow => WindowStyle.SingleBorderWindow,
                    _ => current,
                };

                UI.MainWindow.WindowStyle = current;
            }
            else
            {
                ResizeMode current = UI.MainWindow.ResizeMode;

                UI.MainWindow.ResizeMode = current switch
                {
                    ResizeMode.CanResize => ResizeMode.CanMinimize,
                    ResizeMode.NoResize => ResizeMode.CanMinimize,
                    _ => ResizeMode.CanResize
                };

                UI.MainWindow.ResizeMode = current;
            }
        }
    }
}