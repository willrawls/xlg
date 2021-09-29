//  Required data structures

using System;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable CommentTypo

namespace CombineFileContentsForListOfFilesOnClipboard
{
    public struct POINTAPI
    {
        public long x;
        public long y;
    }

    public struct DROPFILES
    {
        public long pFiles;
        public POINTAPI pt;
        public long fNC;
        public long fWide;
    }

    public static class X
    {
        //  Predefined Clipboard Formats
        public const int CF_TEXT = 1;
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
        public const int CF_UNICODETEXT = 13;
        public const int CF_ENHMETAFILE = 14;
        public const int CF_HDROP = 15;
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

//  Clipboard Manager Functions
        [DllImport("user32.dll")] public static extern long EmptyClipboard();
        [DllImport("user32.dll")] public static extern long OpenClipboard(long hWnd);
        [DllImport("user32.dll")] public static extern long CloseClipboard();
        [DllImport("user32.dll")] public static extern long SetClipboardData(long wFormat, IntPtr hMem);
        [DllImport("user32.dll")] public static extern long GetClipboardData(long wFormat);
        [DllImport("user32.dll")] public static extern long IsClipboardFormatAvailable(long wFormat);

//  Other required Win32 APIs
        [DllImport("shell32.dll", EntryPoint = "DragQueryFileA")]
        public static extern long DragQueryFile(long hDrop, long UINT, string lpStr, long ch);

        [DllImport("shell32.dll")]
        public static extern long DragQueryPoint(long hDrop, POINTAPI lpPoint);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(long wFlags, long dwBytes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMem(IntPtr Destination, IntPtr Source, long Length);

        public static bool ClipCopyFiles(string[] files)
        {
            DROPFILES df = new DROPFILES();
            //  Open and clear existing crud off clipboard.
            if (OpenClipboard(0) != 0)
            {
                EmptyClipboard();
                
                //  Build double-null terminated list of files.
                string data = "";
                for (int i = 0; (i <= files.Length; i++) 
                    data += files[i] + '\0';
                data += '\0';
                
                //  Allocate and get pointer to global memory,
                //  then copy file list to it.
                var sizeOfDROPFILES = Marshal.SizeOf(typeof(DROPFILES));
                int size = sizeOfDROPFILES + data.Length;

                IntPtr handleToDestinationMemory = GlobalAlloc(GHND, size);
                if (handleToDestinationMemory != IntPtr.Zero)
                {
                    IntPtr lockedHandle = GlobalLock(handleToDestinationMemory);
                    //  Build DROPFILES structure in global memory.
                    df.pFiles = sizeOfDROPFILES;
                    IntPtr pointerToDf = IntPtr.Zero;
                    Marshal.StructureToPtr(df, pointerToDf, false);

                    IntPtr pointerToData = Marshal.StringToHGlobalAuto(data);

                    CopyMem(lockedHandle, pointerToDf, sizeOfDROPFILES);
                    CopyMem((lockedHandle + sizeOfDROPFILES), pointerToData, data.Length);
                    GlobalUnlock(handleToDestinationMemory);
                    //  Copy data to clipboard, and return success.
                    if (SetClipboardData(CF_HDROP, handleToDestinationMemory) != 0)
                    {
                        CloseClipboard();
                        return true;
                    }
                }
                //  Clean up
                CloseClipboard();
            }

        }

        public static long PutFileListOnClipboard(string[] files)
        {
            long hDrop;
            long nFiles;
            long i;
            string filename;

            const long MAX_PATH = 260;
            //  Insure desired format is there, and open clipboard.
            if (IsClipboardFormatAvailable(CF_HDROP))
            {
                if (OpenClipboard(0 &))
                {
                    //  Get handle to Dropped Filelist data, and number of files.
                    hDrop = GetClipboardData(CF_HDROP);
                    nFiles = DragQueryFile(hDrop, -1 &, "", 0);
                    //  Allocate space for return and working variables.
                    string[,] Files;
                    filename = Space(MAX_PATH);
                    //  Retrieve each filename in Dropped Filelist.
                    for (i = 0;
                        (i
                         <= (nFiles - 1));
                        i++)
                    {
                        DragQueryFile(hDrop, i, filename, filename.Length);
                        Files[i] = TrimNull(filename);
                    }

                    //  Clean up
                    CloseClipboard();
                }

                //  Assign return value equal to number of files dropped.
                PutFileListOnClipboard = nFiles;
            }

        }

        public static string TrimNull(string target)
        {
            // 
            //  Truncate input string at first null.
            //  If no nulls, perform ordinary Trim.
            // 
            int indexOfNullChar = (target.IndexOf('\0') + 1);

            if(indexOfNullChar > 1)
                return target.Substring(0, indexOfNullChar - 1);
            if (indexOfNullChar == 1)
                return "";
            return target.Trim();
        }

    }
}
