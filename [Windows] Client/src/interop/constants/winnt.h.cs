namespace BSS.Interop
{
    internal static partial class Constants
    {
        internal const UInt32 SERVICE_WIN32_OWN_PROCESS = 0x00000010u;

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

        // end_access
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
        internal const UInt32 PAGE_GRAPHICS_NOACCESS = 0x0800u;
        internal const UInt32 PAGE_GRAPHICS_READONLY = 0x1000u;
        internal const UInt32 PAGE_GRAPHICS_READWRITE = 0x2000u;
        internal const UInt32 PAGE_GRAPHICS_EXECUTE = 0x4000u;
        internal const UInt32 PAGE_GRAPHICS_EXECUTE_READ = 0x8000u;
        internal const UInt32 PAGE_GRAPHICS_EXECUTE_READWRITE = 0x10000u;
        internal const UInt32 PAGE_GRAPHICS_COHERENT = 0x20000u;
        internal const UInt32 PAGE_GRAPHICS_NOCACHE = 0x40000u;
        internal const UInt32 PAGE_ENCLAVE_THREAD_CONTROL = 0x80000000u;
        internal const UInt32 PAGE_REVERT_TO_FILE_MAP = 0x80000000u;
        internal const UInt32 PAGE_TARGETS_NO_UPDATE = 0x40000000u;
        internal const UInt32 PAGE_TARGETS_INVALID = 0x40000000u;
        internal const UInt32 PAGE_ENCLAVE_UNVALIDATED = 0x20000000u;
        internal const UInt32 PAGE_ENCLAVE_MASK = 0x10000000u;
        internal const UInt32 PAGE_ENCLAVE_DECOMMIT = PAGE_ENCLAVE_MASK | 0u;
        internal const UInt32 PAGE_ENCLAVE_SS_FIRST = PAGE_ENCLAVE_MASK | 1u;
        internal const UInt32 PAGE_ENCLAVE_SS_REST = PAGE_ENCLAVE_MASK | 2u;

        internal const UInt32 MEM_COMMIT = 0x00001000u;
        internal const UInt32 MEM_RESERVE = 0x00002000u;
        internal const UInt32 MEM_REPLACE_PLACEHOLDER = 0x00004000u;
        internal const UInt32 MEM_RESERVE_PLACEHOLDER = 0x00040000u;
        internal const UInt32 MEM_RESET = 0x00080000u;
        internal const UInt32 MEM_TOP_DOWN = 0x00100000u;
        internal const UInt32 MEM_WRITE_WATCH = 0x00200000u;
        internal const UInt32 MEM_PHYSICAL = 0x00400000u;
        internal const UInt32 MEM_ROTATE = 0x00800000u;
        internal const UInt32 MEM_DIFFERENT_IMAGE_BASE_OK = 0x00800000u;
        internal const UInt32 MEM_RESET_UNDO = 0x01000000u;
        internal const UInt32 MEM_LARGE_PAGES = 0x20000000u;
        internal const UInt32 MEM_4MB_PAGES = 0x80000000u;
        internal const UInt32 MEM_64K_PAGES = MEM_LARGE_PAGES | MEM_PHYSICAL;

        internal const UInt32 MEM_UNMAP_WITH_TRANSIENT_BOOST = 0x00000001u;
        internal const UInt32 MEM_COALESCE_PLACEHOLDERS = 0x00000001u;
        internal const UInt32 MEM_PRESERVE_PLACEHOLDER = 0x00000002u;
        internal const UInt32 MEM_DECOMMIT = 0x00004000u;
        internal const UInt32 MEM_RELEASE = 0x00008000u;
        internal const UInt32 MEM_FREE = 0x00010000u;

        internal const UInt32 MEM_EXTENDED_PARAMETER_GRAPHICS = 0x00000001u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_NONPAGED = 0x00000002u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_ZERO_PAGES_OPTIONAL = 0x00000004u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_NONPAGED_LARGE = 0x00000008u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_NONPAGED_HUGE = 0x00000010u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_SOFT_FAULT_PAGES = 0x00000020u;
        internal const UInt32 MEM_EXTENDED_PARAMETER_EC_CODE = 0x00000040u;

        //
        // The reparse tags are a DWORD. The 32 bits are laid out as follows:
        //
        //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
        //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
        //  +-+-+-+-+-----------------------+-------------------------------+
        //  |M|R|N|D|     Reserved bits     |       Reparse Tag Value       |
        //  +-+-+-+-+-----------------------+-------------------------------+
        //
        // M is the Microsoft bit. When set to 1, it denotes a tag owned by Microsoft.
        //   All ISVs must use a tag with a 0 in this position.
        //   Note: If a Microsoft tag is used by non-Microsoft software, the
        //   behavior is not defined.
        //
        // R is reserved.  Must be zero for non-Microsoft tags.
        //
        // N is name surrogate. When set to 1, the file represents another named
        //   entity in the system.
        //
        // D is the directory bit. When set to 1, indicates that any directory
        //   with this reparse tag can have children. Has no special meaning when used
        //   on a non-directory file. Not compatible with the name surrogate bit.
        //
        // The M and N bits are OR-able.
        // The following macros check for the M and N bit values:
        //

        //
        // Macro to determine whether a reparse point tag corresponds to a tag
        // owned by Microsoft.
        //

        internal const UInt32 IO_REPARSE_TAG_RESERVED_INVALID = 0xC0008000u;
        internal const UInt32 IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003u;
        internal const UInt32 IO_REPARSE_TAG_HSM = 0xC0000004u;
        internal const UInt32 IO_REPARSE_TAG_HSM2 = 0x80000006u;
        internal const UInt32 IO_REPARSE_TAG_SIS = 0x80000007u;
        internal const UInt32 IO_REPARSE_TAG_WIM = 0x80000008u;
        internal const UInt32 IO_REPARSE_TAG_CSV = 0x80000009u;
        internal const UInt32 IO_REPARSE_TAG_DFS = 0x8000000Au;
        internal const UInt32 IO_REPARSE_TAG_SYMLINK = 0xA000000Cu;
        internal const UInt32 IO_REPARSE_TAG_DFSR = 0x80000012u;
        internal const UInt32 IO_REPARSE_TAG_DEDUP = 0x80000013u;
        internal const UInt32 IO_REPARSE_TAG_NFS = 0x80000014u;
        internal const UInt32 IO_REPARSE_TAG_FILE_PLACEHOLDER = 0x80000015u;
        internal const UInt32 IO_REPARSE_TAG_WOF = 0x80000017u;
        internal const UInt32 IO_REPARSE_TAG_WCI = 0x80000018u;
        internal const UInt32 IO_REPARSE_TAG_WCI_1 = 0x90001018u;
        internal const UInt32 IO_REPARSE_TAG_GLOBAL_REPARSE = 0xA0000019u;
        internal const UInt32 IO_REPARSE_TAG_CLOUD = 0x9000001Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_1 = 0x9000101Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_2 = 0x9000201Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_3 = 0x9000301Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_4 = 0x9000401Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_5 = 0x9000501Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_6 = 0x9000601Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_7 = 0x9000701Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_8 = 0x9000801Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_9 = 0x9000901Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_A = 0x9000A01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_B = 0x9000B01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_C = 0x9000C01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_D = 0x9000D01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_E = 0x9000E01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_F = 0x9000F01Au;
        internal const UInt32 IO_REPARSE_TAG_CLOUD_MASK = 0x0000F000u;
        internal const UInt32 IO_REPARSE_TAG_APPEXECLINK = 0x8000001Bu;
        internal const UInt32 IO_REPARSE_TAG_PROJFS = 0x9000001Cu;
        internal const UInt32 IO_REPARSE_TAG_STORAGE_SYNC = 0x8000001Eu;
        internal const UInt32 IO_REPARSE_TAG_WCI_TOMBSTONE = 0xA000001Fu;
        internal const UInt32 IO_REPARSE_TAG_UNHANDLED = 0x80000020u;
        internal const UInt32 IO_REPARSE_TAG_ONEDRIVE = 0x80000021u;
        internal const UInt32 IO_REPARSE_TAG_PROJFS_TOMBSTONE = 0xA0000022u;
        internal const UInt32 IO_REPARSE_TAG_AF_UNIX = 0x80000023u;
        internal const UInt32 IO_REPARSE_TAG_STORAGE_SYNC_FOLDER = 0x90000027u;
        internal const UInt32 IO_REPARSE_TAG_WCI_LINK = 0xA0000027u;
        internal const UInt32 IO_REPARSE_TAG_WCI_LINK_1 = 0xA0001027u;
        internal const UInt32 IO_REPARSE_TAG_DATALESS_CIM = 0xA0000028u;

    }
}