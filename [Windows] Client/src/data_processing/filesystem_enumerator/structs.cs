using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct MetaData
{
    internal Byte* DirectoryInfoBuffer;
    internal UInt64 DirectoryInfoBufferSize;
    internal Byte* LinkInfoBuffer;
    internal UInt64 LinkInfoBufferSize;
    internal UInt64* UsedDirectoryInfoBufferLength;
    internal UInt64* UsedLinkInfoBufferLength;
}

[StructLayout(LayoutKind.Sequential)]
internal ref struct EnumeratorResult
{
    internal UInt64 AddedSize;
    internal UInt32 NumberOfItems;
    internal Boolean SawDirectory;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct EnumeratorStackFrame
{
    internal Node* Node;
    internal UInt32 ItemIndex;
    internal UInt32 NumberOfItems;
    internal UInt16 PathPopLengthBytes;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct EnumeratorContext
{
    internal const UInt32 NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE = 131_072u; // 32 pages
    internal const UInt32 NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE = 69_632u; // 17 pages

    internal Byte* NtQueryDirectoryFileWorkingBuffer;
    internal Byte* NtFsControlFileWorkingBuffer;
    internal Handle SynchronizationEventHandle;

    internal UInt64 DirectoryInfoBufferOffset;
    internal UInt64 LinkInfoBufferOffset;
}

//

internal enum NodeType : UInt16
{
    File = 0,
    HardLink = 1,
    Junction = 2,
    Directory = 3,
    SymLinkFile = 4,
    SymLinkDirectory = 5,
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct Link
{
    internal const UInt32 RAW_SIZE = 10u; // used to pad (align) | all members but Name

    internal NodeType Type;
    internal UInt16 TargetNtPathLength;
    internal UInt32 LinkAttributes;
    internal UInt16 LinkNtPathLength;
    internal unsafe fixed Char Paths[1];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct Node
{
    internal Directory* ParentDirectory;
    internal UInt64 NextItemOffset;
    internal NodeType Type;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct File
{
    internal const UInt32 RAW_SIZE = 36u; // used to pad (align) | all members but Name

    internal Directory* ParentDirectory;
    internal UInt64 NextItemOffset;
    internal NodeType Type;
    internal UInt16 NameLengthBytes;
    internal UInt32 Attributes;
    internal UInt64 Size;
    internal UInt32 CRC32C;
    internal fixed Char Name[1];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe ref struct Directory
{
    internal const UInt32 RAW_SIZE = 24u; // used to pad (align) | all members but Name

    internal Directory* ParentDirectory;
    internal UInt64 NextItemOffset;
    internal NodeType Type;
    internal UInt16 NameLengthBytes;
    internal UInt32 Attributes;
    internal fixed Char Name[1];
}

/****************************************************************************************/

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

[StructLayout(LayoutKind.Sequential)]
internal ref struct MountPointReparseBuffer
{
    internal UInt16 SubstituteNameOffset;
    internal UInt16 SubstituteNameLength;
    internal UInt16 PrintNameOffset;
    internal UInt16 PrintNameLength;
    internal unsafe fixed Char PathBuffer[1];
}

/****************************************************************************************/

//#pragma pack(2)
//typedef struct alignas(8) Node
//{
//    unsigned __int64 NextItemOffset;
//    unsigned __int16 Type;
//
//    union
//    {
//        struct
//        {
//            unsigned __int16 NameLengthBytes;
//            unsigned __int32 CRC32C;
//            Node* ParentDirectory;
//            unsigned __int64 Size;
//            unsigned __int32 Attributes;
//            wchar_t Name[1];
//        } File;
//
//        struct
//        {
//            unsigned __int16 NameLengthBytes;
//            unsigned __int32 Attributes;
//            Node* ParentDirectory;
//            wchar_t Name[1];
//        } Directory;
//    };
//};
//#pragma pack(pop)