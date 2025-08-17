using System;
using BSS.Interop;
using System.Runtime.InteropServices;

internal static partial class FilesystemEnumerator
{
    // iterative DFS for directory to flat buffer - for realloc support change X->Node from ptr to offset in buffer etc.
    internal unsafe static Boolean RetrieveMetadata(String ntPath, MetaData* metaData)
    {
        #region PREPARE
        UInt16 pathLength = (UInt16)ntPath.Length;
        UInt16 pathLengthBytes = (UInt16)(pathLength << 1);
        Char* pathBuffer = (Char*)NativeMemory.Alloc(65_536);
        fixed (Char* ntPathPtr = &ntPath.GetPinnableReference()) { Buffer.MemoryCopy(ntPathPtr, pathBuffer, pathLengthBytes, pathLengthBytes); }

        EnumeratorContext enumeratorContext;
        enumeratorContext.DirectoryInfoBufferOffset = 0ul;
        enumeratorContext.NtFsControlFileWorkingBuffer = (Byte*)NativeMemory.Alloc(EnumeratorContext.NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE);
        enumeratorContext.NtQueryDirectoryFileWorkingBuffer = (Byte*)NativeMemory.Alloc(EnumeratorContext.NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE);

        NtStatus ntStatus = NtDll.NtCreateEvent(&enumeratorContext.SynchronizationEventHandle, Constants.EVENT_ALL_ACCESS, null, Constants.EVENT_TYPE.SynchronizationEvent, false);
        if (Constants.STATUS_SUCCESS != ntStatus)
        {
            Log.Debug("Failed to create NtEvent: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtCreateEvent");

            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(enumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            return false;
        }

        UInt32 enumeratorStackFrameIndex = 0u;
        EnumeratorStackFrame* enumeratorStack = (EnumeratorStackFrame*)NativeMemory.Alloc((UIntPtr)sizeof(EnumeratorStackFrame) * 16_384);
        enumeratorStack->Node = (Node*)metaData->DirectoryInfoBuffer;
        enumeratorStack->ItemIndex = 0u;

        Directory* parentNode = null;
        EnumeratorResult enumeratorResult = default;
        EnumeratorStackFrame* stackFrame = enumeratorStack;
    #endregion

    ENUMERATE_DIRECTORY:
        if (!EnumerateDirectory(pathBuffer, pathLengthBytes, parentNode, &enumeratorContext, metaData, &enumeratorResult))
        {
            Log.Debug("EnumerateDirectoryToBuffer path was: " + Marshal.PtrToStringUni((IntPtr)pathBuffer, pathLength) + "\n", Log.Level.Error, "EnumerateDirectoryToBuffer");

            _ = NtDll.NtClose(enumeratorContext.SynchronizationEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(enumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(enumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            return false;
        }

        enumeratorContext.DirectoryInfoBufferOffset += enumeratorResult.AddedSize;
        stackFrame->NumberOfItems = enumeratorResult.NumberOfItems;

        if (!enumeratorResult.SawDirectory)
        {
            --enumeratorStackFrameIndex;
            stackFrame = enumeratorStack + enumeratorStackFrameIndex;
            pathLengthBytes -= stackFrame->PathPopLengthBytes;
            pathLength = (UInt16)(pathLengthBytes >>> 1);

            if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto NO_MORE_NODES;
        }

    SEARCH_SUBDIRECTORY_NODE:
        if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto NO_MORE_NODES;

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
            parentNode = directory;

            // set node to next node - move via byte offset
            stackFrame->Node = (Node*)((Byte*)stackFrame->Node + stackFrame->Node->NextItemOffset);

            // point to next frame and set initial state
            ++enumeratorStackFrameIndex;
            stackFrame = enumeratorStack + enumeratorStackFrameIndex;

            stackFrame->ItemIndex = 0u;
            stackFrame->Node = (Node*)(metaData->DirectoryInfoBuffer + enumeratorContext.DirectoryInfoBufferOffset);

            if ((enumeratorContext.DirectoryInfoBufferOffset + (1ul * 1024ul * 1024ul)) > metaData->DirectoryInfoBufferSize)
            {
                Log.Debug("DirectoryInfoBufferSize to small, can not continue - Increase buffer size and try again: allocation size: " + metaData->DirectoryInfoBufferSize + ", buffer size: " + enumeratorContext.DirectoryInfoBufferOffset + ", abort if difference is less than 1MiB\n", Log.Level.Error, "Enumerator");

                _ = NtDll.NtClose(enumeratorContext.SynchronizationEventHandle);
                NativeMemory.Free(pathBuffer);
                NativeMemory.Free(enumeratorStack);
                NativeMemory.Free(enumeratorContext.NtFsControlFileWorkingBuffer);
                NativeMemory.Free(enumeratorContext.NtQueryDirectoryFileWorkingBuffer);

                *metaData->UsedLinkInfoBufferLength = enumeratorContext.LinkInfoBufferOffset;
                *metaData->UsedDirectoryInfoBufferLength = enumeratorContext.DirectoryInfoBufferOffset;

                return false;
            }

            goto ENUMERATE_DIRECTORY;
        }

        if (stackFrame->ItemIndex < stackFrame->NumberOfItems)
        {
            // set node to next node - move via byte offset
            stackFrame->Node = (Node*)((Byte*)stackFrame->Node + stackFrame->Node->NextItemOffset);
            ++stackFrame->ItemIndex;

            goto SEARCH_SUBDIRECTORY_NODE;
        }

    NO_MORE_NODES:
        if (enumeratorStackFrameIndex == 0u)
        {
            _ = NtDll.NtClose(enumeratorContext.SynchronizationEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(enumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(enumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            *metaData->UsedLinkInfoBufferLength = enumeratorContext.LinkInfoBufferOffset;
            *metaData->UsedDirectoryInfoBufferLength = enumeratorContext.DirectoryInfoBufferOffset;
            return true;
        }

        --enumeratorStackFrameIndex;
        stackFrame = enumeratorStack + enumeratorStackFrameIndex;
        pathLengthBytes -= stackFrame->PathPopLengthBytes;
        pathLength = (UInt16)(pathLengthBytes >>> 1);

        if (stackFrame->ItemIndex == stackFrame->NumberOfItems) goto NO_MORE_NODES;
        else goto SEARCH_SUBDIRECTORY_NODE;
    }
}