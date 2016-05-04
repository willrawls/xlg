using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using MetX.Scripts;
#pragma warning disable 1591

namespace MetX.Library
{
    public abstract class BaseLineProcessor
    {
        public StreamBuilder Output;
        public StreamReader InputStream;
        public StringBuilder OutputStringBuilder;
        
        public string DestinationFilePath;
        public string InputFilePath;

        public List<FileInfo> InputFiles;
        public int CurrentInputFileIndex;

        public bool OpenNotepad;

        public abstract bool Start();
        public abstract bool ProcessLine(string line, int number);
        public abstract bool Finish();

        public virtual FileInfo CurrentInputFile
        {
            get
            {
                if (InputFiles.IsEmpty() || CurrentInputFileIndex < 0 || CurrentInputFileIndex >= InputFiles.Count)
                    return null;

                return InputFiles[CurrentInputFileIndex];
            }
        }

        public virtual FileInfo NextInputFile
        {
            get
            {
                if (InputFiles.IsEmpty() || CurrentInputFileIndex < 0)
                    return null;

                CurrentInputFileIndex++;
                return CurrentInputFile;
            }
        }

        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="P:MetX.Library.StreamBuilder.FilePath" /> specified a file that is read-only access is not Read.-or- <see cref="P:MetX.Library.StreamBuilder.FilePath" /> specified a directory.-or- The caller does not have the required permission. -or-mode is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
        /// <exception cref="FileNotFoundException">The file specified in <see cref="P:MetX.Library.StreamBuilder.FilePath" /> was not found. </exception>
        /// <exception cref="TypeLoadException"><paramref name="type" /> is not a valid type. </exception>
        /// <exception cref="COMException"><paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found. </exception>
        /// <exception cref="InvalidComObjectException">The COM type was not obtained through <see cref="Type.GetTypeFromProgID" /> or <see cref="Type.GetTypeFromCLSID" />. </exception>
        /// <exception cref="MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
        /// <exception cref="MethodAccessException">The caller does not have permission to call this constructor. </exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception. </exception>
        /// <exception cref="SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="NotSupportedException"><paramref name="fileName" /> contains a colon (:) in the middle of the string. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="ArgumentException">The file name is empty, contains only white spaces, or contains invalid characters. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null. </exception>
        /// <exception cref="EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
        public virtual bool? ReadInput(string inputType)
        {
            InputFiles = null;
            CurrentInputFileIndex = -1;
            InputFiles = new List<FileInfo>();
            switch (inputType.ToLower().Replace(" ", string.Empty))
            {
                case "none": // This is the equivalent of reading an empty file
                    InputStream = StreamReader.Null;
                    return true;    

                case "clipboard":
                    byte[] bytes = Encoding.UTF8.GetBytes(Clipboard.GetText());
                    InputStream = new StreamReader(new MemoryStream(bytes));
                    break;

                case "databasequery":
                    throw new NotImplementedException("Database query is not yet implemented.");

                case "webaddress":
                    bytes = Encoding.UTF8.GetBytes(IO.HTTP.GetURL(InputFilePath));
                    InputStream = new StreamReader(new MemoryStream(bytes));
                    break;

                case "file":
                    if (string.IsNullOrEmpty(InputFilePath))
                    {
                        MessageBox.Show("Please supply an input filename.", "INPUT FILE PATH REQUIRED");
                        return null;
                    }
                    if (!File.Exists(InputFilePath))
                    {
                        MessageBox.Show("The supplied input filename does not exist.", "INPUT FILE DOES NOT EXIST");
                        return null;
                    }

                    switch ((Path.GetExtension(InputFilePath) ?? string.Empty).ToLower())
                    {
                        case ".xls":
                        case ".xlsx":
                            string sideFile = null;
                            FileInfo inputFile = new FileInfo(InputFilePath);
                            InputFilePath = inputFile.FullName;
                            Type excelType = Type.GetTypeFromProgID("Excel.Application");
                            dynamic excel = Activator.CreateInstance(excelType);
                            try
                            {
                                dynamic workbook = excel.Workbooks.Open(InputFilePath);
                                sideFile = InputFilePath
                                    .Replace(".xlsx", ".xls")
                                    .Replace(".xls", "_" + DateTime.Now.ToString("G").ToLower()
                                    .Replace(":", "")
                                    .Replace("/", "")
                                    .Replace(":", ""));
                                Console.WriteLine("Saving Excel as Tab delimited at: " + sideFile + "*.txt");

                                // 20 = text (tab delimited), 6 = csv
                                int sheetNumber = 0;
                                foreach (dynamic worksheet in workbook.Sheets)
                                {
                                    string worksheetFile = sideFile + "_" + (++sheetNumber).ToString("000") + ".txt";
                                    Console.WriteLine("Saving Worksheet " + sheetNumber + " as: " + worksheetFile);
                                    worksheet.SaveAs(worksheetFile, 20, Type.Missing, Type.Missing, false, false, 1);
                                    InputFiles.Add(new FileInfo(worksheetFile));
                                }
                                
                                //workbook.SaveAs(sideFile, 20, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); 
                                workbook.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            excel.Quit();
                            CurrentInputFileIndex = 0;
                            if (CurrentInputFile == null || !CurrentInputFile.Exists) return false;
                            InputStream = new StreamReader(CurrentInputFile.OpenRead());
                            break;

                        default:
                            InputFiles = new List<FileInfo>()
                            {
                                new FileInfo(InputFilePath),
                            };
                            CurrentInputFileIndex = 0;
                            if (CurrentInputFile == null || !CurrentInputFile.Exists) return false;
                            InputStream = new StreamReader(CurrentInputFile.OpenRead());
                            break;
                    }
                    break;
            }
            if (InputStream == StreamReader.Null || InputStream.BaseStream.Length < 1 || InputStream.EndOfStream)
            {
                MessageBox.Show("The supplied input is empty.", "INPUT FILE EMPTY");
                return false;
            }

            
/*
            // This way supports both windows and linux line endings
            Lines = new List<string>(AllText
                .Replace("\r", string.Empty)
                .Split(new[] { '\n' }, 
                StringSplitOptions.RemoveEmptyEntries));

            if (Lines.Count <= 0)
            {
                MessageBox.Show("The supplied input has no non-blank lines.", "INPUT FILE EMPTY");
                return false;
            }
            LineCount = Lines.Count;
*/
            return true;
        }

        /// <exception cref="ArgumentException"><paramref name="textWriter" /> is null or not writable. </exception>
        /// <exception cref="NotSupportedException"><see cref="P:MetX.Library.StreamBuilder.FilePath" /> is in an invalid format. </exception>
        /// <exception cref="ArgumentOutOfRangeException">mode or access specified an invalid value. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="ArgumentNullException">File path is required</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="P:MetX.Library.StreamBuilder.FilePath" /> specified a file that is read-only access is not Read.-or- <see cref="P:MetX.Library.StreamBuilder.FilePath" /> specified a directory.-or- The caller does not have the required permission. -or-mode is <see cref="F:System.IO.FileMode.Create" /> and the specified file is a hidden file.</exception>
        /// <exception cref="FileNotFoundException">The file specified in <see cref="P:MetX.Library.StreamBuilder.FilePath" /> was not found. </exception>
        public virtual bool SetupOutput(QuickScriptDestination destination, string location)
        {
            switch (destination)
            {
                case QuickScriptDestination.Clipboard:
                case QuickScriptDestination.TextBox:
                    OutputStringBuilder = new StringBuilder();
                    Output = new StreamBuilder(OutputStringBuilder);
                    break;

                case QuickScriptDestination.Notepad:
                case QuickScriptDestination.File:
                    Output = new StreamBuilder(location);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("destination", destination, null);
            }
            return true;
        }

        #region Ask

        public static string Ask(string title, string promptText, string defaultValue)
        {
            string value = defaultValue;
            return Ask(title, promptText, ref value) == DialogResult.Cancel ? null : value;
        }

        public static string Ask(string promptText, string defaultValue = "")
        {
            string value = defaultValue;
            return Ask("ENTER VALUE", promptText, ref value) == DialogResult.Cancel ? null : value;
        }

        public static DialogResult Ask(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        #endregion
    }
}