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

    internal struct MachineAlias
    {
        public MachineAlias()
        {
            Alias = $"{Environment.MachineName}:{Environment.UserName}";
            IsUserDefined = false;
        }

        internal String Alias;
        internal Boolean IsUserDefined;
    }

    internal static class State
    {
        internal static Int64 MachineID;
        internal static MachineAlias MachineAlias = new();

        internal static FileVersionInfo FileVersionInfo;

        internal static readonly Byte[] AccentColor = new Byte[3];
        internal static readonly Byte[] TextSelectionColor = new Byte[3];

        internal const String DatabaseName = "FileSync.db";
        internal const String DatabaseVersion = "1.2.0.0";
        internal const UInt16 TargetTableCount = 4;     // version, transaction, state, machine
    }
}