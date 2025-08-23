using System;
using BSS.Interop;
using static FilesystemEnumerator;
using System.Runtime.CompilerServices;

internal static partial class MainWindow
{
    internal unsafe static void Debug(Object? sender, EventArgs e)
    {

    }

    internal unsafe static void TraversLinkBuffer(Byte* linkBuffer, UInt64 length)
    {
        UInt64 offset = 0;
        Char* path = stackalloc Char[32_768];

    NEXT:
        Link* link = (Link*)(linkBuffer + offset);

        if (link->Type == NodeType.SymbolicLink)
        {
            String linkPath = new(link->Paths, 0, link->LinkNtPathLengthBytes >>> 1);
            String targetPath = new(link->Paths, link->LinkNtPathLengthBytes >>> 1, link->TargetNtPathLengthBytes >>> 1);

            Log.Debug(linkPath + " -> " + targetPath + "\n", Log.Level.Info, "SymLink");

            offset += link->NextItemOffset;
        }
        else if (link->Type == NodeType.Junction)
        {
            String linkPath = new(link->Paths, 0, link->LinkNtPathLengthBytes >>> 1);
            String targetPath = new(link->Paths, link->LinkNtPathLengthBytes >>> 1, link->TargetNtPathLengthBytes >>> 1);

            Log.Debug(linkPath + " -> " + targetPath + "\n", Log.Level.Info, "Junction");

            offset += link->NextItemOffset;
        }

        if (length > offset) goto NEXT;
    }

    internal unsafe static void TraversDirectoryBuffer(Byte* directoryBuffer, UInt64 length)
    {
        UInt64 offset = 0;
        Char* path = stackalloc Char[32_768];
        Directory** directoryStack = stackalloc Directory*[16_384];

    NEXT:
        Node* node = (Node*)(directoryBuffer + offset);

        UInt16 pathLengthBytes;

        pathLengthBytes = GetPath(path, node->ParentDirectoryBaseOffset, directoryBuffer, directoryStack);

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
    private unsafe static UInt16 GetPath(Char* path, UInt64 parentDirectoryBaseOffset, Byte* directoryBuffer, Directory** directoryStack)
    {
        if (parentDirectoryBaseOffset == Constants.INVALID_HANDLE_VALUE) return 0;

        UInt16 offset = 0;
        UInt16 position = 0;

        directoryStack[0] = (Directory*)(directoryBuffer + parentDirectoryBaseOffset);
        if (directoryStack[0]->ParentDirectoryBaseOffset == Constants.INVALID_HANDLE_VALUE) goto FILL_BUFFER;

    ADD_PARENT:
        Directory* directoryNode = (Directory*)(directoryBuffer + directoryStack[position]->ParentDirectoryBaseOffset);

        ++position;
        directoryStack[position] = directoryNode;
        if (directoryNode->ParentDirectoryBaseOffset != Constants.INVALID_HANDLE_VALUE) goto ADD_PARENT;

    FILL_BUFFER:
        path[offset++] = '\\';

        Buffer.MemoryCopy(directoryStack[position]->Name, path + offset, 16_384, directoryStack[position]->NameLengthBytes);
        offset += (UInt16)(directoryStack[position]->NameLengthBytes >>> 1);
        
        if (position != 0)
        {
            --position;
            goto FILL_BUFFER;
        }

        return (UInt16)(offset << 1);
    }
}