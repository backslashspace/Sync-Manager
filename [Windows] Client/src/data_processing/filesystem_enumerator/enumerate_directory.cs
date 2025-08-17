using System;
using BSS.Interop;
using System.Runtime.CompilerServices;

internal static partial class FilesystemEnumerator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static Boolean EnumerateDirectory(Char* ntPath, UInt16 ntPathLengthBytes, Directory* parentNode, EnumeratorContext* enumeratorContext, MetaData* metaData,  EnumeratorResult* enumeratorResult)
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
        enumeratorResult->SawDirectory = false;
        Byte* directoryInfoBuffer = metaData->DirectoryInfoBuffer + enumeratorContext->DirectoryInfoBufferOffset;

    /****************************************************************************************/

    QUERY:
        ntStatus = NtDll.NtQueryDirectoryFile(fileHandle, enumeratorContext->SynchronizationEventHandle, null, null, &ioStatusBlock, enumeratorContext->NtQueryDirectoryFileWorkingBuffer, EnumeratorContext.NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE, Constants.FILE_INFORMATION_CLASS.FileIdFullDirectoryInformation, false, null, false);
        if (ntStatus == Constants.STATUS_PENDING)
        {
            ntStatus = NtDll.NtWaitForSingleObject(enumeratorContext->SynchronizationEventHandle, false, 0ul);
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
        FILE_ID_FULL_DIR_INFORMATION* systemInfo = (FILE_ID_FULL_DIR_INFORMATION*)(enumeratorContext->NtQueryDirectoryFileWorkingBuffer + workingBufferPosition);

        // skip if "." and ".." | little-endian UTF-16
        UInt64 skipCheck = (*(UInt64*)systemInfo->FileName) << 16;
        if (skipCheck == 3014656ul || skipCheck == 197571510272ul)
        {
            workingBufferPosition += systemInfo->NextEntryOffset;

            if (systemInfo->NextEntryOffset != 0u) goto PROCESS;
            else goto QUERY;
        }

        //

        Boolean isReparsePoint = (systemInfo->FileAttributes & Constants.FILE_ATTRIBUTE_REPARSE_POINT) == Constants.FILE_ATTRIBUTE_REPARSE_POINT;
        Boolean isDirectory = (systemInfo->FileAttributes & Constants.FILE_ATTRIBUTE_DIRECTORY) == Constants.FILE_ATTRIBUTE_DIRECTORY;

        // directory
        if (isDirectory && !isReparsePoint)
        {
            Directory* directory = (Directory*)(directoryInfoBuffer + enumeratorResult->AddedSize);

            UInt32 itemSize = (Directory.RAW_SIZE + systemInfo->FileNameLength + 7u) & ~7u;

            directory->NextItemOffset = itemSize;
            directory->Type = NodeType.Directory;
            directory->ParentDirectory = parentNode;
            directory->Attributes = systemInfo->FileAttributes;
            directory->NameLengthBytes = (UInt16)systemInfo->FileNameLength;
            Buffer.MemoryCopy(systemInfo->FileName, directory->Name, systemInfo->FileNameLength, systemInfo->FileNameLength);

            enumeratorResult->AddedSize += itemSize;
            enumeratorResult->SawDirectory = true;

            goto PROCESS_TAIL;
        }

        /****************************************************************************************/

        UInt16 itemPathLengthBytes = (UInt16)(ntPathLengthBytes + systemInfo->FileNameLength + 2);
        Char* itemPath = stackalloc Char[itemPathLengthBytes >>> 1];
        Buffer.MemoryCopy(ntPath, itemPath, itemPathLengthBytes, ntPathLengthBytes);
        itemPath[ntPathLengthBytes >>> 1] = '\\';
        Buffer.MemoryCopy(systemInfo->FileName, itemPath + (ntPathLengthBytes >>> 1) + 1, systemInfo->FileNameLength, systemInfo->FileNameLength);

        // file
        if (!isDirectory && !isReparsePoint)
        {
            File* file = (File*)(directoryInfoBuffer + enumeratorResult->AddedSize);

            UInt32 itemSize = (File.RAW_SIZE + systemInfo->FileNameLength + 7u) & ~7u;

            file->NextItemOffset = itemSize;
            file->Type = NodeType.File;
            file->Size = systemInfo->EndOfFile;
            file->ParentDirectory = parentNode;
            file->Attributes = systemInfo->FileAttributes;
            file->NameLengthBytes = (UInt16)systemInfo->FileNameLength;
            Buffer.MemoryCopy(systemInfo->FileName, file->Name, systemInfo->FileNameLength, systemInfo->FileNameLength);

            enumeratorResult->AddedSize += itemSize;

            //

            //if (ProcessFile(itemPath, itemPathLengthBytes, enumeratorContext, metaData))
            //{

            //}
        }
        //else
        //{
        //    // handle link



        //    //HandleReparsePoint(itemPath, itemPathLengthBytes, enumeratorContext, metaData);
        //}

    PROCESS_TAIL:
        ++enumeratorResult->NumberOfItems;
        workingBufferPosition += systemInfo->NextEntryOffset;

        if (systemInfo->NextEntryOffset != 0u) goto PROCESS;
        else goto QUERY;
    }
}