using System;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/ns-ntddk-_file_name_information
[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct FILE_NAME_INFORMATION
{
    internal UInt32 FileNameLength;
    internal fixed Char FileName[1];
}

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/ns-ntddk-_file_attribute_tag_information
[StructLayout(LayoutKind.Sequential)]
internal ref struct FILE_ATTRIBUTE_TAG_INFORMATION
{
    internal UInt32 FileAttributes;
    internal UInt32 ReparseTag;
}