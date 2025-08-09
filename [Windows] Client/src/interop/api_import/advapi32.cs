using System;
using System.Runtime.InteropServices;

namespace BSS.Interop
{
    internal static partial class Advapi32
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocesstoken
        [LibraryImport("Advapi32", EntryPoint = "OpenProcessToken", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean OpenProcessToken(Handle ProcessHandle, UInt32 DesiredAccess, Handle* TokenHandle);

        // https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-lookupprivilegevaluew
        [LibraryImport("Advapi32", EntryPoint = "LookupPrivilegeValueW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean LookupPrivilegeValueW(Char* lpSystemName, Char* lpName, LUID* lpLuid);

        // https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-adjusttokenprivileges
        [LibraryImport("Advapi32", EntryPoint = "AdjustTokenPrivileges", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean AdjustTokenPrivileges(Handle TokenHandle, [MarshalAs(UnmanagedType.U1)] Boolean DisableAllPrivileges, TOKEN_PRIVILEGES* NewState, UInt32 BufferLength, TOKEN_PRIVILEGES* PreviousState, UInt32* ReturnLength);
    }
}