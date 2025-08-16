using System;
using BSS.Interop;
using static FilesystemEnumerator;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

internal static partial class MainWindow
{
    internal unsafe static void Debug(Object? sender, EventArgs e)
    {

    }

    internal unsafe static void* AllocLargePageTest()
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
        UInt64 size = (8ul * 1024ul * 1024ul);

        ntStatus = NtDll.NtAllocateVirtualMemory(unchecked((UInt64)(-1L)), &pointer, 0, &size, Constants.MEM_RESERVE | Constants.MEM_COMMIT | Constants.MEM_LARGE_PAGES, Constants.PAGE_READWRITE);
        if (ntStatus != Constants.STATUS_SUCCESS)
        {
            Log.Debug("NtAllocateVirtualMemory failed to allocate memory: 0x" + ntStatus.ToString("X") + "\n", Log.Level.Error, "LargePage");
            return null;
        }

        return pointer;
    }

    internal unsafe static void TraversDirectoryBuffer(Byte* directoryBuffer, Int64 length)
    {
        Int64 offset = 0;
        Char* path = stackalloc Char[32_768];
        Directory** workingTree = stackalloc Directory*[16_384];

    NEXT:
        Node* node = (Node*)(directoryBuffer + offset);

        UInt16 pathLengthBytes = GetPath(path, node, workingTree);

        if (node->Type == NodeType.File)
        {
            File* fileNode = (File*)node;

            String parentPath = new(path, 0, pathLengthBytes >>> 1);
            String fileName = new(fileNode->Name, 0, fileNode->NameLengthBytes >>> 1);

            Log.Debug(parentPath + '\\' + fileName + "\n", Log.Level.Info, "FILE");

            offset += fileNode->NextItemOffset;
        }
        else if (node->Type == NodeType.Directory)
        {
            Directory* directoryNode = (Directory*)node;

            String parentPath = new(path, 0, pathLengthBytes >>> 1);
            String fileName = new(directoryNode->Name, 0, directoryNode->NameLengthBytes >>> 1);

            Log.Debug(parentPath + '\\' + fileName + "\n", Log.Level.Info, "DIRECTORY");

            offset += directoryNode->NextItemOffset;
        }

        if (length > offset) goto NEXT;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static UInt16 GetPath(Char* path, Node* node, Directory** workingTree)
    {
        if (node->ParentDirectory == null) return 0;

        UInt16 length = 0;
        UInt16 offset = 0;
        workingTree[0] = node->ParentDirectory;

    FIND_PARENT:
        Directory* directoryNode = workingTree[length]->ParentDirectory;
        ++length;
        workingTree[length] = directoryNode;
        if (directoryNode != null)
        {
            goto FIND_PARENT;
        }

    FILL_BUFFER:
        path[offset] = '\\';
        ++offset;
        --length;

        Buffer.MemoryCopy(workingTree[length]->Name, path + offset, 16_384, workingTree[length]->NameLengthBytes);
        offset += (UInt16)(workingTree[length]->NameLengthBytes >>> 1);
        if (length != 0) goto FILL_BUFFER;

        return (UInt16)(offset << 1);
    }
}