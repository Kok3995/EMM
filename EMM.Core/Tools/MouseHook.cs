using System;
using System.Runtime.InteropServices;

namespace EMM.Core.Tools
{
    /// <summary>
    /// This class is used to timed mouse press duration
    /// </summary>
    public class MouseHook
    {       
        #region Hook

        private static IntPtr hookID = IntPtr.Zero;

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public LowLevelMouseProc proc = HookCallBack;

        private static IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_LBUTTONDOWN)
            {
                Messenger.Send(null, new TimerEventArgs(TimerMessage.StartTimer));
            }

            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_LBUTTONUP)
            {
                Messenger.Send(null, new TimerEventArgs(TimerMessage.StopTimer));
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public void StartHook()
        {
            hookID = SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle("user32.dll"), 0);

            if (hookID == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        public void StopHook()
        {
            UnhookWindowsHookEx(hookID);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        #endregion

        #region External methods

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}
