namespace BSS.Interop
{
    internal static partial class Constants
    {
        //
        //  The following are masks for the predefined standard access types
        //

        internal const UInt32 DELETE = 0x00010000u;
        internal const UInt32 READ_CONTROL = 0x00020000u;
        internal const UInt32 WRITE_DAC = 0x00040000u;
        internal const UInt32 WRITE_OWNER = 0x00080000u;
        internal const UInt32 SYNCHRONIZE = 0x00100000u;

        internal const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000u;

        internal const UInt32 STANDARD_RIGHTS_READ = READ_CONTROL;
        internal const UInt32 STANDARD_RIGHTS_WRITE = READ_CONTROL;
        internal const UInt32 STANDARD_RIGHTS_EXECUTE = READ_CONTROL;

        internal const UInt32 STANDARD_RIGHTS_ALL = 0x001F0000u;

        internal const UInt32 SPECIFIC_RIGHTS_ALL = 0x0000FFFFu;

        //
        // AccessSystemAcl access type
        //

        internal const UInt32 ACCESS_SYSTEM_SECURITY = 0x01000000u;

        //
        // MaximumAllowed access type
        //

        internal const UInt32 MAXIMUM_ALLOWED = 0x02000000u;

        //
        // Define the create disposition values
        //

        internal const UInt32 FILE_SUPERSEDE = 0x00000000u;
        internal const UInt32 FILE_OPEN = 0x00000001u;
        internal const UInt32 FILE_CREATE = 0x00000002u;
        internal const UInt32 FILE_OPEN_IF = 0x00000003u;
        internal const UInt32 FILE_OVERWRITE = 0x00000004u;
        internal const UInt32 FILE_OVERWRITE_IF = 0x00000005u;
        internal const UInt32 FILE_MAXIMUM_DISPOSITION = 0x00000005u;

        //
        //  These are the generic rights.
        //

        internal const UInt32 GENERIC_READ = 0x80000000u;
        internal const UInt32 GENERIC_WRITE = 0x40000000u;
        internal const UInt32 GENERIC_EXECUTE = 0x20000000u;
        internal const UInt32 GENERIC_ALL = 0x10000000u;

        //
        // Event Specific Access Rights.
        //

        internal const UInt32 EVENT_QUERY_STATE = 0x0001u;
        internal const UInt32 EVENT_MODIFY_STATE = 0x0002u;
        internal const UInt32 EVENT_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x3u;

        //
        // Define the create/open option flags
        //

        internal const UInt32 FILE_DIRECTORY_FILE = 0x00000001u;
        internal const UInt32 FILE_WRITE_THROUGH = 0x00000002u;
        internal const UInt32 FILE_SEQUENTIAL_ONLY = 0x00000004u;
        internal const UInt32 FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008u;

        internal const UInt32 FILE_SYNCHRONOUS_IO_ALERT = 0x00000010u;
        internal const UInt32 FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020u;
        internal const UInt32 FILE_NON_DIRECTORY_FILE = 0x00000040u;
        internal const UInt32 FILE_CREATE_TREE_CONNECTION = 0x00000080u;

        internal const UInt32 FILE_COMPLETE_IF_OPLOCKED = 0x00000100u;
        internal const UInt32 FILE_NO_EA_KNOWLEDGE = 0x00000200u;
        internal const UInt32 FILE_OPEN_REMOTE_INSTANCE = 0x00000400u;
        internal const UInt32 FILE_RANDOM_ACCESS = 0x00000800u;

        internal const UInt32 FILE_DELETE_ON_CLOSE = 0x00001000u;
        internal const UInt32 FILE_OPEN_BY_FILE_ID = 0x00002000u;
        internal const UInt32 FILE_OPEN_FOR_BACKUP_INTENT = 0x00004000u;
        internal const UInt32 FILE_NO_COMPRESSION = 0x00008000u;

        internal const UInt32 FILE_OPEN_REQUIRING_OPLOCK = 0x00010000u;

        internal const UInt32 FILE_RESERVE_OPFILTER = 0x00100000u;
        internal const UInt32 FILE_OPEN_REPARSE_POINT = 0x00200000u;
        internal const UInt32 FILE_OPEN_NO_RECALL = 0x00400000u;
        internal const UInt32 FILE_OPEN_FOR_FREE_SPACE_QUERY = 0x00800000u;

        internal const UInt32 FILE_VALID_OPTION_FLAGS = 0x00ffffffu;
        internal const UInt32 FILE_VALID_PIPE_OPTION_FLAGS = 0x00000032u;
        internal const UInt32 FILE_VALID_MAILSLOT_OPTION_FLAGS = 0x00000032u;
        internal const UInt32 FILE_VALID_SET_FLAGS = 0x00000036u;

