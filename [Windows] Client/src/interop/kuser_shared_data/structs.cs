using System.Runtime.InteropServices;

// https://www.nirsoft.net/kernel_struct/vista/KSYSTEM_TIME.html
[StructLayout(LayoutKind.Sequential)]
internal ref struct KSYSTEM_TIME
{
    internal UInt32 LowPart;
    internal UInt32 High1Time;
    internal UInt32 High2Time;
}