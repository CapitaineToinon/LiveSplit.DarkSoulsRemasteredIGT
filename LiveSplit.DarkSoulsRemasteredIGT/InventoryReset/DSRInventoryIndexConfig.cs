using System;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRInventoryIndexConfig
    {
        public static int ArrayOfByteOffset = 14;
        public static byte?[] ArrayOfBytes = new byte?[]
        {
            0x4C, 0x8B, 0x65, 0xEF, 0x4C, 0x8B, 0x6D, 0x0F, 0x49, 0x63, 0xC6, 0x48, 0x8D, 0x0D, null, null, null, null, 0x8B, 0x14, 0x81, 0x83, 0xFA, 0xFF
        };

        public static IntPtr[] GetInventoryAddresses(IntPtr baseAddress)
        {
            return new IntPtr[]
            {
                baseAddress,            // slot 7
                baseAddress+0x4,        // slot 0
                baseAddress+0x8,        // slot 8
                baseAddress+0xC,        // slot 1
                baseAddress+0x3C,       // slot 2
                baseAddress+0x3C+0x4,   // slot 3
                baseAddress+0x3C+0x8,   // slot 4
                baseAddress+0x3C+0xC,   // slot 5
                baseAddress+0x3C+0x10,  // slot 6
                baseAddress+0x20,       // slot 14
                baseAddress+0x20+0x4,   // slot 15
                baseAddress+0x20+0x8,   // slot 16
                baseAddress+0x20+0xC,   // slot 17
                baseAddress+0x34,       // slot 18
                baseAddress+0x34+0x4,   // slot 19
                baseAddress+0x10,       // slot 9
                baseAddress+0x10+0x8,   // slot 10
                baseAddress+0x14,       // slot 11
                baseAddress+0x14+0x8,   // slot 12
            };
        }
    }
}
