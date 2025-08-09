using System;
using System.IO;
using System.Drawing;

internal static partial class Resources
{
    internal static readonly Boolean Initialized = false;

    internal static readonly Icon MainIcon;

    static Resources()
    {
        MemoryStream memoryStream = new(shell32_16801, false);
        MainIcon = new Icon(memoryStream);
        memoryStream.Close();

        Initialized = true;
    }
}