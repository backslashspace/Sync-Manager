using System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-filetime
[StructLayout(LayoutKind.Sequential)]
internal ref struct FILETIME
{
    internal UInt32 dwLowDateTime;
    internal UInt32 dwHighDateTime;
}