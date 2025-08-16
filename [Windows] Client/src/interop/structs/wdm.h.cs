using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;

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

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/ns-wdm-time_fields
internal ref struct TIME_FIELDS
{
    internal UInt16 Year;        // range [1601...]
    internal UInt16 Month;       // range [1..12]
    internal UInt16 Day;         // range [1..31]
    internal UInt16 Hour;        // range [0..23]
    internal UInt16 Minute;      // range [0..59]
    internal UInt16 Second;      // range [0..59]
    internal UInt16 Milliseconds;// range [0..999]
    internal UInt16 Weekday;     // range [0..6] == [Sunday..Saturday]
}