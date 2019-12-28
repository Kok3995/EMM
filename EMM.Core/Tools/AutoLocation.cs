using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace EMM.Core.Tools
{
    public class AutoLocation
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        };

        public IntPtr GetForegroundWindowHandler()
        {
            return GetForegroundWindow();
        }

        public Point GetClientCoordinates(IntPtr clientHandle)
        {
            POINT point = new POINT();
            GetCursorPos(ref point);
            ScreenToClient(clientHandle, ref point);

            return new Point(point.x, point.y);
        }

        public Rect GetClientRect(IntPtr clientHandle)
        {
            RECT rect = new RECT();
            GetClientRect(clientHandle, ref rect);

            return new Rect(new Point(rect.left, rect.top), new Point(rect.right, rect.bottom));
        }
    }
}
