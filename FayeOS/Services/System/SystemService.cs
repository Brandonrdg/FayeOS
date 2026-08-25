using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FayeOS.Services.System
{
    public class SystemService
    {
        //Estructuras y funciones para obtener el uso de de CPU y RAM
        [StructLayout(LayoutKind.Sequential)]
        public struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetSystemTimes
            (
                out FILETIME IdleTime,
                out FILETIME KernelTime,
                out FILETIME UserTime
            );
        private static ulong FileTimeToUInt64(FILETIME time)
        {
            return ((ulong)time.dwHighDateTime << 32) | time.dwLowDateTime;
        }
        private ulong previousIdleTime;
        private ulong previousKernelTime;
        private ulong previousUserTime;

        private bool hasPreviousCpuSample;

        public double GetCpuUsagePercentage() 
        {
            if(!GetSystemTimes
                (
                    out FILETIME idle,
                    out FILETIME kernel,
                    out FILETIME user
                ))
            {
                throw new InvalidOperationException("No se pudo obtener la informacion del sistema");
            }
            ulong idleTime = FileTimeToUInt64(idle);
            ulong kernelTime = FileTimeToUInt64(kernel);
            ulong userTime = FileTimeToUInt64(user);

            if (!hasPreviousCpuSample) 
            {
                previousIdleTime = idleTime;
                previousKernelTime = kernelTime;
                previousUserTime = userTime;
                hasPreviousCpuSample = true;
                return 0;
            }
            ulong idleDiff = idleTime - previousIdleTime;
            ulong kernelDiff = kernelTime - previousKernelTime;
            ulong userDiff = userTime - previousUserTime;

            ulong totalDiff = kernelDiff + userDiff;

            double cpuUsage = totalDiff == 0 ? 0 : (double)(totalDiff - idleDiff) / totalDiff * 100;

            previousIdleTime = idleTime;
            previousKernelTime = kernelTime;
            previousUserTime = userTime;

            return cpuUsage;
        }
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
