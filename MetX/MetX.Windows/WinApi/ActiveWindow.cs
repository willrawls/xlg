using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetX.Windows.WinApi
{
    public class ActiveWindow
    {
        public static int CurrentMoveWindowOffset = -20;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var Buff = new StringBuilder(nChars);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
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