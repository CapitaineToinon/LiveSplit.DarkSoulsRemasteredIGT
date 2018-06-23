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
            this.gameProcess.ProcessHooked += GameProcess_ProcessHooked;
            this.gameProcess.ProcessHasExited += GameProcess_ProcessHasExited;
        }

        private void GameProcess_ProcessHooked(object sender, EventArgs e)
        {
            // Game hooked! Find the InventoryIndex address using AOB scanning
            IRaddress = gameProcess.Scan(DSRInventoryIndexConfig.ArrayOfBytes, DSRInventoryIndexConfig.ArrayOfByteOffset);
        }

        private void GameProcess_ProcessHasExited(object sender, EventArgs e)
        {
            IRaddress = IntPtr.Zero;
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
            }
        }
    }
}
