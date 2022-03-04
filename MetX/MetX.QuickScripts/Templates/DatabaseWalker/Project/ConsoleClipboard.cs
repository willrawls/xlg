using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable CommentTypo

namespace //~~NameInstance~~//
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTAPI
    {
        public long x;
        public long y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DROPFILES
    {
        public long pFiles;
        public POINTAPI pt;
        public long fNC;
        public long fWide;
    }

    public class ConsoleClipboard : IDisposable
    {
        // ---

        //  Predefined Clipboard Formats
        public const int CF_TEXT = 1;                   // Supported
        public const int CF_BITMAP = 2;
        public const int CF_METAFILEPICT = 3;
        public const int CF_SYLK = 4;
        public const int CF_DIF = 5;
        public const int CF_TIFF = 6;
        public const int CF_OEMTEXT = 7;
        public const int CF_DIB = 8;
        public const int CF_PALETTE = 9;
        public const int CF_PENDATA = 10;
        public const int CF_RIFF = 11;
        public const int CF_WAVE = 12;
        public const int CF_UNICODETEXT = 13;           // Supported
        public const int CF_ENHMETAFILE = 14;
        public const int CF_HDROP = 15;                 // Supported
        public const int CF_LOCALE = 16;
        public const int CF_MAX = 17;
        public const int GMEM_FIXED = 0;
        public const int GMEM_MOVEABLE = 2;
        public const int GMEM_NOCOMPACT = 16;
        public const int GMEM_NODISCARD = 32;
        public const int GMEM_ZEROINIT = 64;
        public const int GMEM_MODIFY = 128;
        public const int GMEM_DISCARDABLE = 256;
        public const int GMEM_NOT_BANKED = 4096;
        public const int GMEM_SHARE = 8192;
        public const int GMEM_DDESHARE = 8192;
        public const int GMEM_NOTIFY = 16384;
        public const int GMEM_LOWER = GMEM_NOT_BANKED;
        public const int GMEM_VALID_FLAGS = 32626;
        public const int GMEM_INVALID_HANDLE = 32768;
        public const int GHND = GMEM_MOVEABLE | GMEM_ZEROINIT;
        public const int GPTR = GMEM_FIXED | GMEM_ZEROINIT;

        //  New shell-oriented clipboard formats
        public const string CFSTR_SHELLIDLIST = "Shell IDList Array";
        public const string CFSTR_SHELLIDLISTOFFSET = "Shell Object Offsets";
        public const string CFSTR_NETRESOURCES = "Net Resource";
        public const string CFSTR_FILEDESCRIPTOR = "FileGroupDescriptor";
        public const string CFSTR_FILECONTENTS = "FileContents";
        public const string CFSTR_FILENAME = "FileName";
        public const string CFSTR_PRINTERGROUP = "PrinterFriendlyName";
        public const string CFSTR_FILENAMEMAP = "FileNameMap";

        public const int MAX_PATH = 260;

        // ---
        [DllImport("user32.dll", SetLastError=true)] static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")] static extern bool CloseClipboard();
        [DllImport("user32.dll")] static extern bool SetClipboardData(uint uFormat, IntPtr data);
        [DllImport("user32.dll")] static extern IntPtr GetClipboardData(uint uFormat);
        //[DllImport("user32.dll", SetLastError = true)] public static extern uint EnumClipboardFormats(uint format);
        [DllImport("kernel32.dll", SetLastError = true)] public static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")] public static extern UIntPtr GlobalSize(IntPtr hMem);
        [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)] public static extern bool GlobalUnlock(IntPtr hMem);

        //  Clipboard Manager Functions
        [DllImport("user32.dll")] static extern long EmptyClipboard();
        [DllImport("user32.dll")] static extern long IsClipboardFormatAvailable(long wFormat);

        [DllImport("shell32.dll")] static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder lpszFile, uint cch);
        [DllImport("shell32.dll", EntryPoint = "DragQueryFileA")] static extern long DragQueryFile(IntPtr hDrop, long iFile, [MarshalAs(UnmanagedType.LPStr)] string lpStr, long ch);

        [DllImport("kernel32.dll")] static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")] static extern void CopyMem(IntPtr Destination, [MarshalAs(UnmanagedType.LPStr)] string Source, long Length);

        // ---

        public ConsoleClipboard()
        {
            CloseClipboard();
        }

        /// <summary>
        /// Gets the data on the clipboard in the format specified by the selected item of the specified listbox.
        /// </summary>
        public string GetText()
        {
            if (IsClipboardFormatAvailable(CF_UNICODETEXT) == 0) return "";
            if (!OpenClipboard()) return "";

            IntPtr pointer = IntPtr.Zero;
            try
            {
                //Get pointer to clipboard data in the selected format
                pointer = GetClipboardData(CF_UNICODETEXT);

                //Do a bunch of crap necessary to copy the data from the memory
                //the above pointer points at to a place we can access it.
                var length = GlobalSize(pointer);
                var @lock = GlobalLock(pointer);

                //Init a buffer which will contain the clipboard data
                byte[] buffer = new byte[(int)length];

                //Copy clipboard data to buffer
                Marshal.Copy(@lock, buffer, 0, (int)length);

                if (buffer.Length > 0)
                    return Encoding.ASCII.GetString(buffer).Replace("\0", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                CloseClipboard();
                if (pointer != IntPtr.Zero)
                    GlobalUnlock(pointer);
            }

            return "";
        }

        public bool AreFilenamesOnTheClipboard()
        {
            return IsClipboardFormatAvailable(CF_HDROP) != 0;
        }

        public string[] GetFilenames()
        {
            string[] files = Array.Empty<string>();

            //  Insure desired format is there, and open clipboard.
            if (IsClipboardFormatAvailable(CF_HDROP) == 0) return files;
            if (!OpenClipboard()) return files;

            try
            {
                //  Get handle to Dropped Filelist data, and number of files.
                IntPtr handleToDroppedFilenames = GetClipboardData(CF_HDROP);
                var numberofFilenames = DragQueryFile(handleToDroppedFilenames, -1, "", 0);

                //  Allocate space for return and working variables.
                files = new string[numberofFilenames];

                //  Retrieve each filename in Dropped Filelist.
                for (uint i = 0; i <= numberofFilenames - 1; i++)
                {
                    StringBuilder filename = new StringBuilder(new string(' ', MAX_PATH));
                    DragQueryFile(handleToDroppedFilenames, i, filename, MAX_PATH);
                    files[i] = ForStrings.BeforeNullOrTrim(filename.ToString());
                }
            }
            finally
            {
                //  Clean up
                CloseClipboard();
            }

            //  Assign return value equal to number of files dropped.
            return files;

        }

        public bool Set(string text)
        {
            if (!OpenClipboard()) return false;

            IntPtr pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.StringToHGlobalUni(text);
                SetClipboardData(CF_UNICODETEXT, pointer);
            }
            catch
            {
                return false;
            }
            finally
            {
                CloseClipboard();
            }

            return true;
        }

        public bool Set(string[] files)
        {
            if (OpenClipboard())
            {
                IntPtr handleToDestinationMemory = IntPtr.Zero;
                try
                {
                    EmptyClipboard();
                    if (files.IsEmpty())
                        return true;

                    //  Build double-null terminated list of files.
                    DROPFILES df = new DROPFILES();
                    string data = "";
                    string[] nonEmptyFilenames = files.Where(f => f.IsNotEmpty()).ToArray();
                    if (nonEmptyFilenames.Length == 0)
                        return true;

                    for (int i = 0; i <= nonEmptyFilenames.Length - 1; i++)
                        data += nonEmptyFilenames[i] + '\0';
                    data += '\0';

                    //  Allocate and get pointer to global memory,
                    //  then copy file list to it.
                    var sizeOfDROPFILES = Marshal.SizeOf(typeof(DROPFILES));
                    int size = sizeOfDROPFILES + data.Length;

                    handleToDestinationMemory = Marshal.AllocHGlobal(size);

                    if (handleToDestinationMemory != IntPtr.Zero)
                    {
                        //  Build DROPFILES structure in global memory.
                        df.pFiles = sizeOfDROPFILES;
                        IntPtr pointerToDestinationMemory = GlobalLock(handleToDestinationMemory);
                        Marshal.StructureToPtr(df, pointerToDestinationMemory, false);

                        //IntPtr pointerToData = Marshal.StringToHGlobalUni(data);
                        CopyMem(pointerToDestinationMemory + sizeOfDROPFILES, data, data.Length); //pointerToData, data.Length);

                        //GlobalUnlock(handleToDestinationMemory);
                        //  Copy data to clipboard, and return success.
                        if (SetClipboardData(CF_HDROP, handleToDestinationMemory))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    //  Clean up
                    CloseClipboard();
                }
            }

            return false;
        }

        public bool OpenClipboard()
        {
            int attemptsLeft = 5;
            int millisecondsToSleep = 50;
            while (attemptsLeft > 0)
            {
                try
                {
                    CloseClipboard();
                    if (OpenClipboard(IntPtr.Zero))
                        return true;
                }
                catch 
                {
                    CloseClipboard();
                    if(--attemptsLeft > 0)
                    {
                        Thread.Sleep(millisecondsToSleep);
                        millisecondsToSleep += 200;
                        CloseClipboard();
                    }
                }
            }

            return false;
        }

        private void ReleaseUnmanagedResources()
        {
            CloseClipboard();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~ConsoleClipboard()
        {
            ReleaseUnmanagedResources();
        }
    }
}