namespace BSS.Interop
{
    internal static partial class Constants
    {
        // "C:\Program Files (x86)\Windows Kits\10\Include\10.0.26100.0\shared\ntdef.h"
        internal enum EVENT_TYPE : UInt32
        {
            NotificationEvent,
            SynchronizationEvent
        }

        //
        // Valid values for the Attributes field
        //

        internal const UInt32 OBJ_INHERIT = 0x00000002u;
        internal const UInt32 OBJ_PERMANENT = 0x00000010u;
        internal const UInt32 OBJ_EXCLUSIVE = 0x00000020u;
        internal const UInt32 OBJ_CASE_INSENSITIVE = 0x00000040u;
        internal const UInt32 OBJ_OPENIF = 0x00000080u;
        internal const UInt32 OBJ_OPENLINK = 0x00000100u;
        internal const UInt32 OBJ_KERNEL_HANDLE = 0x00000200u;
        internal const UInt32 OBJ_FORCE_ACCESS_CHECK = 0x00000400u;
        internal const UInt32 OBJ_IGNORE_IMPERSONATED_DEVICEMAP = 0x00000800u;
        internal const UInt32 OBJ_DONT_REPARSE = 0x00001000u;
        internal const UInt32 OBJ_VALID_ATTRIBUTES = 0x00001FF2u;
    }
}