using System;
using BSS.Interop;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal static partial class Log
{
    private static Handle _consoleHandle = 0;

    [Conditional("DEBUG")]
    private unsafe static void InitializeDebug()
    {
        if (Console.LargestWindowWidth == 0)
        {
            if (!Kernel32.AllocConsole()) throw new Exception("Log.InitializeDebug() -> Kernel32.AllocConsole() returned false");
        }

        InitializeLogWord();

        UInt32 mode = 0;
        _consoleHandle = Kernel32.GetStdHandle(Constants.STD_OUTPUT_HANDLE);
        Kernel32.GetConsoleMode(_consoleHandle, &mode);
        Kernel32.SetConsoleMode(_consoleHandle, mode | Constants.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    /**************************************************************************************************/

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe static void Debug(String message, Level logLevel, String source)
    {
        if (_consoleHandle == 0) return;

        String timeString = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

        fixed (Char* messagePtr = &message.GetPinnableReference(), sourcePtr = &source.GetPinnableReference(), timeStringPtr = &timeString.GetPinnableReference())
        {
            Int32 logLineLength;
            const Int32 DATE_LENGTH = 21;
            const Int32 MISC_CHAR_AMOUNT = 7;
            Int32 sourceLength = source.Length;
            Int32 messageLength = message.Length;
            Int32 logWordLength = LogWordLengthMap[(Int32)logLevel];
            Int32 infoPortionLength = DATE_LENGTH + MISC_CHAR_AMOUNT + sourceLength + logWordLength;
            const Int32 PADDING_TARGET = 52 + 9; // 52 is effective padding, plus 9 for ANSI codes (5 color, 4 reset)
            
            if (PADDING_TARGET > infoPortionLength) logLineLength = PADDING_TARGET + messageLength;
            else logLineLength = infoPortionLength + messageLength;

            Char* logLineBuffer = stackalloc Char[logLineLength];
            Int32 bufferOffset;

            logLineBuffer[0] = '[';
            Unsafe.CopyBlock(logLineBuffer + 1, timeStringPtr, 38);
            logLineBuffer[20] = ']';
            logLineBuffer[21] = ' ';

            logLineBuffer[22] = '[';
            Unsafe.CopyBlock(logLineBuffer + 23, LogWords[(Int32)logLevel], (UInt32)(LogWordLengthMap[(Int32)logLevel] + LogWordLengthMap[(Int32)logLevel]));
            bufferOffset = 23 + LogWordLengthMap[(Int32)logLevel];
            logLineBuffer[bufferOffset] = ']';
            logLineBuffer[bufferOffset + 1] = '-';
            logLineBuffer[bufferOffset + 2] = '[';

            Unsafe.CopyBlock(logLineBuffer + bufferOffset + 3, sourcePtr, (UInt32)(sourceLength + sourceLength));
            bufferOffset += 3 + sourceLength;
            logLineBuffer[bufferOffset] = ']';
            ++bufferOffset;

        PAD:
            logLineBuffer[bufferOffset] = ' ';
            if (++bufferOffset < PADDING_TARGET) goto PAD;

            Unsafe.CopyBlock(logLineBuffer + bufferOffset, messagePtr, (UInt32)(messageLength + messageLength));

            IO_STATUS_BLOCK ioStatusBlock;
            _ = NtDll.NtWriteFile(_consoleHandle, 0, null, null, &ioStatusBlock, (Byte*)logLineBuffer, (UInt32)(logLineLength + logLineLength), 0, null);
        }
    }
}