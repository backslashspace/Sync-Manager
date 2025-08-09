using System;
using BSS.Interop;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

internal static partial class MainWindow
{
    internal unsafe static void Debug(Object? sender, EventArgs e)
    {

    }

    private unsafe static void* AllocLargePageTest()
    {
        Handle processToken;
        TOKEN_PRIVILEGES tokenPrivileges;
        tokenPrivileges.PrivilegeCount = 1;
        tokenPrivileges.Privileges.Attributes = Constants.SE_PRIVILEGE_ENABLED;

        NtStatus ntStatus = NtDll.NtOpenProcessToken(unchecked((UInt64)(-1L)), Constants.TOKEN_ADJUST_PRIVILEGES | Constants.TOKEN_QUERY, &processToken);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("NtOpenProcessToken failed: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Error, "LargePage");
            return null;
        }

        fixed (Char* ptr = &"SeLockMemoryPrivilege".GetPinnableReference())
        {
            if (!Advapi32.LookupPrivilegeValueW(null, ptr, &tokenPrivileges.Privileges.Luid))
            {
                Log.Debug("LookupPrivilegeValueW failed: " + Marshal.GetLastPInvokeErrorMessage() + "\n", Log.Level.Error, "LargePage");
                return null;
            }
        }

        ntStatus = NtDll.NtAdjustPrivilegesToken(processToken, false, &tokenPrivileges, (UInt32)sizeof(TOKEN_PRIVILEGES), null, null);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("NtAdjustPrivilegesToken failed: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Error, "LargePage");
            return null;
        }



        void* pointer = null;
        UInt64 size = 8ul * 1024ul * 1024ul;

        ntStatus = NtDll.NtAllocateVirtualMemory(unchecked((UInt64)(-1L)),&pointer, 0, &size, Constants.MEM_RESERVE | Constants.MEM_COMMIT | Constants.MEM_LARGE_PAGES, Constants.PAGE_READWRITE);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("NtAllocateVirtualMemory failed to allocate memory: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Error, "LargePage");
            return null;
        }

        return pointer;

    }

