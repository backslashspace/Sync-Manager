using System;
using BSS.Interop;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

internal static partial class Program
{
    [STAThread]
    private unsafe static Int32 Main(String[] args)
    {
        Log.Initialize();
        ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        DebugX();

        DwmApi.Initialize();
        MainWindow.Run();
        
        return 0;
    }

    
    private unsafe static void DebugX() // X = Extreme
    {
        //Log.Debug("struct: Node = " + sizeof(Node) + "\n", Log.Level.Verbose, "SanityChecks");
        //Log.Debug("struct: File = " + sizeof(File) + "\n", Log.Level.Verbose, "SanityChecks");
        //Log.Debug("struct: Directory = " + sizeof(Directory) + "\n", Log.Level.Verbose, "SanityChecks");
        //Log.Debug("struct: EnumeratorStackItem = " + sizeof(EnumeratorStackFrame) + "\n\n", Log.Level.Verbose, "SanityChecks");

        /****************************************************************************************/

        Stopwatch stopwatch = new();
        stopwatch.Start();
        Int64 directoryBufferLength = 0;
        Byte* directoryBuffer = MainWindow.RetrieveMetadata("\\??\\C:\\Users\\dev0\\Desktop\\walktest", &directoryBufferLength);
        stopwatch.Stop();

        Kernel32.AllocConsole();
        Console.WriteLine("\n\n" + directoryBufferLength + " bytes in " + stopwatch.Elapsed.TotalMilliseconds + "ms");

        Console.ReadLine();

        NativeMemory.Free(directoryBuffer);

        Environment.Exit(0);
    }
}