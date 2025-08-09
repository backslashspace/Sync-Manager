using System;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/ns-wdm-_io_status_block
[StructLayout(LayoutKind.Explicit)]
internal ref struct IO_STATUS_BLOCK
{
    [FieldOffset(0)]
    internal NtStatus Status;
    [FieldOffset(0)]
    internal unsafe void* Pointer;

    [FieldOffset(8)]
    internal UInt64 Information;
}

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/ns-wdm-_file_standard_information
internal ref struct FILE_STANDARD_INFORMATION
{
    internal UInt64 AllocationSize;
    internal UInt64 EndOfFile;
    internal UInt32 NumberOfLinks;
    internal Boolean DeletePending;
    internal Boolean Directory;
}