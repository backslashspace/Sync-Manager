namespace BSS.Interop
{
    internal static partial class Constants
    {
        internal const UInt32 ENABLE_PROCESSED_INPUT = 0x0001u;
        internal const UInt32 ENABLE_LINE_INPUT = 0x0002u;
        internal const UInt32 ENABLE_ECHO_INPUT = 0x0004u;
        internal const UInt32 ENABLE_WINDOW_INPUT = 0x0008u;
        internal const UInt32 ENABLE_MOUSE_INPUT = 0x0010u;
        internal const UInt32 ENABLE_INSERT_MODE = 0x0020u;
        internal const UInt32 ENABLE_QUICK_EDIT_MODE = 0x0040u;
        internal const UInt32 ENABLE_EXTENDED_FLAGS = 0x0080u;
        internal const UInt32 ENABLE_AUTO_POSITION = 0x0100u;
        internal const UInt32 ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200u;

        //                                                        
        // Output Mode flags:                                     
        //                                                        

        internal const UInt32 ENABLE_PROCESSED_OUTPUT = 0x0001u;
        internal const UInt32 ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002u;
        internal const UInt32 ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004u;
        internal const UInt32 DISABLE_NEWLINE_AUTO_RETURN = 0x0008u;
        internal const UInt32 ENABLE_LVB_GRID_WORLDWIDE = 0x0010u;
    }
}