using System;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct FILE_DIRECTORY_INFORMATION
{
    internal UInt32 NextEntryOffset;
    internal UInt32 FileIndex;
    internal UInt64 CreationTime;
    internal UInt64 LastAccessTime;
    internal UInt64 LastWriteTime;
    internal UInt64 ChangeTime;
    internal UInt64 EndOfFile;
    internal UInt64 AllocationSize;
    internal UInt32 FileAttributes;
    internal UInt32 FileNameLength;
    internal fixed Char FileName[1];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct FILE_STAT_INFORMATION
{
    internal UInt64 FileId;
    internal UInt64 CreationTime;
    internal UInt64 LastAccessTime;
    internal UInt64 LastWriteTime;
    internal UInt64 ChangeTime;
    internal UInt64 AllocationSize;
    internal UInt64 EndOfFile;
    internal UInt32 FileAttributes;
    internal UInt32 ReparseTag;
    internal UInt32 NumberOfLinks;
    internal Byte EffectiveAccess;
}

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/ns-ntifs-_reparse_data_buffer
[StructLayout(LayoutKind.Explicit)]
internal ref struct REPARSE_DATA_BUFFER
{
    [FieldOffset(0)]
    internal UInt32 ReparseTag;
    [FieldOffset(4)]
    internal UInt16 ReparseDataLength;
    [FieldOffset(6)]
    internal UInt16 Reserved;

    [FieldOffset(8)]
    internal SymbolicLinkReparseBuffer SymbolicLinkReparseBuffer;
    [FieldOffset(8)]
    internal MountPointReparseBuffer MountPointReparseBuffer;
    [FieldOffset(8)]
    internal unsafe fixed Byte GenericReparseBuffer[1];
}

// REPARSE_DATA_BUFFER member
[StructLayout(LayoutKind.Sequential)]
internal ref struct SymbolicLinkReparseBuffer
{
    internal UInt16 SubstituteNameOffset;
    internal UInt16 SubstituteNameLength;
    internal UInt16 PrintNameOffset;
    internal UInt16 PrintNameLength;
    internal UInt32 Flags;
    internal unsafe fixed Char PathBuffer[1];
}

// REPARSE_DATA_BUFFER member
[StructLayout(LayoutKind.Sequential)]
internal ref struct MountPointReparseBuffer
{
    internal UInt16 SubstituteNameOffset;
    internal UInt16 SubstituteNameLength;
    internal UInt16 PrintNameOffset;
    internal UInt16 PrintNameLength;
    internal unsafe fixed Char PathBuffer[1];
}