using System;
using BSS.Interop;
using System.Runtime.InteropServices;

internal static partial class FilesystemEnumerator
{
    // iterative DFS for directory to flat buffer - for realloc support change X->Node from ptr to offset in buffer etc.
    // uses pointer aliasing instead of unions (yeeh)
    internal unsafe static Boolean RetrieveMetadata(String ntPath, ExternalEnumeratorContext* externalEnumeratorContext)
    {
        #region PREPARE
        UInt16 pathLength = (UInt16)ntPath.Length;
        UInt16 pathLengthBytes = (UInt16)(pathLength << 1);
        Char* pathBuffer = (Char*)NativeMemory.Alloc(65_536);
        fixed (Char* ntPathPtr = &ntPath.GetPinnableReference()) { Buffer.MemoryCopy(ntPathPtr, pathBuffer, pathLengthBytes, pathLengthBytes); }

        InternalEnumeratorContext internalEnumeratorContext;
        internalEnumeratorContext.DirectoryInfoBufferOffset = 0ul;
        internalEnumeratorContext.NtFsControlFileWorkingBuffer = (Byte*)NativeMemory.Alloc(InternalEnumeratorContext.NT_FS_CONTROL_FILE_WORKING_BUFFER_SIZE);
        internalEnumeratorContext.NtQueryDirectoryFileWorkingBuffer = (Byte*)NativeMemory.Alloc(InternalEnumeratorContext.NT_QUERY_DIRECTORY_FILE_WORKING_BUFFER_SIZE);

        NtStatus ntStatus = NtDll.NtCreateEvent(&internalEnumeratorContext.SynchronizationEventHandle, Constants.EVENT_ALL_ACCESS, null, Constants.EVENT_TYPE.SynchronizationEvent, false);
        if (Constants.STATUS_SUCCESS != ntStatus)
        {
            Log.Debug("Failed to create NtEvent: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Debug, "NtCreateEvent");

            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(internalEnumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(internalEnumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            return false;
        }

        UInt32 enumeratorStackFrameIndex = 0u;
        EnumeratorStackFrame* enumeratorStack = (EnumeratorStackFrame*)NativeMemory.Alloc((UIntPtr)sizeof(EnumeratorStackFrame) * 16_384);
        enumeratorStack->Node = (Node*)externalEnumeratorContext->DirectoryInfoBuffer;
        enumeratorStack->ItemIndex = 0u;

        EnumeratorResult enumeratorResult = default;
        EnumeratorStackFrame* stackFrame = enumeratorStack;
        UInt64 parentNodeBaseOffset = Constants.INVALID_HANDLE_VALUE;
        #endregion

    ENUMERATE_DIRECTORY:
        if (!EnumerateDirectory(pathBuffer, pathLengthBytes, parentNodeBaseOffset, &internalEnumeratorContext, externalEnumeratorContext, &enumeratorResult))
        {
            Log.Debug("EnumerateDirectoryToBuffer path was: " + Marshal.PtrToStringUni((IntPtr)pathBuffer, pathLength) + "\n", Log.Level.Error, "EnumerateDirectoryToBuffer");

            _ = NtDll.NtClose(internalEnumeratorContext.SynchronizationEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(internalEnumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(internalEnumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            *externalEnumeratorContext->UsedLinkInfoBufferLength = internalEnumeratorContext.LinkInfoBufferOffset;
            *externalEnumeratorContext->UsedDirectoryInfoBufferLength = internalEnumeratorContext.DirectoryInfoBufferOffset;

            return false;
        }

        internalEnumeratorContext.DirectoryInfoBufferOffset += enumeratorResult.AddedSize;
        stackFrame->NumberOfItems = enumeratorResult.NumberOfItems;

        if (!enumeratorResult.SawSubDirectory)
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
            parentNodeBaseOffset = (UInt64)directory - (UInt64)externalEnumeratorContext->DirectoryInfoBuffer;

            // set node to next node - move via byte offset
            stackFrame->Node = (Node*)((Byte*)stackFrame->Node + stackFrame->Node->NextItemOffset);

            // point to next frame and set initial state
            ++enumeratorStackFrameIndex;
            stackFrame = enumeratorStack + enumeratorStackFrameIndex;

            stackFrame->ItemIndex = 0u;
            stackFrame->Node = (Node*)(externalEnumeratorContext->DirectoryInfoBuffer + internalEnumeratorContext.DirectoryInfoBufferOffset);

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
            _ = NtDll.NtClose(internalEnumeratorContext.SynchronizationEventHandle);
            NativeMemory.Free(pathBuffer);
            NativeMemory.Free(enumeratorStack);
            NativeMemory.Free(internalEnumeratorContext.NtFsControlFileWorkingBuffer);
            NativeMemory.Free(internalEnumeratorContext.NtQueryDirectoryFileWorkingBuffer);

            *externalEnumeratorContext->UsedLinkInfoBufferLength = internalEnumeratorContext.LinkInfoBufferOffset;
            *externalEnumeratorContext->UsedDirectoryInfoBufferLength = internalEnumeratorContext.DirectoryInfoBufferOffset;
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