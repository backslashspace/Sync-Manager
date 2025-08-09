namespace BSS.Interop
{
    internal static partial class Constants
    {
        // https://learn.microsoft.com/en-us/windows/console/getstdhandle
        internal const UInt32 STD_INPUT_HANDLE = unchecked((UInt32)(-10));
        internal const UInt32 STD_OUTPUT_HANDLE = unchecked((UInt32)(-11));
        internal const UInt32 STD_ERROR_HANDLE = unchecked((UInt32)(-12));

    }
}