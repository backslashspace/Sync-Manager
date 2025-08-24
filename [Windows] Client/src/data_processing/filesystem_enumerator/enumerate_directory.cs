using System;
using BSS.Interop;
using System.Runtime.CompilerServices;

internal static partial class FilesystemEnumerator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static Boolean EnumerateDirectory(Char* ntPath, UInt16 ntPathLengthBytes, UInt64 parentNodeBaseOffset, InternalEnumeratorContext* internalEnumeratorContext, ExternalEnumeratorContext* externalEnumeratorContext, EnumeratorResult* enumeratorResult)
    {
        #region OPEN_DIRECTORY
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

        ntStatus = NtDll.NtOpenFile(&fileHandle, Constants.FILE_LIST_DIRECTORY | Constants.FILE_TRAVERSE, &objectAttributes, &ioStatusBlock, Constants.FILE_SHARE_READ | Constants.FILE_SHARE_WRITE | Constants.FILE_SHARE_DELETE, Constants.FILE_DIRECTORY_FILE);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("Failed to open directory: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtOpenFile");
            return false;
        }
        #endregion

        enumeratorResult->AddedSize = 0;
        enumeratorResult->NumberOfItems = 0;
        enumeratorResult->SawSubDirectory = false;
        Byte* directoryInfoBuffer = externalEnumeratorContext->DirectoryInfoBuffer + internalEnumeratorContext->DirectoryInfoBufferOffset;

    /****************************************************************************************/

    QUERY:
        if (internalEnumeratorContext->DirectoryInfoBufferOffset + InternalEnumeratorContext.NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE > externalEnumeratorContext->DirectoryInfoBufferSize)
        {
            Log.Debug("DirectoryInfoBufferSize to small, can not continue - Increase buffer size and try again: allocation size: " + externalEnumeratorContext->DirectoryInfoBufferSize + ", (could not fit another working buffer)\n", Log.Level.Error, "EnumerateDirectory");
            _ = NtDll.NtClose(fileHandle);
            return false;
        }

        ntStatus = NtDll.NtQueryDirectoryFile(fileHandle, internalEnumeratorContext->SynchronizationEventHandle, null, null, &ioStatusBlock, internalEnumeratorContext->NtQueryDirectoryFileWorkingBuffer, InternalEnumeratorContext.NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE, Constants.FILE_INFORMATION_CLASS.FileDirectoryInformation, false, null, false);
        if (ntStatus == Constants.STATUS_PENDING)
        {
            ntStatus = NtDll.NtWaitForSingleObject(internalEnumeratorContext->SynchronizationEventHandle, false, 0ul);
            if (ntStatus != Constants.STATUS_SUCCESS)
            {
                Log.Debug("WaitForSingleObject failed: " + ntStatus.ToString("X") + "\n", Log.Level.Error, "NtQueryDirectoryFile");
                _ = NtDll.NtClose(fileHandle);
                return false;
            }

            ntStatus = ioStatusBlock.Status;
        }

        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            if (ntStatus == Constants.STATUS_NO_MORE_FILES)
            {
                _ = NtDll.NtClose(fileHandle);
                return true;
            }
            else
            {
                Log.Debug("NtQueryDirectoryFile failed: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryDirectoryFile");
                _ = NtDll.NtClose(fileHandle);
                return false;
            }
        }

        UInt32 workingBufferPosition = 0u;

    PROCESS:
        FILE_DIRECTORY_INFORMATION* fileDirectoryInformation = (FILE_DIRECTORY_INFORMATION*)(internalEnumeratorContext->NtQueryDirectoryFileWorkingBuffer + workingBufferPosition);

        // skip if "." and ".." | little-endian UTF-16
        UInt64 skipCheck = (*(UInt64*)fileDirectoryInformation->FileName) << 16;
        if (skipCheck == 3014656ul || skipCheck == 197571510272ul)
        {
            workingBufferPosition += fileDirectoryInformation->NextEntryOffset;

            if (fileDirectoryInformation->NextEntryOffset != 0u) goto PROCESS;
            else goto QUERY;
        }

        //

        Boolean isReparsePoint = (fileDirectoryInformation->FileAttributes & Constants.FILE_ATTRIBUTE_REPARSE_POINT) == Constants.FILE_ATTRIBUTE_REPARSE_POINT;
        Boolean isDirectory = (fileDirectoryInformation->FileAttributes & Constants.FILE_ATTRIBUTE_DIRECTORY) == Constants.FILE_ATTRIBUTE_DIRECTORY;

        // directory
        if (isDirectory && !isReparsePoint)
        {
            UInt32 itemSize = (Directory.RAW_SIZE + fileDirectoryInformation->FileNameLength + 7u) & ~7u;

            Directory* directory = (Directory*)(directoryInfoBuffer + enumeratorResult->AddedSize);
            directory->NextItemOffset = itemSize;
            directory->Type = NodeType.Directory;
            directory->ParentDirectoryBaseOffset = parentNodeBaseOffset;
            directory->Attributes = fileDirectoryInformation->FileAttributes;
            directory->NameLengthBytes = (UInt16)fileDirectoryInformation->FileNameLength;
            Buffer.MemoryCopy(fileDirectoryInformation->FileName, directory->Name, fileDirectoryInformation->FileNameLength, fileDirectoryInformation->FileNameLength);

            enumeratorResult->AddedSize += itemSize;
            enumeratorResult->SawSubDirectory = true;

            goto PROCESS_TAIL;
        }

        /****************************************************************************************/

        UInt16 itemPathLengthBytes = (UInt16)(ntPathLengthBytes + fileDirectoryInformation->FileNameLength + 2);
        Char* itemPath = stackalloc Char[itemPathLengthBytes >>> 1];
        Buffer.MemoryCopy(ntPath, itemPath, itemPathLengthBytes, ntPathLengthBytes);
        itemPath[ntPathLengthBytes >>> 1] = '\\';
        Buffer.MemoryCopy(fileDirectoryInformation->FileName, itemPath + (ntPathLengthBytes >>> 1) + 1, fileDirectoryInformation->FileNameLength, fileDirectoryInformation->FileNameLength);

        if (!isDirectory && !isReparsePoint)
        {
            if (!ProcessFile(itemPath, itemPathLengthBytes, internalEnumeratorContext, externalEnumeratorContext, parentNodeBaseOffset, fileDirectoryInformation, &enumeratorResult->AddedSize))
            {
                _ = NtDll.NtClose(fileHandle);
                return false;
            }
        }
        else // handle link (junction / symbolic link)
        {
            if (!HandleReparsePoint(itemPath, itemPathLengthBytes, internalEnumeratorContext, externalEnumeratorContext))
            {
                _ = NtDll.NtClose(fileHandle);
                return false;
            }

            workingBufferPosition += fileDirectoryInformation->NextEntryOffset;
            if (fileDirectoryInformation->NextEntryOffset != 0u) goto PROCESS;
            else goto QUERY;
        }

    PROCESS_TAIL:
        ++enumeratorResult->NumberOfItems;
        workingBufferPosition += fileDirectoryInformation->NextEntryOffset;
        if (fileDirectoryInformation->NextEntryOffset != 0u) goto PROCESS;
        else goto QUERY;
    }
}