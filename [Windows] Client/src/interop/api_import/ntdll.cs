using System;
using System.Runtime.InteropServices;

namespace BSS.Interop
{
    internal static partial class NtDll
    {
        // https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntcreatefile
        [LibraryImport("ntdll", EntryPoint = "NtCreateFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtCreateFile(Handle* fileHandle, UInt32 desiredAccess, OBJECT_ATTRIBUTES* objectAttributes, IO_STATUS_BLOCK* ioStatusBlock, UInt64 allocationSize, UInt32 fileAttributes, UInt32 shareAccess, UInt32 createDisposition, UInt32 createOptions, void* eaBuffer, UInt32 eaLength);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-zwopenfile
        [LibraryImport("ntdll", EntryPoint = "NtOpenFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtOpenFile(Handle* fileHandle, UInt32 desiredAccess, OBJECT_ATTRIBUTES* objectAttributes, IO_STATUS_BLOCK* ioStatusBlock, UInt32 shareAccess, UInt32 openOptions);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntwritefile
        [LibraryImport("ntdll", EntryPoint = "NtWriteFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtWriteFile(Handle fileHandle, Handle _event, void* apcRoutine, void* apcContext, IO_STATUS_BLOCK* ioStatusBlock, Byte* buffer, UInt32 length, UInt64 byteOffset, UInt32* key);

        // https://learn.microsoft.com/en-us/windows/win32/devnotes/ntreadfile
        [LibraryImport("ntdll", EntryPoint = "NtReadFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtReadFile(Handle fileHandle, Handle _event, void* apcRoutine, void* apcContext, IO_STATUS_BLOCK* ioStatusBlock, Byte* buffer, UInt32 length, UInt64 byteOffset, UInt32* key);

        // https://learn.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntclose
        [LibraryImport("ntdll", EntryPoint = "NtClose", SetLastError = false)]
        internal unsafe static partial NtStatus NtClose(Handle handle);

        /********************************************************/

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-rtltimetotimefields
        [LibraryImport("ntdll", EntryPoint = "RtlTimeToTimeFields", SetLastError = false)]
        internal unsafe static partial void RtlTimeToTimeFields(UInt64* Time, TIME_FIELDS* TimeFields);

        /********************************************************/

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntquerydirectoryfile
        [LibraryImport("ntdll", EntryPoint = "NtQueryDirectoryFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtQueryDirectoryFile(Handle FileHandle, Handle Event, void* ApcRoutine, void* ApcContext, IO_STATUS_BLOCK* IoStatusBlock, void* FileInformation, UInt32 Length, Constants.FILE_INFORMATION_CLASS FileInformationClass, [MarshalAs(UnmanagedType.U1)] Boolean ReturnSingleEntry, UNICODE_STRING* FileName, [MarshalAs(UnmanagedType.U1)] Boolean RestartScan);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntqueryinformationfile
        [LibraryImport("ntdll", EntryPoint = "NtQueryInformationFile", SetLastError = false)]
        internal unsafe static partial NtStatus NtQueryInformationFile(Handle FileHandle, IO_STATUS_BLOCK* IoStatusBlock, void* FileInformation, UInt32 Length, Constants.FILE_INFORMATION_CLASS FileInformationClass);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-zwcreateevent
        [LibraryImport("ntdll", EntryPoint = "NtCreateEvent", SetLastError = false)]
        internal unsafe static partial NtStatus NtCreateEvent(Handle* EveHandle, UInt32 DesiredAccess, OBJECT_ATTRIBUTES* ObjectAttributes, Constants.EVENT_TYPE EventType, [MarshalAs(UnmanagedType.U1)] Boolean InitialState);

        // https://learn.microsoft.com/de-de/windows/win32/api/winternl/nf-winternl-ntwaitforsingleobject
        [LibraryImport("ntdll", EntryPoint = "NtWaitForSingleObject", SetLastError = false)]
        internal unsafe static partial NtStatus NtWaitForSingleObject(Handle Handle, [MarshalAs(UnmanagedType.U1)] Boolean Alertable, UInt64 Timeout);

        /********************************************************/

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntallocatevirtualmemory
        [LibraryImport("ntdll", EntryPoint = "NtAllocateVirtualMemory", SetLastError = false)]
        internal unsafe static partial NtStatus NtAllocateVirtualMemory(Handle ProcessHandle, void** BaseAddress, UInt64 ZeroBits, UInt64* RegionSize, UInt32 AllocationType, UInt32 Protect);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntfreevirtualmemory
        [LibraryImport("ntdll", EntryPoint = "NtFreeVirtualMemory", SetLastError = false)]
        internal unsafe static partial NtStatus NtFreeVirtualMemory(Handle ProcessHandle, void** BaseAddress, UInt64* RegionSize, UInt32 FreeType);

        /********************************************************/

        // https://ntdoc.m417z.com/ntadjustprivilegestoken
        [LibraryImport("ntdll", EntryPoint = "NtAdjustPrivilegesToken", SetLastError = false)]
        internal unsafe static partial NtStatus NtAdjustPrivilegesToken(Handle TokenHandle, [MarshalAs(UnmanagedType.U1)] Boolean DisableAllPrivileges, TOKEN_PRIVILEGES* NewState, UInt32 BufferLength, TOKEN_PRIVILEGES* PreviousState, UInt32* ReturnLength);

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/nf-ntifs-ntopenprocesstoken
        [LibraryImport("ntdll", EntryPoint = "NtOpenProcessToken", SetLastError = false)]
        internal unsafe static partial NtStatus NtOpenProcessToken(Handle ProcessHandle, UInt32 DesiredAccess, Handle* TokenHandle);
    }
}