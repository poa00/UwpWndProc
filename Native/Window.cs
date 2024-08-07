using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static WndProcTest.Native.Interop;

namespace WndProcTest.Native
{
    public static class Window
    {
        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;

        public static void FlashWindow()
        {
            IntPtr hwnd = WndProc.CoreWindowHwnd.Value;
            FLASHWINFO info = new FLASHWINFO();
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.hwnd = hwnd;
            info.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            info.uCount = 5;
            info.dwTimeout = 0;

            bool success = Interop.FlashWindowEx(ref info);
            if (!success)
            {
                int lastError = Marshal.GetLastWin32Error();
                if (lastError != 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
    }
}
