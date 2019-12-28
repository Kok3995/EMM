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

        private IntPtr hookID = IntPtr.Zero;

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// To keep the delegate alive, save the instance of it here
        /// </summary>
        private LowLevelMouseProc proc;

        public void StartHook(LowLevelMouseProc callBack)
        {
            this.proc = callBack;

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

        public IntPtr GetCurrentHookID()
        {
            return this.hookID;
        }

        private const int WH_MOUSE_LL = 14;       

        #endregion

        #region External methods

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}
