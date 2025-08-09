using System;
using System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/winsvc/ns-winsvc-service_status
[StructLayout(LayoutKind.Sequential)]
internal ref struct SERVICE_STATUS
{
    internal UInt32 dwServiceType;
    internal UInt32 dwCurrentState;
    internal UInt32 dwControlsAccepted;
    internal UInt32 dwWin32ExitCode;
    internal UInt32 dwServiceSpecificExitCode;
    internal UInt32 dwCheckPoint;
    internal UInt32 dwWaitHint;
}

// https://learn.microsoft.com/de-de/windows/win32/api/winsvc/ns-winsvc-service_table_entryw
[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct SERVICE_TABLE_ENTRYW
{
    internal Char* lpServiceName;
    internal delegate* unmanaged<UInt32, Char**, void> lpServiceProc;
}
