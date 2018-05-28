using System.Collections.Generic;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRIGTConfig
    {
        public static string Module = "DarkSoulsRemastered";
        public const int TimerQuitoutDelay = 512;

        public static int IGTOffset = 0xA4;

        public static Dictionary<int, long> IGTAddresses = new Dictionary<int, long>()
        {
            { 75928576, 0x141D00F50 }, // 1.01
            { 76987904, 0x141C90F40 }, // 1.01.1
        };
    }
}
