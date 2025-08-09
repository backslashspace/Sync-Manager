using BSS.Interop;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

internal static partial class Program
{
    [STAThread]
    private unsafe static Int32 Main(String[] args)
    {
        UInt32 canary = 0xDEADBEEFu;

        Log.Initialize();
        ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        DwmApi.Initialize();






        const String path = "\\??\\C:\\Users\\dev0\\Desktop\\walktest";

        Char* pathBuffer = stackalloc Char[32_767];
        fixed (Char* pathPtr = &path.GetPinnableReference())
        {
            Buffer.MemoryCopy(pathPtr, pathBuffer, path.Length + path.Length, path.Length + path.Length);
        }

        Stopwatch stopwatch = new();

        Byte* dirBuf = null;

        

        stopwatch.Start();
        Int64 byteCount = MainWindow.RetrieveMetadata(pathBuffer, (UInt16)path.Length, dirBuf, &canary);
        stopwatch.Stop();

        Kernel32.AllocConsole();
        Console.WriteLine("\n\n" + byteCount + " bytes in " + stopwatch.Elapsed.TotalMilliseconds + "ms");

        //Thread.Sleep(1000);









        //MainWindow.Run();
        Console.ReadLine();
        return 0;
    }
}