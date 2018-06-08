namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRIGTConfig
    {
        public static string Module = "DarkSoulsRemastered";
        public const int TimerQuitoutDelay = 512;

        public static int IGTOffset = 0xA4;
        public static int ArrayOfByteOffset = 3;
        public static byte?[] ArrayOfBytes = new byte?[]
        {
            0x48, 0x8B, 0x05, null, null, null, null, 0x32, 0xDB, 0x48, 0x8B, 0x48, 0x10, 0x48, 0x81, 0xC1, 0x80, 0x02, 0x00, 0x00
        };

        public static string AOBNotFound = "Array of Bytes not found ({0})";
        public static string AOBMultiple = "Array of Bytes found {0} times instead of just 1 time. ({0})";

        /**
         * Keeping for references. Now replaced by an array of bytes scan
         */
        //public static Dictionary<int, long> IGTAddresses = new Dictionary<int, long>()
        //{
        //    { 75928576, 0x141D00F50 }, // 1.01
        //    { 76987904, 0x141C90F40 }, // 1.01.1
        //};
    }
}
