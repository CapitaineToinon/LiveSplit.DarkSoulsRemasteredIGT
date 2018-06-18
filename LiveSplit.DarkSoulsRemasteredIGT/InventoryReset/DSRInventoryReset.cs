using System;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal class DSRInventoryReset
    {
        private DSRProcess gameProcess;
        private IntPtr IRaddress;

        public DSRInventoryReset(DSRProcess gameProcess)
        {
            this.gameProcess = gameProcess;
            Hook();
        }

        public void ResetInventory()
        {
            if (gameProcess.IsHooked)
            {
                // Foreach equipement slot...
                foreach (IntPtr equipementSlot in DSRInventoryIndexConfig.GetInventoryAddresses(IRaddress))
                {
                    // Gets the address to the value of the index
                    DSRMemory.WUInt32(gameProcess.Process.Handle, equipementSlot, UInt32.MaxValue); // Sets to the default value (4,294,967,295)
                }
            } else
            {
                Hook();
            }
        }

        private void Hook()
        {
            if (gameProcess.Hook())
            {
                // Game hooked! Find the InventoryIndex address using AOB scanning
                IRaddress = gameProcess.Scan(DSRInventoryIndexConfig.ArrayOfBytes, DSRInventoryIndexConfig.ArrayOfByteOffset);
            }
            else
            {
                IRaddress = IntPtr.Zero;
            }
        }
    }
}
