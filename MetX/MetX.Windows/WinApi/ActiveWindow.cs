using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetX.Windows.WinApi
{
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