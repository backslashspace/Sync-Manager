using System;
using BSS.Interop;
using System.Runtime.CompilerServices;

internal static partial class FilesystemEnumerator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static Boolean ProcessFile(Char* ntPath, UInt16 ntPathLengthBytes, InternalEnumeratorContext* internalEnumeratorContext, ExternalEnumeratorContext* externalEnumeratorContext, UInt64 parentNodeBaseOffset, FILE_DIRECTORY_INFORMATION* fileDirectoryInformation, UInt64* enumeratorResult_AddedSize)
    {
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
            _ = NtDll.NtClose(fileHandle);
            return false;
        }

        /****************************************************************************************/

        UInt32 itemSize;
        UInt64 currentNodeOffset = internalEnumeratorContext->DirectoryInfoBufferOffset + *enumeratorResult_AddedSize;
        void* currentNode = externalEnumeratorContext->DirectoryInfoBuffer + internalEnumeratorContext->DirectoryInfoBufferOffset + *enumeratorResult_AddedSize;








        if (fileStatInformation.NumberOfLinks > 1)
        {
            Boolean alreadySeen = false;
            UInt64 hardLinkMasterBaseOffset = currentNodeOffset;
            if (!ProcessHardLink(internalEnumeratorContext, externalEnumeratorContext, enumeratorResult_AddedSize, fileStatInformation.FileId, &alreadySeen, &hardLinkMasterBaseOffset))
            {
                _ = NtDll.NtClose(fileHandle);
                return false;
            }

            if (alreadySeen)
            {
                itemSize = (HardLinkSlave.RAW_SIZE + fileDirectoryInformation->FileNameLength + 7u) & ~7u;

                HardLinkSlave* hardLinkSlave = (HardLinkSlave*)currentNode;
                hardLinkSlave->NextItemOffset = itemSize;
                hardLinkSlave->Type = NodeType.HardLinkSlave;
                hardLinkSlave->ParentDirectoryBaseOffset = parentNodeBaseOffset;
                hardLinkSlave->HardLinkMasterBaseOffset = hardLinkMasterBaseOffset;
                hardLinkSlave->Attributes = fileDirectoryInformation->FileAttributes;
                hardLinkSlave->NameLengthBytes = (UInt16)fileDirectoryInformation->FileNameLength;
                Buffer.MemoryCopy(fileDirectoryInformation->FileName, hardLinkSlave->Name, fileDirectoryInformation->FileNameLength, fileDirectoryInformation->FileNameLength);

                *enumeratorResult_AddedSize += itemSize;

                _ = NtDll.NtClose(fileHandle);
                return true;
            }
        }

        itemSize = (File.RAW_SIZE + fileDirectoryInformation->FileNameLength + 7u) & ~7u;

        File* file = (File*)currentNode;
        file->NextItemOffset = itemSize;
        file->Type = NodeType.File;
        file->Size = fileDirectoryInformation->EndOfFile;
        file->ParentDirectoryBaseOffset = parentNodeBaseOffset;
        file->Attributes = fileDirectoryInformation->FileAttributes;
        file->NameLengthBytes = (UInt16)fileDirectoryInformation->FileNameLength;
        Buffer.MemoryCopy(fileDirectoryInformation->FileName, file->Name, fileDirectoryInformation->FileNameLength, fileDirectoryInformation->FileNameLength);

        *enumeratorResult_AddedSize += itemSize;

        // digest file

        _ = NtDll.NtClose(fileHandle);
        return true;
    }





    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static Boolean ProcessHardLink(InternalEnumeratorContext* internalEnumeratorContext, ExternalEnumeratorContext* externalEnumeratorContext, UInt64* enumeratorResult_AddedSize, UInt64 fileId, Boolean* alreadySeen, UInt64* hardLinkMasterBaseOffset)
    {
        // scan
        UInt64 index = 0;

        if (internalEnumeratorContext->HardLinkBufferOffset == 0)
        {
            goto ADD_MASTER;
        }

    NEXT:
        if (externalEnumeratorContext->HardLinkWorkingBuffer[index].FileId == fileId)
        {
            *alreadySeen = true;
            *hardLinkMasterBaseOffset = externalEnumeratorContext->HardLinkWorkingBuffer[index].HardLinkMasterBaseOffset;
            return true;
        }

        if (internalEnumeratorContext->HardLinkBufferOffset > index)
        {
            ++index;
            goto NEXT;
        }

    ADD_MASTER:
        if (internalEnumeratorContext->HardLinkBufferOffset == externalEnumeratorContext->HardLinkWorkingBufferSize)
        {
            Log.Debug("HardLinkWorkingBufferSize to small, can not continue - Increase buffer size and try again: allocation size: " + externalEnumeratorContext->HardLinkWorkingBufferSize + ", (could not fit another element)\n", Log.Level.Error, "ProcessHardLink");
            return false;
        }

        externalEnumeratorContext->HardLinkWorkingBuffer[internalEnumeratorContext->HardLinkBufferOffset].FileId = fileId;
        externalEnumeratorContext->HardLinkWorkingBuffer[internalEnumeratorContext->HardLinkBufferOffset].HardLinkMasterBaseOffset = *hardLinkMasterBaseOffset;
        ++internalEnumeratorContext->HardLinkBufferOffset;

        return true;
    }

}