namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRIGTConfig
    {
        public static int TimerQuitoutDelay = 512;
        public static int IGTRefreshRate = 16;

        public static int IGTOffset = 0xA4;
        public static int ArrayOfByteOffset = 3;
        public static byte?[] ArrayOfBytes = new byte?[]
        {
            0x48, 0x8B, 0x05, null, null, null, null, 0x32, 0xDB, 0x48, 0x8B, 0x48, 0x10, 0x48, 0x81, 0xC1, 0x80, 0x02, 0x00, 0x00
        };
    }
}
