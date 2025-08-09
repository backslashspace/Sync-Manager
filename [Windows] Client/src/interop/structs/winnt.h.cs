using System.Runtime.InteropServices;

#pragma warning disable CS0649

// https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-token_privileges
[StructLayout(LayoutKind.Sequential)]
internal ref struct TOKEN_PRIVILEGES
{
    internal UInt32 PrivilegeCount;
    internal LUID_AND_ATTRIBUTES Privileges;
}

// https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-luid_and_attributes
[StructLayout(LayoutKind.Sequential)]
internal ref struct LUID_AND_ATTRIBUTES
{
    internal LUID Luid;
    internal UInt32 Attributes;
}

// https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-luid
[StructLayout(LayoutKind.Sequential)]
internal ref struct LUID
{
    internal UInt32 LowPart;
    internal UInt32 HighPart;
}