using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Standard.Interfaces;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public abstract class BaseLineProcessor
    {
        public int CurrentInputFileIndex;
        public string DestinationFilePath;
        public string InputFilePath;
        public List<FileInfo> InputFiles;
        public StreamReader InputStream;
        public bool OpenNotepad;
        public StreamBuilder Output;
        public StringBuilder OutputStringBuilder;

        public BaseLineProcessor(IGenerationHost host)
        {
            Host = host;
        }

        //public IGenerationHost Gui { get; set; }
        public IGenerationHost Host { get; set; }

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

        public abstract bool Finish();

        public abstract bool ProcessLine(string line, int number);

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
                {
                    //throw new NotImplementedException();
                    
                    var clipboardBytes = Encoding.UTF8.GetBytes(Host.GetTextForProcessing());
                    InputStream = new StreamReader(new MemoryStream(clipboardBytes));
                    break;
                }                    

                case "databasequery":
                    throw new NotImplementedException("Database query is not yet implemented.");

                case "webaddress":
                    var bytes = Encoding.UTF8.GetBytes(Http.GetUrl(InputFilePath));
                    InputStream = new StreamReader(new MemoryStream(bytes));
                    break;

                case "file":
                    if (string.IsNullOrEmpty(InputFilePath))
                    {
                        
                        Host.MessageBox.Show("Please supply an input filename.", "INPUT FILE PATH REQUIRED");
                        return null;
                    }

                    if (!File.Exists(InputFilePath))
                    {
                        Host.MessageBox.Show("The supplied input filename does not exist.", "INPUT FILE DOES NOT EXIST");
                        return null;
                    }

                    switch (Path.GetExtension(InputFilePath)?.ToLower())
                    {
                        case ".xls":
                        case ".xlsx":
                            throw new NotImplementedException();
                            /*string sideFile;
                            var inputFile = new FileInfo(InputFilePath);
                            InputFilePath = inputFile.FullName;
                            var excelType = Type.GetTypeFromProgID("Excel.Application");
                            dynamic excel = Activator.CreateInstance(excelType ?? throw new InvalidOperationException());
                            try
                            {
                                if (excel != null)
                                {
                                    var workbook = excel.Workbooks.Open(InputFilePath);
                                    sideFile = InputFilePath
                                        .Replace(".xlsx", ".xls")
                                        .Replace(".xls", "_" + DateTime.Now.ToString("G").ToLower()
                                            .Replace(":", string.Empty)
                                            .Replace("/", string.Empty)
                                            .Replace(":", string.Empty));
                                    Console.WriteLine("Saving Excel as Tab delimited at: " + sideFile + "*.txt");

                                    // 20 = text (tab delimited), 6 = csv
                                    var sheetNumber = 0;
                                    foreach (var worksheet in workbook.Sheets)
                                    {
                                        var worksheetFile = sideFile + "_" + (++sheetNumber).ToString("000") + ".txt";
                                        Console.WriteLine("Saving Worksheet " + sheetNumber + " as: " + worksheetFile);
                                        worksheet.SaveAs(worksheetFile, 20, Type.Missing, Type.Missing, false, false, 1);
                                        InputFiles.Add(new FileInfo(worksheetFile));
                                    }

                                    // workbook.SaveAs(sideFile, 20, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                                    workbook.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }

                            excel?.Quit();
                            CurrentInputFileIndex = 0;
                            if (CurrentInputFile == null || !CurrentInputFile.Exists) return false;
                            InputStream = new StreamReader(CurrentInputFile.OpenRead());
                            break;
                            */

                        default:
                            InputFiles = new List<FileInfo> {
                                new(InputFilePath ?? throw new InvalidOperationException())
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
                Host.MessageBox.Show("The supplied input is empty.", "INPUT FILE EMPTY");
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
                            Host.MessageBox.Show("The supplied input has no non-blank lines.", "INPUT FILE EMPTY");
                            return false;
                        }
                        LineCount = Lines.Count;
            */
            return true;
        }

        public virtual bool SetupOutput(QuickScriptDestination destination, ref string location)
        {
            switch (destination)
            {
                case QuickScriptDestination.Clipboard:
                case QuickScriptDestination.TextBox:
                    OutputStringBuilder = new StringBuilder();
                    Output = new StreamBuilder(OutputStringBuilder);
                    break;

                case QuickScriptDestination.Notepad:
                    if (location.IsEmpty())
                    {
                        location = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Left(13) + ".txt");
                    }
                    Output = new StreamBuilder(location);
                    break;

                case QuickScriptDestination.File:
                    Output = new StreamBuilder(location);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(destination), destination, null);
            }

            if (!Output.IsOpenAndReady)
            {
                Host.MessageBox.Show("Couldn't create/open the output file");
                return false;
            }

            return true;
        }

        public abstract bool Start();
    }
}
