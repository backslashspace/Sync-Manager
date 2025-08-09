using System;

#pragma warning disable CS0649

// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/ns-ntifs-_file_id_full_dir_information
internal unsafe ref struct FILE_ID_FULL_DIR_INFORMATION
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
    internal UInt32 EaSize;
    internal UInt64 FileId;
    internal fixed Char FileName[1];
}