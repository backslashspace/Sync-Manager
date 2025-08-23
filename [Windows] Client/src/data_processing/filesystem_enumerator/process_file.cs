using System;
using BSS.Interop;
using System.Runtime.CompilerServices;

internal static partial class FilesystemEnumerator
{
    private unsafe static Boolean ProcessFile(Char* ntPath, UInt16 ntPathLengthBytes, EnumeratorContext* enumeratorContext, MetaData* metaData, UInt32* crc32c)
    {
        return true;

        #region OPEN_FILE
        Handle fileHandle;
        NtStatus ntStatus;
        IO_STATUS_BLOCK ioStatusBlock;

        UNICODE_STRING unicodeString;
        unicodeString.Length = ntPathLengthBytes;
        unicodeString.MaximumLength = ntPathLengthBytes;
        unicodeString.Buffer = ntPath;

        OBJECT_ATTRIBUTES objectAttributes;
        objectAttributes.Length = (UInt32)sizeof(OBJECT_ATTRIBUTES);
        objectAttributes.RootDirectory = 0ul;
        objectAttributes.ObjectName = &unicodeString;
        objectAttributes.Attributes = Constants.OBJ_CASE_INSENSITIVE;
        objectAttributes.SecurityDescriptor = null;
        objectAttributes.SecurityQualityOfService = null;

        ntStatus = NtDll.NtOpenFile(&fileHandle, Constants.FILE_READ_ATTRIBUTES | Constants.FILE_READ_DATA | Constants.SYNCHRONIZE, &objectAttributes, &ioStatusBlock, Constants.FILE_SHARE_READ, Constants.FILE_SYNCHRONOUS_IO_NONALERT | Constants.FILE_SEQUENTIAL_ONLY);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("Failed to open: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtOpenFile");
            return false;
        }
        #endregion

        /****************************************************************************************/

        FILE_STAT_INFORMATION fileStatInformation;
        ntStatus = NtDll.NtQueryInformationFile(fileHandle, &ioStatusBlock, &fileStatInformation, (UInt32)sizeof(FILE_STAT_INFORMATION), Constants.FILE_INFORMATION_CLASS.FileStatInformation);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("FileAttributeTagInformation failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryInformationFile");
            return false;
        }

        if (fileStatInformation.NumberOfLinks > 1)
        {
            // check if already seen
        }












        _ = NtDll.NtClose(fileHandle);
        return true;
    }
}