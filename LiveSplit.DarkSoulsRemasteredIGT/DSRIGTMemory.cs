using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiveSplit.DarkSoulsRemasteredIGT
{
    internal static class DSRIGTMemory
    {
        #region [Memory functions]
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public static Int32 RInt32(IntPtr HANDLE, IntPtr addr)
        {
            int bytesRead = 0;
            byte[] _rtnBytes = new byte[4];
            ReadProcessMemory(HANDLE, addr, _rtnBytes, _rtnBytes.Length, ref bytesRead);
            return BitConverter.ToInt32(_rtnBytes, 0);
        }

        public static UInt32 RUInt32(IntPtr HANDLE, IntPtr addr)
        {
            int bytesRead = 0;
            byte[] _rtnBytes = new byte[4];
            ReadProcessMemory(HANDLE, addr, _rtnBytes, _rtnBytes.Length, ref bytesRead);
            return BitConverter.ToUInt32(_rtnBytes, 0);
        }

        public static Int32 RInt32(IntPtr HANDLE, long addr)
        {
            return RInt32(HANDLE, new IntPtr(addr));
        }

        public static void WUInt32(IntPtr HANDLE, IntPtr addr, UInt32 val)
        {
            int bytesRead = 0;
            WriteProcessMemory(HANDLE, addr, BitConverter.GetBytes(val), 4, ref bytesRead);
        }

        public static byte[] ReadBytes(IntPtr HANDLE, IntPtr address, int size)
        {
            int bytesRead = 0;
            byte[] result = new byte[size];
            ReadProcessMemory(HANDLE, address, result, size, ref bytesRead);
            return result;
        }
        #endregion

        #region [AOB SCANNING]
        private const uint MEM_COMMIT = 0x1000;
        private const uint PAGE_GUARD = 0x100;
        private const uint PAGE_EXECUTE_ANY = 0xF0;

        [DllImport("kernel32.dll")]
        private static extern uint VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [StructLayout(LayoutKind.Sequential)]
        protected struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public ulong RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        private static Dictionary<IntPtr, byte[]> GetMemory(Process process)
        {
            List<MEMORY_BASIC_INFORMATION> memRegions = new List<MEMORY_BASIC_INFORMATION>();

            IntPtr memRegionAddr = process.MainModule.BaseAddress;
            IntPtr mainModuleEnd = process.MainModule.BaseAddress + process.MainModule.ModuleMemorySize;
            uint queryResult;

            do
            {
                MEMORY_BASIC_INFORMATION memInfo = new MEMORY_BASIC_INFORMATION();
                queryResult = VirtualQueryEx(process.Handle, memRegionAddr, out memInfo, (uint)Marshal.SizeOf(memInfo));
                if (queryResult != 0)
                {
                    if ((memInfo.State & MEM_COMMIT) != 0 && (memInfo.Protect & PAGE_GUARD) == 0 && (memInfo.Protect & PAGE_EXECUTE_ANY) != 0)
                    {
                        memRegions.Add(memInfo);
                    }
                    memRegionAddr = (IntPtr)((ulong)memInfo.BaseAddress.ToInt64() + memInfo.RegionSize);
                }
            } while (queryResult != 0 && memRegionAddr.ToInt64() < mainModuleEnd.ToInt64());

            Dictionary<IntPtr, byte[]> readMemory = new Dictionary<IntPtr, byte[]>();
            foreach (MEMORY_BASIC_INFORMATION memRegion in memRegions)
            {
                readMemory[memRegion.BaseAddress] = ReadBytes(process.Handle, memRegion.BaseAddress, (int)memRegion.RegionSize);
            }

            return readMemory;
        }
        
        public static IntPtr Scan(Process process, byte?[] aob)
        {
            List<IntPtr> results = new List<IntPtr>();
            Dictionary<IntPtr, byte[]> readMemory = GetMemory(process);

            foreach (IntPtr baseAddress in readMemory.Keys)
            {
                byte[] bytes = readMemory[baseAddress];

                for (int i = 0; i < bytes.Length - aob.Length; i++)
                {
                    bool found = true;
                    for (int j = 0; j < aob.Length; j++)
                    {
                        if (aob[j] != null && aob[j] != bytes[i + j])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        results.Add(baseAddress + i);
                    }
                }
            }

            if (results.Count == 0)
            {
                throw new ArgumentException(string.Format(DSRIGTConfig.AOBNotFound, aob.ToString()));
            }
            else if (results.Count > 1)
            {
                throw new ArgumentException(string.Format(DSRIGTConfig.AOBMultiple, results.Count, aob.ToString()));
            }
            return results[0];
        }

        public static IntPtr Scan(Process process, byte?[] aob, int offset)
        {
            IntPtr result = Scan(process, aob);
            return result + RInt32(process.Handle, result + offset) + offset + 4;
        }
        #endregion
    }
}
