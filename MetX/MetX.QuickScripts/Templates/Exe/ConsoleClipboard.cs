using System;
using System.Text;
using System.Runtime.InteropServices;

namespace //~~NameInstance~~//
{
    public class ConsoleClipboard
    {
        [DllImport("user32.dll")] static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")] static extern bool CloseClipboard();
        [DllImport("user32.dll")] static extern bool SetClipboardData(uint uFormat, IntPtr data);
        [DllImport("user32.dll")] static extern IntPtr GetClipboardData(uint uFormat);
        [DllImport("user32.dll", SetLastError = true)] public static extern uint EnumClipboardFormats(uint format);

        [DllImport("kernel32.dll", SetLastError = true)] public static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")] public static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)] public static extern bool GlobalUnlock(IntPtr hMem);


        /// <summary>
        /// Gets the data on the clipboard in the format specified by the selected item of the specified listbox.
        /// </summary>
        public string GetText()
        {
            uint format = 13;
            if (format != 0)
            {
                OpenClipboard(IntPtr.Zero);

                //Get pointer to clipboard data in the selected format
                IntPtr pointer = GetClipboardData(format);

                //Do a bunch of crap necessary to copy the data from the memory
                //the above pointer points at to a place we can access it.
                var length = GlobalSize(pointer);
                var @lock = GlobalLock(pointer);

                //Init a buffer which will contain the clipboard data
                byte[] buffer = new byte[(int) length];

                //Copy clipboard data to buffer
                Marshal.Copy(@lock, buffer, 0, (int) length);
                CloseClipboard();

                if (buffer.Length > 0)
                    return Encoding.ASCII.GetString(buffer).Replace("\0", "");
            }

            return "";
        }

        public bool SetText(string text)
        {
            OpenClipboard(IntPtr.Zero);
            IntPtr pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.StringToHGlobalUni(text);
                SetClipboardData(13, pointer);
                CloseClipboard();
            }
            catch // (Exception e)
            {
                return false;
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pointer);
            }

            return true;
        }
    }
}