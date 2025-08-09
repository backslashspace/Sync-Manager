using System;
using System.Runtime.InteropServices;

/********************************************************/

namespace BSS.Interop
{
    internal static partial class Kernel32
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew
        [LibraryImport("Kernel32", EntryPoint = "CreateFileW", SetLastError = true)]
        internal unsafe static partial Handle CreateFileW(Char* lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, void* lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, Handle hTemplateFile);

        // https://learn.microsoft.com/en-us/windows/win32/api/ioapiset/nf-ioapiset-deviceiocontrol
        [LibraryImport("Kernel32", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean DeviceIoControl(Handle hDevice, UInt32 dwIoControlCode, void* lpInBuffer, UInt32 nInBufferSize, void* lpOutBuffer, UInt32 nOutBufferSize, UInt32* lpBytesReturned, void* lpOverlapped);

        // https://learn.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle
        [LibraryImport("Kernel32", EntryPoint = "CloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean CloseHandle(Handle hObject);

        /********************************************************/

        // https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-findfirstfilew
        [LibraryImport("Kernel32", EntryPoint = "FindFirstFileW", SetLastError = true)]
        internal unsafe static partial Handle FindFirstFileW(Char* lpFileName, WIN32_FIND_DATAW* lpFindFileData);

        // https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-findnextfilew
        [LibraryImport("Kernel32", EntryPoint = "FindNextFileW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean FindNextFileW(Handle hFindFile, WIN32_FIND_DATAW* lpFindFileData);

        // https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-findclose
        [LibraryImport("Kernel32", EntryPoint = "FindClose", SetLastError = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean FindClose(Handle hFindFile);

        /********************************************************/

        // https://learn.microsoft.com/en-us/windows/console/allocconsole
        [LibraryImport("Kernel32", EntryPoint = "AllocConsole", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean AllocConsole();

        // https://learn.microsoft.com/en-us/windows/console/getstdhandle
        [LibraryImport("Kernel32", EntryPoint = "GetStdHandle", SetLastError = true)]
        internal unsafe static partial Handle GetStdHandle(UInt32 nStdHandle);

        // https://learn.microsoft.com/en-us/windows/console/getconsolemode
        [LibraryImport("Kernel32", EntryPoint = "GetConsoleMode", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean GetConsoleMode(Handle hConsoleHandle, UInt32* lpMode);

        // https://learn.microsoft.com/en-us/windows/console/setconsolemode
        [LibraryImport("Kernel32", EntryPoint = "SetConsoleMode", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static partial Boolean SetConsoleMode(Handle hConsoleHandle, UInt32 dwMode);
    }
}