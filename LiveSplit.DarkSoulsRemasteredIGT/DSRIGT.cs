using System;
using System.Diagnostics;
using System.Linq;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal class DSRIGT
    {
        private Process _game;
        private IntPtr IGTAddress;
        private bool _latch = true;

        private bool IsHooked => (_game != null && !_game.HasExited);

        private int _IGT = 0;
        public int IGT
        {
            get
            {
                if (IsHooked)
                {
                    IntPtr ptr = (IntPtr)DSRIGTMemory.RInt32(_game.Handle, IGTAddress);
                    int tmpIGT = DSRIGTMemory.RInt32(_game.Handle, IntPtr.Add(ptr, DSRIGTConfig.IGTOffset));

                    // If not in the main menu, update the timer normally
                    if (tmpIGT > 0)
                    {
                        _IGT = tmpIGT;
                        _latch = false;
                    }

                    // If in the game menu and the timer hasn't be readjusted already...
                    if (tmpIGT == 0 && !_latch)
                    {
                        // When you quitout, the game saves the IGT to the savefile but the timer
                        // actually keeps ticking for 18 more frames. So we remove that from the
                        // actual timer.
                        _IGT -= DSRIGTConfig.TimerQuitoutDelay;
                        _latch = true;
                    }

                    return _IGT;
                }
                else
                {
                    // Tries to hook the game for the next call
                    Hook();
                    return _IGT;
                }
            }
        }

        public DSRIGT()
        {
            Hook();
        }

        public void Reset()
        {
            _IGT = 0;
            _latch = true;
        }

        private bool GetProcess()
        {
            Process[] candidates = Process.GetProcessesByName(DSRIGTConfig.Module);
            if (candidates.Length > 0 && !candidates.First().HasExited)
            {
                _game = candidates.First();
                _game.EnableRaisingEvents = true;
                _game.Exited += Game_Exited;
                IGTAddress = DSRIGTMemory.Scan(_game, DSRIGTConfig.ArrayOfBytes, DSRIGTConfig.ArrayOfByteOffset);
                return true;
            }
            else
            {
                Unhook();
                return false;
            }
        }

        private void Game_Exited(object sender, EventArgs e)
        {
            // Game closed or crashed
            Unhook();
        }

        private void Hook()
        {
            GetProcess();
        }

        public void Unhook()
        {
            _game = null;
            IGTAddress = IntPtr.Zero;
        }
    }
}
