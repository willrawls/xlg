using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings.Extensions;

namespace MetX.Standard.Primary.Scripts
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

        protected BaseLineProcessor(IGenerationHost host)
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

            if (InputStream != StreamReader.Null 
                && InputStream.BaseStream.Length >= 1 
                && !InputStream.EndOfStream) 
                return true;

            Host.MessageBox.Show("The supplied input is empty.", "INPUT FILE EMPTY");
            return false;

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