        // https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/ne-wdm-_file_information_class
        internal enum FILE_INFORMATION_CLASS : UInt32
        {
            FileDirectoryInformation = 1,
            FileFullDirectoryInformation,                   // 2
            FileBothDirectoryInformation,                   // 3
            FileBasicInformation,                           // 4
            FileStandardInformation,                        // 5
            FileInternalInformation,                        // 6
            FileEaInformation,                              // 7
            FileAccessInformation,                          // 8
            FileNameInformation,                            // 9
            FileRenameInformation,                          // 10
            FileLinkInformation,                            // 11
            FileNamesInformation,                           // 12
            FileDispositionInformation,                     // 13
            FilePositionInformation,                        // 14
            FileFullEaInformation,                          // 15
            FileModeInformation,                            // 16
            FileAlignmentInformation,                       // 17
            FileAllInformation,                             // 18
            FileAllocationInformation,                      // 19
            FileEndOfFileInformation,                       // 20
            FileAlternateNameInformation,                   // 21
            FileStreamInformation,                          // 22
            FilePipeInformation,                            // 23
            FilePipeLocalInformation,                       // 24
            FilePipeRemoteInformation,                      // 25
            FileMailslotQueryInformation,                   // 26
            FileMailslotSetInformation,                     // 27
            FileCompressionInformation,                     // 28
            FileObjectIdInformation,                        // 29
            FileCompletionInformation,                      // 30
            FileMoveClusterInformation,                     // 31
            FileQuotaInformation,                           // 32
            FileReparsePointInformation,                    // 33
            FileNetworkOpenInformation,                     // 34
            FileAttributeTagInformation,                    // 35
            FileTrackingInformation,                        // 36
            FileIdBothDirectoryInformation,                 // 37
            FileIdFullDirectoryInformation,                 // 38
            FileValidDataLengthInformation,                 // 39
            FileShortNameInformation,                       // 40
            FileIoCompletionNotificationInformation,        // 41
            FileIoStatusBlockRangeInformation,              // 42
            FileIoPriorityHintInformation,                  // 43
            FileSfioReserveInformation,                     // 44
            FileSfioVolumeInformation,                      // 45
            FileHardLinkInformation,                        // 46
            FileProcessIdsUsingFileInformation,             // 47
            FileNormalizedNameInformation,                  // 48
            FileNetworkPhysicalNameInformation,             // 49
            FileIdGlobalTxDirectoryInformation,             // 50
            FileIsRemoteDeviceInformation,                  // 51
            FileUnusedInformation,                          // 52
            FileNumaNodeInformation,                        // 53
            FileStandardLinkInformation,                    // 54
            FileRemoteProtocolInformation,                  // 55

            //
            //  These are special versions of these operations (defined earlier)
            //  which can be used by kernel mode drivers only to bypass security
            //  access checks for Rename and HardLink operations.  These operations
            //  are only recognized by the IOManager, a file system should never
            //  receive these.
            //

            FileRenameInformationBypassAccessCheck,         // 56
            FileLinkInformationBypassAccessCheck,           // 57

            //
            // End of special information classes reserved for IOManager.
            //

            FileVolumeNameInformation,                      // 58
            FileIdInformation,                              // 59
            FileIdExtdDirectoryInformation,                 // 60
            FileReplaceCompletionInformation,               // 61
            FileHardLinkFullIdInformation,                  // 62
            FileIdExtdBothDirectoryInformation,             // 63
            FileDispositionInformationEx,                   // 64
            FileRenameInformationEx,                        // 65
            FileRenameInformationExBypassAccessCheck,       // 66
            FileDesiredStorageClassInformation,             // 67
            FileStatInformation,                            // 68
            FileMemoryPartitionInformation,                 // 69
            FileStatLxInformation,                          // 70
            FileCaseSensitiveInformation,                   // 71
            FileLinkInformationEx,                          // 72
            FileLinkInformationExBypassAccessCheck,         // 73
            FileStorageReserveIdInformation,                // 74
            FileCaseSensitiveInformationForceAccessCheck,   // 75
            FileKnownFolderInformation,                     // 76
            FileStatBasicInformation,                       // 77
            FileId64ExtdDirectoryInformation,               // 78
            FileId64ExtdBothDirectoryInformation,           // 79
            FileIdAllExtdDirectoryInformation,              // 80
            FileIdAllExtdBothDirectoryInformation,          // 81
            FileStreamReservationInformation,               // 82

            //
            //  It is an internal special request.
            //  This operation should only be issued to the IOManager
            //  through the Filter Manager. NtSetInformationFile/NtQueryInformationFile
            //  and a file system should never receive this.
            //

            FileMupProviderInfo,                            // 83

            //
            // End of special information classes reserved for filterMgr.
            //

            FileMaximumInformation
        }

        //
        // Define access rights to files and directories
        //

