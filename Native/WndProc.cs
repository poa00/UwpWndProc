using System;
using System.Runtime.InteropServices;

namespace WndProcTest.Native
{
    public static class WndProc
    {
        public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);
        private const int GWLP_WNDPROC = -4;

        public static readonly Lazy<IntPtr> CoreWindowHwnd = new Lazy<IntPtr>(GetCoreWindowHwnd);

        // Make sure to hold a reference to the delegate so it doesn't get garbage
        // collected, or you'll get baffling ExecutionEngineExceptions when
        // Windows tries to call your function pointer which no longer points
        // to anything.
        private static WndProcDelegate _currDelegate = null;

        public static IntPtr SetWndProc(WndProcDelegate newProc)
        {
            _currDelegate = newProc;
            
            IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newProc);
            return Interop.SetWindowLongPtr64(CoreWindowHwnd.Value, GWLP_WNDPROC, newWndProcPtr);
        }

        private static IntPtr GetCoreWindowHwnd()
        {            
            dynamic coreWindow = Windows.UI.Core.CoreWindow.GetForCurrentThread();
            var interop = (ICoreWindowInterop)coreWindow;
            return interop.WindowHandle;
        }
    }
}
