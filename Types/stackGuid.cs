using System;

namespace SyncMan
{
    internal ref struct xGuid
    {
        internal UInt32 A;
        internal UInt16 B;
        internal UInt16 C;
        internal Byte D;
        internal Byte E;
        internal Byte F;
        internal Byte G;
        internal Byte H;
        internal Byte I;
        internal Byte J;
        internal Byte K;

        internal static xGuid CreateGuid()
        {
            xGuid guid = new();
            Byte[] random;

            try
            {
                random = Util.RandomDotOrg_Get_16_Bytes();
            }
            catch
            {
                Guid tempGuid = Guid.NewGuid();
                random = tempGuid.ToByteArray();
            }
            
            guid.A = unchecked((UInt32)((random[3] << 24) | (random[2] << 16) | (random[1] << 8) | random[0]));
            guid.B = unchecked((UInt16)(short)((random[5] << 8) | random[4]));
            guid.C = unchecked((UInt16)((random[7] << 8) | random[6]));
            guid.D = random[8];
            guid.E = random[9];
            guid.F = random[10];
            guid.G = random[11];
            guid.H = random[12];
            guid.I = random[13];
            guid.J = random[14];
            guid.K = random[15];

            return guid;
        }
    }
}