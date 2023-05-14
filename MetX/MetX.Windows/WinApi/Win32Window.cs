using System;

namespace MetX.Windows.WinApi;

public class Win32Window : System.Windows.Forms.IWin32Window
{
    public IntPtr Handle { get; }

    public Win32Window(IntPtr handle)
    {
        Handle = handle;
    }
}