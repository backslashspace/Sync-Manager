using System;
using BSS.Interop;
using System.Diagnostics;
using static FilesystemEnumerator;
using System.Runtime.InteropServices;

internal static partial class Program
{
    [STAThread]
    private unsafe static Int32 Main(String[] args)
    {
        Log.Initialize();
        //ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
        //Application.EnableVisualStyles();
        //Application.SetCompatibleTextRenderingDefault(false);

        return DebugX();

        DwmApi.Initialize();
        MainWindow.Run();
        
        return 0;
    }

    private unsafe static Int32 DebugX() // X = Extreme
    {
        Log.Debug("struct: Node = " + sizeof(Node) + "\n", Log.Level.Verbose, "SanityChecks");
        Log.Debug("struct: File = " + sizeof(File) + "\n", Log.Level.Verbose, "SanityChecks");
        Log.Debug("struct: Directory = " + sizeof(Directory) + "\n", Log.Level.Verbose, "SanityChecks");
        Log.Debug("struct: EnumeratorStackItem = " + sizeof(EnumeratorStackFrame) + "\n\n", Log.Level.Verbose, "SanityChecks");

        /****************************************************************************************/

        /****************************************************************************************/

        /****************************************************************************************/

        const UInt64 allocationSize = 8ul * 1024ul * 1024ul;
        const UInt64 linkAllocationSize = 1ul * 1024ul * 1024ul;

        UInt64 usedLinkInfoBufferLength = 0;
        UInt64 usedDirectoryInfoBufferLength = 0;

        MetaData metaData;
        metaData.DirectoryInfoBuffer = (Byte*)NativeMemory.Alloc((UIntPtr)allocationSize);
        metaData.DirectoryInfoBufferSize = allocationSize;
        metaData.UsedDirectoryInfoBufferLength = &usedDirectoryInfoBufferLength;
        metaData.LinkInfoBuffer = (Byte*)NativeMemory.Alloc((UIntPtr)linkAllocationSize);
        metaData.LinkInfoBufferSize = linkAllocationSize;
        metaData.UsedLinkInfoBufferLength = &usedLinkInfoBufferLength;

        Stopwatch stopwatch = Stopwatch.StartNew();
        Boolean success = RetrieveMetadata("\\??\\C:\\Users\\dev0\\Desktop\\walktest2_links", &metaData);
        stopwatch.Stop();

        Log.Debug(*metaData.UsedDirectoryInfoBufferLength + " bytes in " + stopwatch.Elapsed.TotalMilliseconds + "ms\n", Log.Level.Info, "DebugX->Directories");
        Log.Debug(*metaData.UsedLinkInfoBufferLength + " bytes in " + stopwatch.Elapsed.TotalMilliseconds + "ms\n\n", Log.Level.Info, "DebugX->Links");

        //Console.ReadLine();

        MainWindow.TraversDirectoryBuffer(metaData.DirectoryInfoBuffer, *metaData.UsedDirectoryInfoBufferLength);
        Console.WriteLine();
        MainWindow.TraversLinkBuffer(metaData.LinkInfoBuffer, *metaData.UsedLinkInfoBufferLength);













        Console.ReadLine();

        NativeMemory.Free(metaData.DirectoryInfoBuffer);
        NativeMemory.Free(metaData.LinkInfoBuffer);

        return 0;
    }
}