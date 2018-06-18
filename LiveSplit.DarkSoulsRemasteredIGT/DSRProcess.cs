using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal class DSRProcess
    {
        private Process _game = null;
        private Dictionary<IntPtr, byte[]> memory = new Dictionary<IntPtr, byte[]>();

        public bool IsHooked { get => _game != null; }
        public Process Process { get => _game; }

        public bool Hook()
        {
            Process[] candidates = Process.GetProcessesByName(DSRConfig.Module);
            if (candidates.Length > 0 && !candidates.First().HasExited)
            {
                _game = candidates.First();
                _game.EnableRaisingEvents = true;
                _game.Exited += Game_Exited;
                memory = DSRMemory.GetMemory(_game);
                return true;
            }
            else
            {
                Unhook();
                return false;
            }
        }

        public void Unhook()
        {
            _game = null;
            memory = new Dictionary<IntPtr, byte[]>();
        }

        public IntPtr Scan(byte?[] aob)
        {
            return DSRMemory.Scan(_game, memory, aob);
        }

        public IntPtr Scan(byte?[] aob, int offset)
        {
            return DSRMemory.Scan(_game, memory, aob, offset);
        }

        private void Game_Exited(object sender, EventArgs e)
        {
            Unhook();
        }
    }
}
