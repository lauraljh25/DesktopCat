using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopCat
{
    public static class MouseHandler
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static void SetMousePosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetMousePosition(Point point)
        {
            int x = Convert.ToInt32(point.X);
            int y = Convert.ToInt32(point.Y);

            SetCursorPos(x, y);
        }

        public static void ShowMouse()
        {
            ShowCursor(true);
        }

        public static void HideMouse()
        {
            ShowCursor(false);
        }
    }
}
