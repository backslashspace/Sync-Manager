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
    internal unsafe static void Debug(String message, Level logLevel, String source)
    {
        fixed (Char* messagePtr = &message.GetPinnableReference(), sourcePtr = &source.GetPinnableReference())
        {
            Debug(messagePtr, (UInt16)message.Length, logLevel, sourcePtr, (UInt16)source.Length);
        }
    }

    [Conditional("DEBUG")]
    internal unsafe static void Debug(Char* message, UInt16 messageLength, Level logLevel, Char* source, UInt16 sourceLength)
    {
        if (_consoleHandle == 0) return;

        const Int32 DATE_LENGTH = 21;
        const Int32 PADDING_TARGET = 52 + 9; // 52 is effective padding, plus 9 for ANSI codes (5 color, 4 reset)
        const Int32 MISC_CHAR_AMOUNT = 7;

        UInt64 localNow = *(UInt64*)KUSER_SHARED_DATA.SystemTime - *(UInt64*)KUSER_SHARED_DATA.TimeZoneBias;

        /**************************************************************************************************/

        Int32 logLineLength;
        Int32 logWordLength = LogWordLengthMap[(Int32)logLevel];
        Int32 infoPortionLength = DATE_LENGTH + MISC_CHAR_AMOUNT + sourceLength + logWordLength;

        if (PADDING_TARGET > infoPortionLength) logLineLength = PADDING_TARGET + messageLength;
        else logLineLength = infoPortionLength + messageLength;

        Int32 bufferOffset;
        Char* logLineBuffer = stackalloc Char[logLineLength];

        /**************************************************************************************************/

        SetTime(logLineBuffer, &localNow);

        /**************************************************************************************************/

        logLineBuffer[21] = ' ';

        logLineBuffer[22] = '[';
        Unsafe.CopyBlock(logLineBuffer + 23, LogWords[(Int32)logLevel], (UInt32)(LogWordLengthMap[(Int32)logLevel] + LogWordLengthMap[(Int32)logLevel]));
        bufferOffset = 23 + LogWordLengthMap[(Int32)logLevel];
        logLineBuffer[bufferOffset] = ']';
        logLineBuffer[bufferOffset + 1] = '-';
        logLineBuffer[bufferOffset + 2] = '[';

        Unsafe.CopyBlock(logLineBuffer + bufferOffset + 3, source, (UInt32)(sourceLength + sourceLength));
        bufferOffset += 3 + sourceLength;
        logLineBuffer[bufferOffset] = ']';
        ++bufferOffset;

    PAD:
        logLineBuffer[bufferOffset] = ' ';
        if (++bufferOffset < PADDING_TARGET) goto PAD;

        Unsafe.CopyBlock(logLineBuffer + bufferOffset, message, (UInt32)(messageLength + messageLength));

        IO_STATUS_BLOCK ioStatusBlock;
        _ = NtDll.NtWriteFile(_consoleHandle, 0, null, null, &ioStatusBlock, (Byte*)logLineBuffer, (UInt32)(logLineLength + logLineLength), 0, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static void SetTime(Char* buffer, UInt64* localNow)
    {
        TIME_FIELDS timeFields;
        NtDll.RtlTimeToTimeFields(localNow, &timeFields);

        UInt16 length;
        Char* timeStringBuffer = stackalloc Char[5];

        buffer[0] = '[';
        buffer[5] = '.';
        buffer[8] = '.';
        buffer[11] = ' ';
        buffer[14] = ':';
        buffer[17] = ':';
        buffer[20] = ']';

        // very unsafe - buffer is expected to be 20 - will be written from back - only 5 needed to not have overflow since UIn16
        length = Tools.UInt64ToString(timeFields.Year, timeStringBuffer - 15);
        switch (length)
        {
            case 4:
                buffer[1] = timeStringBuffer[1];
                buffer[2] = timeStringBuffer[2];
                buffer[3] = timeStringBuffer[3];
                buffer[4] = timeStringBuffer[4];
                break;

            case 5:
                buffer[1] = 'W';
                buffer[2] = 'A';
                buffer[3] = 'R';
                buffer[4] = 'N';
                break;

            case 3:
                buffer[1] = '0';
                buffer[2] = timeStringBuffer[2];
                buffer[3] = timeStringBuffer[3];
                buffer[4] = timeStringBuffer[4];
                break;

            case 2:
                buffer[1] = '0';
                buffer[2] = '0';
                buffer[3] = timeStringBuffer[3];
                buffer[4] = timeStringBuffer[4];
                break;

            case 1:
                buffer[1] = '0';
                buffer[2] = '0';
                buffer[3] = '0';
                buffer[4] = timeStringBuffer[4];
                break;
        }

        length = Tools.UInt64ToString(timeFields.Month, timeStringBuffer - 15);
        if (length == 1)
        {
            buffer[6] = '0';
            buffer[7] = timeStringBuffer[4];
        }
        else
        {
            buffer[6] = timeStringBuffer[3];
            buffer[7] = timeStringBuffer[4];
        }

        length = Tools.UInt64ToString(timeFields.Day, timeStringBuffer - 15);
        if (length == 2)
        {
            buffer[9] = timeStringBuffer[3];
            buffer[10] = timeStringBuffer[4];
        }
        else
        {
            buffer[9] = '0';
            buffer[10] = timeStringBuffer[4];
        }

        length = Tools.UInt64ToString(timeFields.Hour, timeStringBuffer - 15);
        if (length == 2)
        {
            buffer[12] = timeStringBuffer[3];
            buffer[13] = timeStringBuffer[4];
        }
        else
        {
            buffer[12] = '0';
            buffer[13] = timeStringBuffer[4];
        }

        length = Tools.UInt64ToString(timeFields.Minute, timeStringBuffer - 15);
        if (length == 2)
        {
            buffer[15] = timeStringBuffer[3];
            buffer[16] = timeStringBuffer[4];
        }
        else
        {
            buffer[15] = '0';
            buffer[16] = timeStringBuffer[4];
        }

        length = Tools.UInt64ToString(timeFields.Second, timeStringBuffer - 15);
        if (length == 2)
        {
            buffer[19] = timeStringBuffer[3];
            buffer[20] = timeStringBuffer[4];
        }
        else
        {
            buffer[19] = '0';
            buffer[20] = timeStringBuffer[4];
        }
    }
}