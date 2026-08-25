using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FayeOS.Services.System
{
    public class SystemService
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        public double GetRamUsagePercentage() 
        {
            MEMORYSTATUSEX memoryStatus = new MEMORYSTATUSEX();

            memoryStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

            if (!GlobalMemoryStatusEx(ref memoryStatus)) 
            {
                throw new InvalidOperationException("No se pudo obtener la informacion de memoria");
            }

            return memoryStatus.dwMemoryLoad;
        }
    }

}
