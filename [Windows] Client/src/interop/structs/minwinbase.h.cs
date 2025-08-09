using System;
using System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-win32_find_dataw
[StructLayout(LayoutKind.Sequential)]
internal ref struct WIN32_FIND_DATAW
{
    internal readonly UInt32 dwFileAttributes;
    internal readonly FILETIME ftCreationTime;
    internal readonly FILETIME ftLastAccessTime;
    internal readonly FILETIME ftLastWriteTime;
    internal readonly UInt32 nFileSizeHigh;
    internal readonly UInt32 nFileSizeLow;
    internal readonly UInt32 dwReserved0;
    internal readonly UInt32 dwReserved1;
    internal unsafe fixed Char cFileName[260];
    internal unsafe fixed Char cAlternateFileName[14];
}