        //
        // The FILE_READ_DATA and FILE_WRITE_DATA constants are also defined in
        // devioctl.h as FILE_READ_ACCESS and FILE_WRITE_ACCESS. The values for these
        // constants *MUST* always be in sync.
        // The values are redefined in devioctl.h because they must be available to
        // both DOS and NT.
        //

        internal const UInt32 FILE_READ_DATA = 0x0001u;   // file & pipe
        internal const UInt32 FILE_LIST_DIRECTORY = 0x0001u;   // directory

        internal const UInt32 FILE_WRITE_DATA = 0x0002u;    // file & pipe
        internal const UInt32 FILE_ADD_FILE = 0x0002u;   // directory

        internal const UInt32 FILE_APPEND_DATA = 0x0004u;    // file
        internal const UInt32 FILE_ADD_SUBDIRECTORY = 0x0004u;   // directory
        internal const UInt32 FILE_CREATE_PIPE_INSTANCE = 0x0004u;   // named pipe

        internal const UInt32 FILE_READ_EA = 0x0008u;   // file & directory

        internal const UInt32 FILE_WRITE_EA = 0x0010u;  // file & directory

        internal const UInt32 FILE_EXECUTE = 0x0020u;   // file
        internal const UInt32 FILE_TRAVERSE = 0x0020u;    // directory

        internal const UInt32 FILE_DELETE_CHILD = 0x0040u;    // directory

        internal const UInt32 FILE_READ_ATTRIBUTES = 0x0080u;    // all

        internal const UInt32 FILE_WRITE_ATTRIBUTES = 0x0100u;   // all

        internal const UInt32 FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FFu;

        internal const UInt32 FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE;

        internal const UInt32 FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE;

        internal const UInt32 FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE;

        // end_access end_winnt

        //
        // Define share access rights to files and directories
        //

        internal const UInt32 FILE_SHARE_READ = 0x00000001u;
        internal const UInt32 FILE_SHARE_WRITE = 0x00000002u;
        internal const UInt32 FILE_SHARE_DELETE = 0x00000004u;

        //
        // Define the file attributes values
        //
        // Note:  0x00000008 is reserved for use for the old DOS VOLID (volume ID)
        //        and is therefore not considered valid in NT.
        //
        // Note:  Note also that the order of these flags is set to allow both the
        //        FAT and the Pinball File Systems to directly set the attributes
        //        flags in attributes words without having to pick each flag out
        //        individually.  The order of these flags should not be changed!
        //

        internal const UInt32 FILE_ATTRIBUTE_READONLY = 0x00000001u;
        internal const UInt32 FILE_ATTRIBUTE_HIDDEN = 0x00000002u;
        internal const UInt32 FILE_ATTRIBUTE_SYSTEM = 0x00000004u;
        internal const UInt32 FILE_ATTRIBUTE_DIRECTORY = 0x00000010u;
        internal const UInt32 FILE_ATTRIBUTE_ARCHIVE = 0x00000020u;
        internal const UInt32 FILE_ATTRIBUTE_DEVICE = 0x00000040u;
        internal const UInt32 FILE_ATTRIBUTE_NORMAL = 0x00000080u;
        internal const UInt32 FILE_ATTRIBUTE_TEMPORARY = 0x00000100u;
        internal const UInt32 FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200u;
        internal const UInt32 FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400u;
        internal const UInt32 FILE_ATTRIBUTE_COMPRESSED = 0x00000800u;
        internal const UInt32 FILE_ATTRIBUTE_OFFLINE = 0x00001000u;
        internal const UInt32 FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000u;
        internal const UInt32 FILE_ATTRIBUTE_ENCRYPTED = 0x00004000u;
        internal const UInt32 FILE_ATTRIBUTE_INTEGRITY_STREAM = 0x00008000u;
        internal const UInt32 FILE_ATTRIBUTE_VIRTUAL = 0x00010000u;
        internal const UInt32 FILE_ATTRIBUTE_NO_SCRUB_DATA = 0x00020000u;
        internal const UInt32 FILE_ATTRIBUTE_EA = 0x00040000u;
        internal const UInt32 FILE_ATTRIBUTE_PINNED = 0x00080000u;
        internal const UInt32 FILE_ATTRIBUTE_UNPINNED = 0x00100000u;
        internal const UInt32 FILE_ATTRIBUTE_RECALL_ON_OPEN = 0x00040000u;
        internal const UInt32 FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS = 0x00400000u;

