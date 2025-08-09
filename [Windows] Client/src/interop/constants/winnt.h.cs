namespace BSS.Interop
{
    internal static partial class Constants
    {
        internal const UInt32 SERVICE_WIN32_OWN_PROCESS = 0x00000010u;

        internal const UInt32 PAGE_NOACCESS = 0x01u;
        internal const UInt32 PAGE_READONLY = 0x02u;
        internal const UInt32 PAGE_READWRITE = 0x04u;
        internal const UInt32 PAGE_WRITECOPY = 0x08u;
        internal const UInt32 PAGE_EXECUTE = 0x10u;
        internal const UInt32 PAGE_EXECUTE_READ = 0x20u;
        internal const UInt32 PAGE_EXECUTE_READWRITE = 0x40u;
        internal const UInt32 PAGE_EXECUTE_WRITECOPY = 0x80u;
        internal const UInt32 PAGE_GUARD = 0x100u;
        internal const UInt32 PAGE_NOCACHE = 0x200u;
        internal const UInt32 PAGE_WRITECOMBINE = 0x400u;

        internal const UInt32 MEM_COMMIT = 0x00001000u;
        internal const UInt32 MEM_RESERVE = 0x00002000u;

        internal const UInt32 MEM_LARGE_PAGES = 0x20000000u;

        ////////////////////////////////////////////////////////////////////
        //                                                                //
        //           Token Object Definitions                             //
        //                                                                //
        //                                                                //
        ////////////////////////////////////////////////////////////////////

        // begin_access

        //
        // Token Specific Access Rights.
        //

        internal const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001u;
        internal const UInt32 TOKEN_DUPLICATE = 0x0002u;
        internal const UInt32 TOKEN_IMPERSONATE = 0x0004u;
        internal const UInt32 TOKEN_QUERY = 0x0008u;
        internal const UInt32 TOKEN_QUERY_SOURCE = 0x0010u;
        internal const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020u;
        internal const UInt32 TOKEN_ADJUST_GROUPS = 0x0040u;
        internal const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080u;
        internal const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100u;

        internal const UInt32 TOKEN_ALL_ACCESS_P = STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT; 

        internal const UInt32 TOKEN_ALL_ACCESS = TOKEN_ALL_ACCESS_P | TOKEN_ADJUST_SESSIONID;

        internal const UInt32 TOKEN_READ = STANDARD_RIGHTS_READ | TOKEN_QUERY;

        internal const UInt32 TOKEN_WRITE = STANDARD_RIGHTS_WRITE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT; 

        internal const UInt32 TOKEN_EXECUTE = STANDARD_RIGHTS_EXECUTE; 

        internal const UInt32 TOKEN_TRUST_CONSTRAINT_MASK = STANDARD_RIGHTS_READ | TOKEN_QUERY | TOKEN_QUERY_SOURCE; 

        internal const UInt32 TOKEN_TRUST_ALLOWED_MASK = TOKEN_TRUST_CONSTRAINT_MASK | TOKEN_DUPLICATE | TOKEN_IMPERSONATE;

        internal const UInt32 TOKEN_ACCESS_PSEUDO_HANDLE_WIN8 = TOKEN_QUERY | TOKEN_QUERY_SOURCE; 

        internal const UInt32 TOKEN_ACCESS_PSEUDO_HANDLE = TOKEN_ACCESS_PSEUDO_HANDLE_WIN8;

        ////////////////////////////////////////////////////////////////////////
        //                                                                    //
        //               Privilege Related Data Structures                    //
        //                                                                    //
        ////////////////////////////////////////////////////////////////////////

        // end_ntifs
        // begin_wdm
        //
        // Privilege attributes
        //

        internal const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001u;
        internal const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002u;
        internal const UInt32 SE_PRIVILEGE_REMOVED = 0x00000004u;
        internal const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000u;

        internal const UInt32 SE_PRIVILEGE_VALID_ATTRIBUTES = SE_PRIVILEGE_ENABLED_BY_DEFAULT | SE_PRIVILEGE_ENABLED | SE_PRIVILEGE_REMOVED | SE_PRIVILEGE_USED_FOR_ACCESS;
    }
}