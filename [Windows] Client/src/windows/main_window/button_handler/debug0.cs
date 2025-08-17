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

    internal unsafe static void TraversDirectoryBuffer(Byte* directoryBuffer, UInt64 length)
    {
        UInt64 offset = 0;
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