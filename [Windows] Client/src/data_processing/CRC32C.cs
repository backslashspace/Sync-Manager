using System;
using System.Runtime.Intrinsics.X86;

internal static partial class Program
{
    /// <summary>
    /// Initial value must be <see href="0xFFFFFFFFul"/><br/>
    /// Finish CRC with:<br/>
    /// <see langword="crc ^= 0xFFFFFFFFul;"/><br/>
    /// <see langword="*crc32c = (UInt32) crc;"/>
    /// </summary>
    /// <remarks>
    /// Verified using <see href="https://md5calc.com/hash/crc32c"/>
    /// </remarks>
    /// <param name="buffer"></param>
    /// <param name="crc32c"></param>
    /// <param name="length"></param>
    private unsafe static void UpdateCRC32C(Byte* buffer, UInt64* crc32c, UInt64 length)
    {
        UInt64 counter = 0;
        UInt64 internalCrc32C = *crc32c;
        UInt64* movedBuffer;

    L256:
        if (counter + 256 <= length)
        {
            movedBuffer = (UInt64*)(buffer + counter);

            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *movedBuffer);
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 1));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 2));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 3));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 4));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 5));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 6));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 7));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 8));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 9));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 10));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 11));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 12));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 13));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 14));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 15));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 16));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 17));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 18));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 19));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 20));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 21));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 22));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 23));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 24));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 25));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 26));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 27));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 28));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 29));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 30));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 31));

            counter += 256;
            goto L256;
        }

    L128:
        if (counter + 128 <= length)
        {
            movedBuffer = (UInt64*)(buffer + counter);

            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *movedBuffer);
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 1));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 2));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 3));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 4));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 5));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 6));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 7));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 8));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 9));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 10));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 11));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 12));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 13));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 14));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 15));

            counter += 128;
            goto L128;
        }

    L64:
        if (counter + 64 <= length)
        {
            movedBuffer = (UInt64*)(buffer + counter);

            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *movedBuffer);
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 1));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 2));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 3));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 4));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 5));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 6));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 7));

            counter += 64;
            goto L64;
        }

    L32:
        if (counter + 32 <= length)
        {
            movedBuffer = (UInt64*)(buffer + counter);

            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *movedBuffer);
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 1));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 2));
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(movedBuffer + 3));

            counter += 32;
            goto L32;
        }

    L8:
        if (counter + 8 <= length)
        {
            internalCrc32C = Sse42.X64.Crc32(internalCrc32C, *(UInt64*)(buffer + counter));

            counter += 8;
            goto L8;
        }

    L1:
        if (counter < length)
        {
            internalCrc32C = Sse42.Crc32((UInt32)internalCrc32C, buffer[counter]);

            ++counter;
            goto L1;
        }

        *crc32c = internalCrc32C;
    }
}