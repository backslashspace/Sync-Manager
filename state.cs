using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace SyncMan
{
    internal static class UI
    {
        internal static IntPtr MainWindowHandle;

        internal static MainWindow MainWindow;
        internal static Dispatcher Dispatcher;
    }

    internal static class State
    {
        internal static Guid MachineGuid;

        internal static FileVersionInfo FileVersionInfo;
    }
}