using System;
using System.Runtime.InteropServices;

namespace Structural.Adapter
{
    public class Process
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CreateProcess(
            string lpApplicationName, 
            string lpCommandLine, 
            IntPtr lpProcessAttributes, 
            IntPtr lpThreadAttributes, 
            bool bInheritHandles, 
            uint dwCreationFlags, 
            IntPtr lpEnvironment, 
            string lpCurrentDirectory,
            [In] ref Startupinfo lpStartupInfo, 
            out ProcessInformation lpProcessInformation
        );

        public bool Create(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref Startupinfo lpStartupInfo,
            out ProcessInformation lpProcessInformation
        )
        {
            return CreateProcess(
                lpApplicationName,
                lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes,
                bInheritHandles,
                dwCreationFlags,
                lpEnvironment,
                lpCurrentDirectory,
                ref lpStartupInfo,
                out lpProcessInformation
            );
        }
    }

    public class AdaptedPocess
    {
        private Process _process;
        public AdaptedPocess(Process process)
        {
            this._process = process;
        }

        private IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public IntPtr Create(string commandLine)
        {
            ProcessInformation pi = new ProcessInformation();
            Startupinfo si = new Startupinfo();
            si.cb = Marshal.SizeOf(si);
            if (this._process.Create(null, commandLine, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi))
                return pi.hProcess;
            else
                return INVALID_HANDLE_VALUE;
        }
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Startupinfo
    {
        public Int32 cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public Int32 dwX;
        public Int32 dwY;
        public Int32 dwXSize;
        public Int32 dwYSize;
        public Int32 dwXCountChars;
        public Int32 dwYCountChars;
        public Int32 dwFillAttribute;
        public Int32 dwFlags;
        public Int16 wShowWindow;
        public Int16 cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }
    

}