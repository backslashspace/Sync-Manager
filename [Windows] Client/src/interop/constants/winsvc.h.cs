namespace BSS.Interop
{
    internal static partial class Constants
    {
        //
        // Start Type
        //

        internal const UInt32 SERVICE_BOOT_START = 0x00000000u;
        internal const UInt32 SERVICE_SYSTEM_START = 0x00000001u;
        internal const UInt32 SERVICE_AUTO_START = 0x00000002u;
        internal const UInt32 SERVICE_DEMAND_START = 0x00000003u;
        internal const UInt32 SERVICE_DISABLED = 0x00000004u;

        //
        // Value to indicate no change to an optional parameter
        //
        internal const UInt32 SERVICE_NO_CHANGE = 0xffffffffu;

        //
        // Service State -- for Enum Requests (Bit Mask)
        //
        internal const UInt32 SERVICE_ACTIVE = 0x00000001;
        internal const UInt32 SERVICE_INACTIVE = 0x00000002;
        internal const UInt32 SERVICE_STATE_ALL = (SERVICE_ACTIVE | SERVICE_INACTIVE);

        //
        // Controls
        //
        internal const UInt32 SERVICE_CONTROL_STOP = 0x00000001;
        internal const UInt32 SERVICE_CONTROL_PAUSE = 0x00000002;
        internal const UInt32 SERVICE_CONTROL_CONTINUE = 0x00000003;
        internal const UInt32 SERVICE_CONTROL_INTERROGATE = 0x00000004;
        internal const UInt32 SERVICE_CONTROL_SHUTDOWN = 0x00000005;
        internal const UInt32 SERVICE_CONTROL_PARAMCHANGE = 0x00000006;
        internal const UInt32 SERVICE_CONTROL_NETBINDADD = 0x00000007;
        internal const UInt32 SERVICE_CONTROL_NETBINDREMOVE = 0x00000008;
        internal const UInt32 SERVICE_CONTROL_NETBINDENABLE = 0x00000009;
        internal const UInt32 SERVICE_CONTROL_NETBINDDISABLE = 0x0000000A;
        internal const UInt32 SERVICE_CONTROL_DEVICEEVENT = 0x0000000B;
        internal const UInt32 SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0x0000000C;
        internal const UInt32 SERVICE_CONTROL_POWEREVENT = 0x0000000D;
        internal const UInt32 SERVICE_CONTROL_SESSIONCHANGE = 0x0000000E;
        internal const UInt32 SERVICE_CONTROL_PRESHUTDOWN = 0x0000000F;
        internal const UInt32 SERVICE_CONTROL_TIMECHANGE = 0x00000010;
        //#define SERVICE_CONTROL_USER_LOGOFF            0x00000011
        internal const UInt32 SERVICE_CONTROL_TRIGGEREVENT = 0x00000020;
        //reserved for internal use                    0x00000021
        //reserved for internal use                    0x00000050
        internal const UInt32 SERVICE_CONTROL_LOWRESOURCES = 0x00000060;
        internal const UInt32 SERVICE_CONTROL_SYSTEMLOWRESOURCES = 0x00000061;

        //
        // Service State -- for CurrentState
        //
        internal const UInt32 SERVICE_STOPPED = 0x00000001;
        internal const UInt32 SERVICE_START_PENDING = 0x00000002;
        internal const UInt32 SERVICE_STOP_PENDING = 0x00000003;
        internal const UInt32 SERVICE_RUNNING = 0x00000004;
        internal const UInt32 SERVICE_CONTINUE_PENDING = 0x00000005;
        internal const UInt32 SERVICE_PAUSE_PENDING = 0x00000006;
        internal const UInt32 SERVICE_PAUSED = 0x00000007;

        //
        // Controls Accepted  (Bit Mask)
        //
        internal const UInt32 SERVICE_ACCEPT_STOP = 0x00000001;
        internal const UInt32 SERVICE_ACCEPT_PAUSE_CONTINUE = 0x00000002;
        internal const UInt32 SERVICE_ACCEPT_SHUTDOWN = 0x00000004;
        internal const UInt32 SERVICE_ACCEPT_PARAMCHANGE = 0x00000008;
        internal const UInt32 SERVICE_ACCEPT_NETBINDCHANGE = 0x00000010;
        internal const UInt32 SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x00000020;
        internal const UInt32 SERVICE_ACCEPT_POWEREVENT = 0x00000040;
        internal const UInt32 SERVICE_ACCEPT_SESSIONCHANGE = 0x00000080;
        internal const UInt32 SERVICE_ACCEPT_PRESHUTDOWN = 0x00000100;
        internal const UInt32 SERVICE_ACCEPT_TIMECHANGE = 0x00000200;
        internal const UInt32 SERVICE_ACCEPT_TRIGGEREVENT = 0x00000400;
        internal const UInt32 SERVICE_ACCEPT_USER_LOGOFF = 0x00000800;
        // reserved for internal use                   0x00001000                
        internal const UInt32 SERVICE_ACCEPT_LOWRESOURCES = 0x00002000;
        internal const UInt32 SERVICE_ACCEPT_SYSTEMLOWRESOURCES = 0x00004000;

        //
        // Service Control Manager object specific access types
        //
        internal const UInt32 SC_MANAGER_CONNECT = 0x0001;
        internal const UInt32 SC_MANAGER_CREATE_SERVICE = 0x0002;
        internal const UInt32 SC_MANAGER_ENUMERATE_SERVICE = 0x0004;
        internal const UInt32 SC_MANAGER_LOCK = 0x0008;
        internal const UInt32 SC_MANAGER_QUERY_LOCK_STATUS = 0x0010;
        internal const UInt32 SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020;

        internal const UInt32 SC_MANAGER_ALL_ACCESS = (0x000F0000u | SC_MANAGER_CONNECT | SC_MANAGER_CREATE_SERVICE | SC_MANAGER_ENUMERATE_SERVICE | SC_MANAGER_LOCK | SC_MANAGER_QUERY_LOCK_STATUS | SC_MANAGER_MODIFY_BOOT_CONFIG);

        //
        // Service object specific access type
        //
        internal const UInt32 SERVICE_QUERY_CONFIG = 0x0001u;
        internal const UInt32 SERVICE_CHANGE_CONFIG = 0x0002u;
        internal const UInt32 SERVICE_QUERY_STATUS = 0x0004u;
        internal const UInt32 SERVICE_ENUMERATE_DEPENDENTS = 0x0008u;
        internal const UInt32 SERVICE_START = 0x0010u;
        internal const UInt32 SERVICE_STOP = 0x0020u;
        internal const UInt32 SERVICE_PAUSE_CONTINUE = 0x0040u;
        internal const UInt32 SERVICE_INTERROGATE = 0x0080u;
        internal const UInt32 SERVICE_USER_DEFINED_CONTROL = 0x0100u;

        internal const UInt32 SERVICE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG | SERVICE_QUERY_STATUS | SERVICE_ENUMERATE_DEPENDENTS | SERVICE_START | SERVICE_STOP | SERVICE_PAUSE_CONTINUE | SERVICE_INTERROGATE | SERVICE_USER_DEFINED_CONTROL;
    }
}