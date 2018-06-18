using System;
using System.Diagnostics;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal class DSRIGT
    {
        private DSRProcess gameProcess;
        private IntPtr IGTAddress;
        private bool latch = true;
        private Stopwatch stopwatch = new Stopwatch();
        private long lastUpdate;

        private int _IGT = 0;
        public int IGT
        {
            get
            {
                if (gameProcess.IsHooked)
                {
                    // 60 times per seconds
                    if (stopwatch.ElapsedMilliseconds - lastUpdate > DSRIGTConfig.IGTRefreshRate)
                    {
                        IntPtr ptr = (IntPtr)DSRMemory.RInt32(gameProcess.Process.Handle, IGTAddress);
                        int tmpIGT = DSRMemory.RInt32(gameProcess.Process.Handle, IntPtr.Add(ptr, DSRIGTConfig.IGTOffset));

                        // If not in the main menu, update the timer normally
                        if (tmpIGT > 0)
                        {
                            _IGT = tmpIGT;
                            latch = false;
                        }

                        // If in the game menu and the timer hasn't be readjusted already...
                        if (tmpIGT == 0 && !latch)
                        {
                            // When you quitout, the game saves the IGT to the savefile but the timer
                            // actually keeps ticking for 18 more frames. So we remove that from the
                            // actual timer.
                            _IGT -= DSRIGTConfig.TimerQuitoutDelay;
                            latch = true;
                        }

                        lastUpdate = stopwatch.ElapsedMilliseconds;
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

        public DSRIGT(DSRProcess gameProcess)
        {
            this.gameProcess = gameProcess;
            Hook();
        }

        public void Reset()
        {
            _IGT = 0;
            latch = true;
            lastUpdate = 0;
            stopwatch.Restart();
        }

        private void Hook()
        {
            if (gameProcess.Hook())
            {
                // Game hooked! Find the IGT address using AOB scanning
                IGTAddress = gameProcess.Scan(DSRIGTConfig.ArrayOfBytes, DSRIGTConfig.ArrayOfByteOffset);
                lastUpdate = 0;
                stopwatch.Restart();
            } else
            {
                IGTAddress = IntPtr.Zero;
                lastUpdate = 0;
                stopwatch.Stop();
            }
        }
    }
}