        internal const UInt32 TREE_CONNECT_ATTRIBUTE_PRIVACY = 0x00004000u;
        internal const UInt32 TREE_CONNECT_ATTRIBUTE_INTEGRITY = 0x00008000u;
        internal const UInt32 TREE_CONNECT_ATTRIBUTE_GLOBAL = 0x00000004u;
        internal const UInt32 TREE_CONNECT_ATTRIBUTE_PINNED = 0x00000002u;
        internal const UInt32 FILE_ATTRIBUTE_STRICTLY_SEQUENTIAL = 0x20000000u;
        internal const UInt32 FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001u;
        internal const UInt32 FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002u;
        internal const UInt32 FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004u;
        internal const UInt32 FILE_NOTIFY_CHANGE_SIZE = 0x00000008u;
        internal const UInt32 FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010u;
        internal const UInt32 FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020u;
        internal const UInt32 FILE_NOTIFY_CHANGE_CREATION = 0x00000040u;
        internal const UInt32 FILE_NOTIFY_CHANGE_SECURITY = 0x00000100u;
        internal const UInt32 FILE_ACTION_ADDED = 0x00000001u;
        internal const UInt32 FILE_ACTION_REMOVED = 0x00000002u;
        internal const UInt32 FILE_ACTION_MODIFIED = 0x00000003u;
        internal const UInt32 FILE_ACTION_RENAMED_OLD_NAME = 0x00000004u;
        internal const UInt32 FILE_ACTION_RENAMED_NEW_NAME = 0x00000005u;
        internal const UInt32 MAILSLOT_NO_MESSAGE = UInt32.MaxValue;
        internal const UInt32 MAILSLOT_WAIT_FOREVER = UInt32.MaxValue;
        internal const UInt32 FILE_CASE_SENSITIVE_SEARCH = 0x00000001u;
        internal const UInt32 FILE_CASE_PRESERVED_NAMES = 0x00000002u;
        internal const UInt32 FILE_UNICODE_ON_DISK = 0x00000004u;
        internal const UInt32 FILE_PERSISTENT_ACLS = 0x00000008u;
        internal const UInt32 FILE_FILE_COMPRESSION = 0x00000010u;
        internal const UInt32 FILE_VOLUME_QUOTAS = 0x00000020u;
        internal const UInt32 FILE_SUPPORTS_SPARSE_FILES = 0x00000040u;
        internal const UInt32 FILE_SUPPORTS_REPARSE_POINTS = 0x00000080u;
        internal const UInt32 FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100u;
        internal const UInt32 FILE_RETURNS_CLEANUP_RESULT_INFO = 0x00000200u;
        internal const UInt32 FILE_SUPPORTS_POSIX_UNLINK_RENAME = 0x00000400u;
        internal const UInt32 FILE_SUPPORTS_BYPASS_IO = 0x00000800u;
        internal const UInt32 FILE_SUPPORTS_STREAM_SNAPSHOTS = 0x00001000u;
        internal const UInt32 FILE_SUPPORTS_CASE_SENSITIVE_DIRS = 0x00002000u;

        internal const UInt32 FILE_VOLUME_IS_COMPRESSED = 0x00008000u;
        internal const UInt32 FILE_SUPPORTS_OBJECT_IDS = 0x00010000u;
        internal const UInt32 FILE_SUPPORTS_ENCRYPTION = 0x00020000u;
        internal const UInt32 FILE_NAMED_STREAMS = 0x00040000u;
        internal const UInt32 FILE_READ_ONLY_VOLUME = 0x00080000u;
        internal const UInt32 FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000u;
        internal const UInt32 FILE_SUPPORTS_TRANSACTIONS = 0x00200000u;
        internal const UInt32 FILE_SUPPORTS_HARD_LINKS = 0x00400000u;
        internal const UInt32 FILE_SUPPORTS_EXTENDED_ATTRIBUTES = 0x00800000u;
        internal const UInt32 FILE_SUPPORTS_OPEN_BY_FILE_ID = 0x01000000u;
        internal const UInt32 FILE_SUPPORTS_USN_JOURNAL = 0x02000000u;
        internal const UInt32 FILE_SUPPORTS_INTEGRITY_STREAMS = 0x04000000u;
        internal const UInt32 FILE_SUPPORTS_BLOCK_REFCOUNTING = 0x08000000u;
        internal const UInt32 FILE_SUPPORTS_SPARSE_VDL = 0x10000000u;
        internal const UInt32 FILE_DAX_VOLUME = 0x20000000u;
        internal const UInt32 FILE_SUPPORTS_GHOSTING = 0x40000000u;

        internal const UInt64 FILE_INVALID_FILE_ID = UInt64.MaxValue;

        //
        // Define the I/O status information return values for NtCreateFile/NtOpenFile
        //

        internal const UInt32 FILE_SUPERSEDED = 0x00000000u;
        internal const UInt32 FILE_OPENED = 0x00000001u;
        internal const UInt32 FILE_CREATED = 0x00000002u;
        internal const UInt32 FILE_OVERWRITTEN = 0x00000003u;
        internal const UInt32 FILE_EXISTS = 0x00000004u;
        internal const UInt32 FILE_DOES_NOT_EXIST = 0x00000005u;
    }
}