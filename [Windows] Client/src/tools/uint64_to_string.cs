using System;

internal static partial class Tools
{
    /// <summary>
    /// Written back to front - front bytes might be empty for small numbers
    /// </summary>
    /// <remarks>
    /// To get the actually start use buffer + 20 - length | 20 is internal max length <br/>Version of <see href="https://gist.githubusercontent.com/lpereira/c0bf3ca3148321395037/raw/97aac8794c4c79321f1a102fc2c82d2b05c32878/gistfile1.cpp">facebook_fixed_test</see>
    /// </remarks>
    /// <param name="value">Input</param>
    /// <param name="buffer">Buffer of length 20</param>
    /// <returns>Length in visual digits</returns>
    internal unsafe static UInt16 UInt64ToString(UInt64 value, Char* buffer)
    {
        const String DIGITS_LUT = "00010203040506070809101112131415161718192021222324252627282930313233343536373839404142434445464748495051525354555657585960616263646566676869707172737475767778798081828384858687888990919293949596979899";
        const UInt32 MAX_LENGTH = 20u;

        UInt32 position = MAX_LENGTH - 1u;
        Int32 index = 0;

        while (value >= 100ul)
        {
            index = (Int32)(value % 100uL << 1);
            value /= 100ul;
            buffer[position] = DIGITS_LUT[index + 1];
            buffer[position - 1u] = DIGITS_LUT[index];
            position -= 2u;
        }

        if (value < 10ul)
        {
            buffer[position] = (Char)(48 + value);
            return (UInt16)(20 - position);
        }

        index = (Int32)value << 1;
        buffer[position] = DIGITS_LUT[index + 1];
        buffer[position - 1u] = DIGITS_LUT[index];

        return (UInt16)(20u - position + 1u);
    }
}