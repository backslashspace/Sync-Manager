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
        internal static String Alias;

        internal static FileVersionInfo FileVersionInfo;

        internal static readonly Byte[] AccentColor = new Byte[3];
        internal static readonly Byte[] TextSelectionColor = new Byte[3];

        internal const String DatabaseName = "FileSync.db";
    }
}