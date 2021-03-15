using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MetX.Windows.WinApi
{
    public class User32
    {
        public const short SWP_NOMOVE = 0X2;
        public const short SWP_NOSIZE = 1;
        public const short SWP_NOZORDER = 0X4;
        public const int SWP_SHOWWINDOW = 0x0040;


        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy,
            int wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; // x position of upper-left corner
            public int Top; // y position of upper-left corner
            public int Right; // x position of lower-right corner
            public int Bottom; // y position of lower-right corner
        }
    }

    public class ActiveWindow
    {
        public static int CurrentMoveWindowOffset = -20;

        public static void Move(Process process)
        {
            try
            {
                if (process == null || process.HasExited) return;

                Task.Run(() =>
                {
                    var handle = process.MainWindowHandle;
                    if (User32.GetWindowRect(handle, out var activeWindowLocation))
                    {
                        CurrentMoveWindowOffset += 20;
                        if (CurrentMoveWindowOffset > 60)
                            CurrentMoveWindowOffset = -60;

                        var width = activeWindowLocation.Right - activeWindowLocation.Left;
                        var height = activeWindowLocation.Bottom - activeWindowLocation.Top;
                        var left = activeWindowLocation.Left + CurrentMoveWindowOffset;
                        var top = activeWindowLocation.Top + CurrentMoveWindowOffset;

                        if (left < 100)
                            left = 100;
                        if (top < 10)
                            top = 10;

                        User32.SetWindowPos(handle, 0, left, top, width, height,
                            User32.SWP_NOZORDER | User32.SWP_SHOWWINDOW | User32.SWP_NOSIZE);
                    }
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}