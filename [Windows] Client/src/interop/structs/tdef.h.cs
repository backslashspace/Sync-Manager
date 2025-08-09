using System;
using System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/subauth/ns-subauth-unicode_string
[StructLayout(LayoutKind.Sequential)]
internal ref struct UNICODE_STRING
{
    internal UInt16 Length;
    internal UInt16 MaximumLength;
    internal unsafe Char* Buffer;
}

// https://learn.microsoft.com/en-us/windows/win32/api/ntdef/ns-ntdef-_object_attributes
[StructLayout(LayoutKind.Sequential)]
internal ref struct OBJECT_ATTRIBUTES
{
    internal UInt32 Length;
    internal Handle RootDirectory;
    internal unsafe UNICODE_STRING* ObjectName;
    internal UInt32 Attributes;
    internal unsafe void* SecurityDescriptor;
    internal unsafe void* SecurityQualityOfService;
}