    internal unsafe static Byte* RetrieveMetadata(String ntPath, Int64* bufferLength)
    {
        #region PREPARE
        UInt16 pathLength = (UInt16)ntPath.Length;
        UInt16 pathLengthBytes = (UInt16)(pathLength << 1);
        Char* pathBuffer = (Char*)NativeMemory.Alloc(65_536);
        fixed (Char* ntPathPtr = &ntPath.GetPinnableReference())
        {
            Buffer.MemoryCopy(ntPathPtr, pathBuffer, pathLengthBytes, pathLengthBytes);
        }

        Handle syncEventHandle;
        NtStatus ntStatus = NtDll.NtCreateEvent(&syncEventHandle, Constants.EVENT_ALL_ACCESS, null, Constants.EVENT_TYPE.SynchronizationEvent, false);
        if (Constants.STATUS_SUCCESS != ntStatus)
        {
            Log.Debug("Failed create NtEvent: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtOpenFile");

            NativeMemory.Free(pathBuffer);

            return null;
        }

        // todo: large pages?
        Int64 directoryInfoBufferOffset = 0;
        UInt32 enumeratorStackFrameIndex = 0;
        Byte* enumeratorWorkingBuffer = (Byte*)NativeMemory.Alloc(131_072);
        EnumeratorStackFrame* enumeratorStack = (EnumeratorStackFrame*)NativeMemory.Alloc((UIntPtr)sizeof(EnumeratorStackFrame) * 16_384);
        Byte* directoryInfoBuffer = (Byte*)NativeMemory.Alloc(8 * 1024 * 1024); // alloc last -> will be used in realloc
        enumeratorStack->Node = (Node*)directoryInfoBuffer;
        enumeratorStack->ItemIndex = 0;

        EnumeratorResult enumeratorResult = default;
        EnumeratorStackFrame* stackFrame = enumeratorStack;
        #endregion

    ENUMERATE_DIRECTORY:
        if (!EnumerateDirectory(pathBuffer, pathLengthBytes, directoryInfoBuffer + directoryInfoBufferOffset, syncEventHandle, enumeratorWorkingBuffer, &enumeratorResult))
        {
            Log.Debug("EnumerateDirectoryToBuffer path was: " + Marshal.PtrToStringUni((IntPtr)pathBuffer, pathLength) + "\n", Log.Level.Error, "EnumerateDirectoryToBuffer");
            
            _ = NtDll.NtClose(syncEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(enumeratorWorkingBuffer);

            return null;
        }

        directoryInfoBufferOffset += enumeratorResult.AddedSize;
        stackFrame->NumberOfItems = enumeratorResult.NumberOfItems;

        if (!enumeratorResult.SawDirectory)
        {
            --enumeratorStackFrameIndex;
            stackFrame = enumeratorStack + enumeratorStackFrameIndex;
            pathLengthBytes -= stackFrame->PathPopLengthBytes;
            pathLength = (UInt16)(pathLengthBytes >>> 1);

            if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto END_OF_FRAME_DATA;
        }

    // pass when come from sub directory
    SEARCH_SUBDIRECTORY_NODE:

        if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto END_OF_FRAME_DATA;

        if (stackFrame->Node->Type == NodeType.Directory)
        {
            ++stackFrame->ItemIndex;
            Directory* directory = (Directory*)stackFrame->Node;

            // prepare path for sub directory enumeration
            pathBuffer[pathLength] = '\\';
            pathLength += 1;
            pathLengthBytes += 2;
            Buffer.MemoryCopy(directory->Name, pathBuffer + pathLength, directory->NameLengthBytes, directory->NameLengthBytes);
            pathLengthBytes += directory->NameLengthBytes;
            pathLength = (UInt16)(pathLengthBytes >>> 1);
            stackFrame->PathPopLengthBytes = (UInt16)(directory->NameLengthBytes + 2);

            // set node to next node - move via byte offset
            stackFrame->Node = (Node*)((Byte*)stackFrame->Node + stackFrame->Node->NextItemOffset);

            // point to next frame and set initial state
            ++enumeratorStackFrameIndex;
            stackFrame = enumeratorStack + enumeratorStackFrameIndex;

            stackFrame->ItemIndex = 0;
            stackFrame->Node = (Node*)(directoryInfoBuffer + directoryInfoBufferOffset);

            goto ENUMERATE_DIRECTORY;
        }

        if (stackFrame->ItemIndex < stackFrame->NumberOfItems)
        {
            // set node to next node - move via byte offset
            stackFrame->Node = (Node*)((Byte*)stackFrame->Node + stackFrame->Node->NextItemOffset);
            ++stackFrame->ItemIndex;

            goto SEARCH_SUBDIRECTORY_NODE;
        }

    END_OF_FRAME_DATA:
        if (enumeratorStackFrameIndex == 0)
        {
            Log.Debug("Done - " + directoryInfoBufferOffset + " bytes in buffer\n", Log.Level.Info, "Enumerator");

            _ = NtDll.NtClose(syncEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(enumeratorWorkingBuffer);

            *bufferLength = directoryInfoBufferOffset;
            return directoryInfoBuffer;
        }

        --enumeratorStackFrameIndex;
        stackFrame = enumeratorStack + enumeratorStackFrameIndex;
        pathLengthBytes -= stackFrame->PathPopLengthBytes;
        pathLength = (UInt16)(pathLengthBytes >>> 1);

        if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto END_OF_FRAME_DATA;
        else goto SEARCH_SUBDIRECTORY_NODE;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static Boolean EnumerateDirectory(Char* path, UInt16 pathLengthBytes, Byte* directoryInfoBuffer, Handle eveHandle, Byte* workingBuffer, EnumeratorResult* enumeratorResult)
    {
        #region OPEN_DIRECTORY
        Handle fileHandle;
        NtStatus ntStatus;
        IO_STATUS_BLOCK ioStatusBlock;

        UNICODE_STRING unicodeString;
        unicodeString.Length = pathLengthBytes;
        unicodeString.MaximumLength = pathLengthBytes;
        unicodeString.Buffer = path;

        OBJECT_ATTRIBUTES objectAttributes;
        objectAttributes.Length = (UInt32)sizeof(OBJECT_ATTRIBUTES);
        objectAttributes.RootDirectory = 0;
        objectAttributes.ObjectName = &unicodeString;
        objectAttributes.Attributes = Constants.OBJ_CASE_INSENSITIVE; // | OBJ_OPENLINK;
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

        /****************************************************************************************/

    QUERY:
        ntStatus = NtDll.NtQueryDirectoryFile(fileHandle, eveHandle, null, null, &ioStatusBlock, workingBuffer, 131_072, Constants.FILE_INFORMATION_CLASS.FileIdFullDirectoryInformation, false, null, false);
        if (ntStatus == Constants.STATUS_PENDING)
        {
            ntStatus = NtDll.NtWaitForSingleObject(eveHandle, false, 0);
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
            else if (ntStatus == Constants.STATUS_BUFFER_OVERFLOW)
            {
                Log.Debug("NtQueryDF STATUS_BUFFER_OVERFLOW\n", Log.Level.Critical, "NtQueryDirectoryFile");
                _ = NtDll.NtClose(fileHandle);
                return false;
            }
            else
            {
                Log.Debug("NtQueryDirectoryFile failed: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtQueryDirectoryFile");
                _ = NtDll.NtClose(fileHandle);
                return false;
            }
        }
        
        UInt32 workingBufferPosition = 0;

    PROCESS:
        FILE_ID_FULL_DIR_INFORMATION* systemInfo = (FILE_ID_FULL_DIR_INFORMATION*)(workingBuffer + workingBufferPosition);

        // skip if "." and ".." | little-endian UTF-16
        UInt64 skipCheck = (*(UInt64*)systemInfo->FileName) << 16;
        if (skipCheck == 3014656 || skipCheck == 197571510272)
        {
            workingBufferPosition += systemInfo->NextEntryOffset;

            if (systemInfo->NextEntryOffset != 0) goto PROCESS;
            else goto QUERY;
        }

        if ((systemInfo->FileAttributes & Constants.FILE_ATTRIBUTE_DIRECTORY) == Constants.FILE_ATTRIBUTE_DIRECTORY)
        {
            Directory* directory = (Directory*)(directoryInfoBuffer + enumeratorResult->AddedSize);

            UInt32 itemSize = (Directory.RAW_SIZE + systemInfo->FileNameLength + 7u) & ~7u;

            directory->NextItemOffset = itemSize;
            directory->Type = NodeType.Directory;
            directory->Attributes = systemInfo->FileAttributes;
            directory->NameLengthBytes = (UInt16)systemInfo->FileNameLength;
            Buffer.MemoryCopy(systemInfo->FileName, directory->Name, systemInfo->FileNameLength, systemInfo->FileNameLength);

            enumeratorResult->AddedSize += itemSize;
            enumeratorResult->SawDirectory = true;
        }
        else
        {            
            File* file = (File*)(directoryInfoBuffer + enumeratorResult->AddedSize);

            UInt32 itemSize = (File.RAW_SIZE + systemInfo->FileNameLength + 7u) & ~7u;

            file->NextItemOffset = itemSize;
            file->Type = NodeType.File;
            file->Size = systemInfo->EndOfFile;
            file->Attributes = systemInfo->FileAttributes;
            file->NameLengthBytes = (UInt16)systemInfo->FileNameLength;
            Buffer.MemoryCopy(systemInfo->FileName, file->Name, systemInfo->FileNameLength, systemInfo->FileNameLength);

            enumeratorResult->AddedSize += itemSize;
        }

        ++enumeratorResult->NumberOfItems;
        workingBufferPosition += systemInfo->NextEntryOffset;

        if (systemInfo->NextEntryOffset != 0) goto PROCESS;
        else goto QUERY;
    }

    /****************************************************************************************/

    [StructLayout(LayoutKind.Sequential)]
    private ref struct EnumeratorResult
    {
        internal Int64 AddedSize;
        internal UInt32 NumberOfItems;
        internal Boolean SawDirectory;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe ref struct EnumeratorStackFrame
    {
        internal Node* Node;
        internal UInt32 ItemIndex;
        internal UInt32 NumberOfItems;
        internal UInt16 PathPopLengthBytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe ref struct Node
    {
        internal UInt64 NextItemOffset;
        internal NodeType Type;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe ref struct File
    {
        internal const UInt32 RAW_SIZE = 28u; // used to pad (align) | all members but Name

        internal Int64 NextItemOffset;
        internal NodeType Type;
        internal UInt16 NameLengthBytes;
        internal UInt32 Attributes;
        internal UInt64 Size;
        internal UInt32 CRC32C;
        internal fixed Char Name[1];
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe ref struct Directory
    {
        internal const UInt32 RAW_SIZE = 16u; // used to pad (align) | all members but Name

        internal Int64 NextItemOffset;
        internal NodeType Type;
        internal UInt16 NameLengthBytes;
        internal UInt32 Attributes;
        internal fixed Char Name[1];
    }

    private enum NodeType : UInt16
    {
        File = 0,
        Directory = 1,
    }
}