using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal static partial class Log
{
    internal enum Level : Int32
    {
        Debug = 0,      // green
        Verbose = 1,    // magenta
        Info = 2,       // cyan
        Warning = 3,    // yellow
        Alert = 4,      // yellow
        Error = 5,      // red
        Critical = 6,   // red
    };

    /**************************************************************************************************/

    private unsafe static Char** LogWords = null!;
    private unsafe static Int32* LogWordLengthMap = null!;

    [Conditional("DEBUG")]
    private unsafe static void InitializeLogWord()
    {
        const String DEBUG = "\x1B[92mDebug\x1B[0m";
        const String VERBOSE = "\x1B[95mVerbose\x1B[0m";
        const String INFO = "\x1B[96mInfo\x1B[0m";
        const String WARNING = "\x1B[33mWarning\x1B[0m";
        const String ALERT = "\x1B[93mAlert\x1B[0m";
        const String ERROR = "\x1B[91mError\x1B[0m";
        const String CRITICAL = "\x1B[31mCritical\x1B[0m";

        LogWordLengthMap = (Int32*)NativeMemory.Alloc((UIntPtr)(7 * sizeof(Int32*)));
        LogWordLengthMap[0] = DEBUG.Length;
        LogWordLengthMap[1] = VERBOSE.Length;
        LogWordLengthMap[2] = INFO.Length;
        LogWordLengthMap[3] = WARNING.Length;
        LogWordLengthMap[4] = ALERT.Length;
        LogWordLengthMap[5] = ERROR.Length;
        LogWordLengthMap[6] = CRITICAL.Length;

        LogWords = (Char**)NativeMemory.Alloc((UIntPtr)(7 * sizeof(Char*)));
        LogWords[(Int32)Level.Debug] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Debug] * sizeof(Char)));
        LogWords[(Int32)Level.Verbose] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Verbose] * sizeof(Char)));
        LogWords[(Int32)Level.Info] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Info] * sizeof(Char)));
        LogWords[(Int32)Level.Warning] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Warning] * sizeof(Char)));
        LogWords[(Int32)Level.Alert] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Alert] * sizeof(Char)));
        LogWords[(Int32)Level.Error] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Error] * sizeof(Char)));
        LogWords[(Int32)Level.Critical] = (Char*)NativeMemory.Alloc((UIntPtr)(LogWordLengthMap[(Int32)Level.Critical] * sizeof(Char)));

        fixed (Char* debugPtr = &DEBUG.GetPinnableReference(),
            verbosePtr = &VERBOSE.GetPinnableReference(),
            infoPtr = &INFO.GetPinnableReference(),
            warningPtr = &WARNING.GetPinnableReference(),
            alertPtr = &ALERT.GetPinnableReference(),
            errorPtr = &ERROR.GetPinnableReference(),
            criticalPtr = &CRITICAL.GetPinnableReference())
        {
            Unsafe.CopyBlock(LogWords[(Int32)Level.Debug], debugPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Debug] + LogWordLengthMap[(Int32)Level.Debug]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Verbose], verbosePtr, (UInt32)(LogWordLengthMap[(Int32)Level.Verbose] + LogWordLengthMap[(Int32)Level.Verbose]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Info], infoPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Info] + LogWordLengthMap[(Int32)Level.Info]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Warning], warningPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Warning] + LogWordLengthMap[(Int32)Level.Warning]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Alert], alertPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Alert] + LogWordLengthMap[(Int32)Level.Alert]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Error], errorPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Error] + LogWordLengthMap[(Int32)Level.Error]));
            Unsafe.CopyBlock(LogWords[(Int32)Level.Critical], criticalPtr, (UInt32)(LogWordLengthMap[(Int32)Level.Critical] + LogWordLengthMap[(Int32)Level.Critical]));
        }
    }
}