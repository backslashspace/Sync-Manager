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

        ntStatus = NtDll.NtOpenFile(&fileHandle, Constants.FILE_READ_ATTRIBUTES | Constants.SYNCHRONIZE, &objectAttributes, &ioStatusBlock, Constants.FILE_SHARE_READ, Constants.FILE_SYNCHRONOUS_IO_NONALERT | Constants.FILE_OPEN_REPARSE_POINT);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("Failed to open: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtOpenFile");
            return false;
        }
        #endregion

        /****************************************************************************************/

        FILE_ATTRIBUTE_TAG_INFORMATION fileTagInformation;
        ntStatus = NtDll.NtQueryInformationFile(fileHandle, &ioStatusBlock, &fileTagInformation, (UInt32)sizeof(FILE_ATTRIBUTE_TAG_INFORMATION), Constants.FILE_INFORMATION_CLASS.FileAttributeTagInformation);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("FileAttributeTagInformation failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryInformationFile");
            return false;
        }

        ntStatus = NtDll.NtFsControlFile(fileHandle, 0, null, null, &ioStatusBlock, Constants.FSCTL_GET_REPARSE_POINT, null, 0, enumeratorContext->NtFsControlFileWorkingBuffer, EnumeratorContext.NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("FSCTL_GET_REPARSE_POINT failed with: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtFsControlFile");
            return false;
        }

        Boolean isMountPoint = (fileTagInformation.ReparseTag & Constants.IO_REPARSE_TAG_MOUNT_POINT) == Constants.IO_REPARSE_TAG_MOUNT_POINT;
        Boolean isSymLink = (fileTagInformation.ReparseTag & Constants.IO_REPARSE_TAG_SYMLINK) == Constants.IO_REPARSE_TAG_SYMLINK;
        REPARSE_DATA_BUFFER* reparseDataBuffer = (REPARSE_DATA_BUFFER*)enumeratorContext->NtFsControlFileWorkingBuffer;

        /****************************************************************************************/

        Link* link = (Link*)(metaData->LinkInfoBuffer + enumeratorContext->LinkInfoBufferOffset);
        link->Attributes = fileTagInformation.FileAttributes;

        if (isMountPoint)
        {
            UInt32 itemSize = (Link.RAW_SIZE + reparseDataBuffer->MountPointReparseBuffer.SubstituteNameLength + ntPathLengthBytes + 3u) & ~3u;
            link->Type = NodeType.Junction;
            link->NextItemOffset = itemSize;
            link->LinkNtPathLengthBytes = ntPathLengthBytes;
            link->TargetNtPathLengthBytes = reparseDataBuffer->MountPointReparseBuffer.SubstituteNameLength;

            Buffer.MemoryCopy(ntPath, link->Paths, ntPathLengthBytes, ntPathLengthBytes);
            Buffer.MemoryCopy(reparseDataBuffer->MountPointReparseBuffer.PathBuffer + (reparseDataBuffer->MountPointReparseBuffer.SubstituteNameOffset >>> 1), link->Paths + (ntPathLengthBytes >>> 1), reparseDataBuffer->MountPointReparseBuffer.SubstituteNameLength, reparseDataBuffer->MountPointReparseBuffer.SubstituteNameLength);

            enumeratorContext->LinkInfoBufferOffset += itemSize;
        }
        else if (isSymLink)
        {
            UInt32 itemSize = (Link.RAW_SIZE + reparseDataBuffer->SymbolicLinkReparseBuffer.SubstituteNameLength + ntPathLengthBytes + 3u) & ~3u;
            link->NextItemOffset = itemSize;
            link->Type = NodeType.SymbolicLink;
            link->LinkNtPathLengthBytes = ntPathLengthBytes;
            link->TargetNtPathLengthBytes = reparseDataBuffer->SymbolicLinkReparseBuffer.SubstituteNameLength;

            Buffer.MemoryCopy(ntPath, link->Paths, ntPathLengthBytes, ntPathLengthBytes);
            Buffer.MemoryCopy(reparseDataBuffer->SymbolicLinkReparseBuffer.PathBuffer + (reparseDataBuffer->SymbolicLinkReparseBuffer.SubstituteNameOffset >>> 1), link->Paths + (ntPathLengthBytes >>> 1), reparseDataBuffer->SymbolicLinkReparseBuffer.SubstituteNameLength, reparseDataBuffer->SymbolicLinkReparseBuffer.SubstituteNameLength);

            enumeratorContext->LinkInfoBufferOffset += itemSize;
        }
        else
        {
            Log.Debug("ReparseTag was not a SymLink or MountPoint\n", Log.Level.Error, "FILE_ATTRIBUTE_TAG_INFORMATION");
            _ = NtDll.NtClose(fileHandle);
            return false;
        }

        _ = NtDll.NtClose(fileHandle);
        return true;
    }
}