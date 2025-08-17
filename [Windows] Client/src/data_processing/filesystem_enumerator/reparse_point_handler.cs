using System;
using BSS.Interop;

internal static partial class FilesystemEnumerator
{
    private unsafe static Boolean HandleReparsePoint(Char* ntPath, UInt16 ntPathLengthBytes, EnumeratorContext* enumeratorContext, MetaData* metaData)
    {
        #region OPEN_LINK
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

        ntStatus = NtDll.NtOpenFile(&fileHandle, Constants.FILE_READ_ATTRIBUTES | Constants.FILE_READ_EA | Constants.SYNCHRONIZE, &objectAttributes, &ioStatusBlock, Constants.FILE_SHARE_READ, Constants.FILE_SYNCHRONOUS_IO_NONALERT | Constants.FILE_OPEN_REPARSE_POINT);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("Failed to open: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtOpenFile");
            return false;
        }
        #endregion

        FILE_ATTRIBUTE_TAG_INFORMATION fileTagInformation;
        ntStatus = NtDll.NtQueryInformationFile(fileHandle, &ioStatusBlock, &fileTagInformation, (UInt32)sizeof(FILE_ATTRIBUTE_TAG_INFORMATION), Constants.FILE_INFORMATION_CLASS.FileAttributeTagInformation);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("FileAttributeTagInformation failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryInformationFile");
            return false;
        }

        if ((fileTagInformation.ReparseTag & Constants.IO_REPARSE_TAG_MOUNT_POINT) == Constants.IO_REPARSE_TAG_MOUNT_POINT)
        {
            ntStatus = NtDll.NtFsControlFile(fileHandle, 0, null, null, &ioStatusBlock, Constants.FSCTL_GET_REPARSE_POINT, null, 0, enumeratorContext->NtFsControlFileWorkingBuffer, EnumeratorContext.NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE);
            if (ntStatus != Constants.STATUS_SUCCESS)
            {
                Log.Debug("FSCTL_GET_REPARSE_POINT failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtFsControlFile");
                return false;
            }

            REPARSE_DATA_BUFFER* reparseDataBuffer = (REPARSE_DATA_BUFFER*)enumeratorContext->NtFsControlFileWorkingBuffer;



        }
        else if ((fileTagInformation.ReparseTag & Constants.IO_REPARSE_TAG_SYMLINK) == Constants.IO_REPARSE_TAG_SYMLINK)
        {
            ntStatus = NtDll.NtFsControlFile(fileHandle, 0, null, null, &ioStatusBlock, Constants.FSCTL_GET_REPARSE_POINT, null, 0, enumeratorContext->NtFsControlFileWorkingBuffer, EnumeratorContext.NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE);
            if (ntStatus != Constants.STATUS_SUCCESS)
            {
                Log.Debug("Failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryInformationFile");
                return false;
            }

            REPARSE_DATA_BUFFER* reparseDataBuffer = (REPARSE_DATA_BUFFER*)enumeratorContext->NtFsControlFileWorkingBuffer;
        }
        else
        {
            throw new Exception("weird reparse tag found");
        }







        _ = NtDll.NtClose(fileHandle);

        return false;
    }

    
}