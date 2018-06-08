using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRInventoryReset
    {
        private static Dictionary<int, UInt32> InventoryAddresses = new Dictionary<int, UInt32>()
        {
            { DSRIGTConfig.ModuleSizes["1.01"], 0x1AA8F00 },
            { DSRIGTConfig.ModuleSizes["1.01.1"], 0x1A38F10 },
        };

        private static UInt32[] GetEquipementSlots(UInt32 baseAddress)
        {
            return new UInt32[]
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
                baseAddress+0x10+0x8,   // slot 10
                baseAddress+0x10+0x8,   // slot 10
                baseAddress+0x14,       // slot 11
                baseAddress+0x14+0x8,   // slot 12
            };
        }

        public static void Reset(Process darksouls)
        {
            if (darksouls != null && !darksouls.HasExited)
            {
                UInt32 baseAddress = InventoryAddresses[darksouls.MainModule.ModuleMemorySize];
                foreach (UInt32 equipementSlot in GetEquipementSlots(baseAddress))
                {
                    IntPtr p = IntPtr.Add(darksouls.MainModule.BaseAddress, (int)equipementSlot);
                    DSRIGTMemory.WUInt32(darksouls.Handle, p, 0); // Reset the index
                }
            }
        }
    }
}
