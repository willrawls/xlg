using System;
using System.Windows.Forms;
using Win32Interop.Methods;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class Win32Window : IWin32Window
    {
        public static IWin32Window ActiveWindow
        {
            get { return new Win32Window(User32.GetForegroundWindow()); }
        }

        public Win32Window(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; set; }
    }
}