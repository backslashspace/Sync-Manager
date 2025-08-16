namespace BSS.Interop
{
    internal static class KUSER_SHARED_DATA
    {
        // https://www.geoffchappell.com/studies/windows/km/ntoskrnl/inc/api/ntexapi_x/kuser_shared_data/index.htm
        internal const UInt64 LOCATION = 0x7FFE0000;

        internal const UInt64 SYSTEMTIME_OFFSET = 0x14ul;
        internal unsafe static readonly KSYSTEM_TIME* SystemTime = (KSYSTEM_TIME*)(LOCATION + SYSTEMTIME_OFFSET);

        internal const UInt64 TIME_ZONE_BIAS_OFFSET = 0x20ul;
        internal unsafe static readonly KSYSTEM_TIME* TimeZoneBias = (KSYSTEM_TIME*)(LOCATION + TIME_ZONE_BIAS_OFFSET);

        internal unsafe static readonly UInt32* NtMajorVersion = (UInt32*)(LOCATION + 0x026Cul);
        internal unsafe static readonly UInt32* NtMinorVersion = (UInt32*)(LOCATION + 0x0270ul);
        internal unsafe static readonly UInt32* NtBuildNumber = (UInt32*)(LOCATION + 0x0260ul);
    }
}