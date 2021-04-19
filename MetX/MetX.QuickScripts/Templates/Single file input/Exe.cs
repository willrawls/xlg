using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using System.Windows.Forms.VisualStyles;
using MetX;
using MetX.IO;
using MetX.Standard.Data;
using MetX.Scripts;
using MetX.Standard.Library;
using Microsoft.CSharp;

//~~Usings~~//

namespace MetX.Scripts.Executable
{
    // //~~NameInstance~~//
    static class Program
    {
        [STAThread()]
        static void Main(string[] args)
        {
            try
            {
                QuickScriptProcessor processor = new QuickScriptProcessor();

                processor.InputFilePath = //~~InputFilePath~~//;
                processor.DestinationFilePath = //~~DestinationFilePath~~//;

                if (args.Length > 0)
                    processor.InputFilePath = args[0];
                if (args.Length > 1)
                    processor.InputFilePath = args[1];
                if (args.Length > 2 && args[2].ToLower() == "open") 
                    processor.OpenNotepad = true;

                if (!processor.ReadInput()) return;

                if (!processor.Start()) return;

                for(int number = 0; number < processor.Lines.Count; number++)
                {
                    if (number > 0 && number % 100 == 0) Console.WriteLine("Lines processed: " + number);
                    string currLine = processor.Lines[number];
                    try
                    {
                        if(!processor.ProcessLine(currLine, number)) return;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error processing line " + (number+1) + ":" + Environment.NewLine +
                            currLine + Environment.NewLine +
                            Environment.NewLine +
                            "CONTINUE PROCESSING ?");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        if (answer.Key.ToString().ToLower() == "n") return;
                    }
                }

                if (!processor.Finish()) return;

                if (processor.Output == null || processor.Output.Length == 0) return;

                if (string.IsNullOrEmpty(processor.DestinationFilePath)) 
                    processor.DestinationFilePath = "Output.txt";
                File.WriteAllText(processor.DestinationFilePath, processor.Output.ToString());

                if(processor.OpenNotepad)
                    System.Diagnostics.Process.Start("notepad", processor.DestinationFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public class QuickScriptProcessor
    {
        public readonly StringBuilder OutputStringBuilder = new StringBuilder();
        public readonly StreamBuilder Output;
        public readonly List<string> Files = new List<string>();
        public List<string> Lines = new List<string>();
        public string AllText;
        public string DestinationFilePath;
        public string InputFilePath;
        public int LineCount;
        public bool OpenNotepad;

        public QuickScriptProcessor()
        {
            Output = new StreamBuilder(OutputStringBuilder);
        }
        
        //~~ClassMembers~~//

        public bool Start()
        {
            //~~Start~~//
            return true;
        }

        public bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;
            //~~ProcessLine~~//
            return true;
        }

        public bool Finish()
        {
            //~~Finish~~//
            return true;
        }

        public bool ReadInput()
        {
//~~ReadInput~~//
            try
            {
                if (string.IsNullOrEmpty(InputFilePath)) InputFilePath = "none";

                Console.WriteLine("Input: " + InputFilePath);
                switch(InputFilePath.ToLower())
                {
                    case "none": // This is the equivalent of reading an empty file
                        AllText = string.Empty;
                        Lines = new List<string> {string.Empty};
                        LineCount = 1;
                        return true;    

                    case "clipboard":
                        AllText = Clipboard.GetText();
                        break;

                    default:
                        /*
                        if (InputFilePath.StartsWith("http"))
                        {
                            AllText = MetX.IO.HTTP.GetURL(InputFilePath);
                        }
                        else if (!File.Exists(InputFilePath))
                        */
                        if (!File.Exists(InputFilePath))
                        {
                            Console.WriteLine("Input file missing: " + InputFilePath);
                            return false;
                        }
                        else
                        {
                            switch ((Path.GetExtension(InputFilePath) ?? string.Empty).ToLower())
                            {
                                case "xls":
                                case "xlsx":
                                case ".xls":
                                case ".xlsx":
                                    string sideFile = null;
                                    FileInfo inputFile = new FileInfo(InputFilePath);
                                    InputFilePath = inputFile.FullName;
                                    // TODO Find another way to do the Excel thing
/* 
                                    Type ExcelType = Type.GetTypeFromProgID("Excel.Application");
                                    dynamic excel = Activator.CreateInstance(ExcelType);
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
                                            string worksheetFile = sideFile + "_" + ++sheetNumber + ".txt";
                                            Console.WriteLine("Saving Worksheet " + sheetNumber + " as: " + worksheetFile);
                                            worksheet.SaveAs(worksheetFile, 20, Type.Missing, Type.Missing, false, false, 1);
                                            if (sideFile == null) sideFile = worksheetFile;
                                            Files.Add(worksheetFile);
                                        }
                                        workbook.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                        return false;
                                    }
                                    excel.Quit();
*/
                                    GC.Collect(5);
                                    AllText = File.ReadAllText(Files[0]);
                                    break;

                                default:
                                    AllText = File.ReadAllText(InputFilePath);
                                    Files.Add(InputFilePath);
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            if (string.IsNullOrEmpty(AllText))
            {
                Console.WriteLine("Input empty.");
                return false;
            }

            Lines = new List<string>(AllText
                .Replace("\r", string.Empty)
                .Split(new[] { '\n' }, StringSplitOptions
                .RemoveEmptyEntries));

            LineCount = Lines.Count;

            return true;
        }


        public static string Ask(string title, string promptText, string defaultValue)
        {
            string value = defaultValue;
            return Ask(title, promptText, ref value) == MessageBoxResult.Cancel
                ? null
                : value;
        }

        public static string Ask(string promptText, string defaultValue = "")
        {
            string value = defaultValue;
            return Ask("ENTER VALUE", promptText, ref value) == MessageBoxResult.Cancel
                ? null
                : value;
        }

        public static MessageBoxResult Ask(string title, string promptText, ref string value)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine(title);
            Console.WriteLine();
            Console.WriteLine(promptText);

            value = Console.ReadLine().AsString().Trim();
            MessageBoxResult MessageBoxResult = value.IsNotEmpty() ? MessageBoxResult.OK : MessageBoxResult.Cancel;
            return MessageBoxResult;
            
            /*
            IGenerationHost form = new GenerationHost();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.MessageBoxResult = MessageBoxResult.OK;
            buttonCancel.MessageBoxResult = MessageBoxResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            MessageBoxResult MessageBoxResult = form.ShowDialog();
            value = textBox.Text;
        */
        }
    }
}