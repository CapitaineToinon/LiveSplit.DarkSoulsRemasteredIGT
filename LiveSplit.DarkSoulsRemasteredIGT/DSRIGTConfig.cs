namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRIGTConfig
    {
        public static string Module = "DarkSoulsRemastered";
        public const int TimerQuitoutDelay = 512;

        public static int IGTOffset = 0xA4;
        public static long GetIGTAddress(int ModuleSize)
        {
            switch(ModuleSize)
            {
                // 1.01
                default:
                    return 0x141D00F50;
                // 1.01.1
                case 76987904:
                    return 0x141C90F40;
            }
        }
    }
}
