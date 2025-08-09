using System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-filetime
[StructLayout(LayoutKind.Sequential)]
internal readonly ref struct FILETIME
{
    internal readonly UInt32 dwLowDateTime;
    internal readonly UInt32 dwHighDateTime;
}