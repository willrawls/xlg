using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Processor 
{
    // //~~NameInstance~~//
    static class Program
    {
        static string InputFilePath;
        static string DestinationFilePath;
        static string AllText;
        static int LineCount;
        static bool OpenNotepad;

        static List<string> AllLines = new List<string>();
        static StringBuilder Output = new StringBuilder();

//~~ClassMembers~~//

        [STAThread()]
        static void Main(string[] args)
        {
            try
            {
                InputFilePath = //~~InputFilePath~~//;
                DestinationFilePath = //~~DestinationFilePath~~//;

                if (args.Length > 0)
                    InputFilePath = args[0];
                if (args.Length > 1)
                    InputFilePath = args[1];
                if (args.Length > 2 && args[2].ToLower() == "open") 
                    OpenNotepad = true;

                if (!File.Exists(InputFilePath)) Console.WriteLine("Input file missing: " + InputFilePath);

                ReadFile();
                if(string.IsNullOrEmpty(AllText))
                {
                    Console.WriteLine("Input file empty: " + InputFilePath);
                    return;
                }

                AllLines = new List<string>(AllText
                    .Replace("\r", string.Empty)
                    .Split(new[] { '\n' }, StringSplitOptions
                    .RemoveEmptyEntries));

                LineCount = AllLines.Count;

                Start();

                for(int number = 0; number < AllLines.Count; number++)
                {
                    if (number > 0 && number % 100 == 0) Console.WriteLine("Lines processed: " + number);
                    string currLine = AllLines[number];
                    try
                    {
                        if(!ProcessLine(currLine, number)) return;
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

                Finish();

                if (Output == null || Output.Length == 0) return;

                File.WriteAllText(DestinationFilePath, Output.ToString());

                if(OpenNotepad)
                    System.Diagnostics.Process.Start("notepad", DestinationFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void Start()
        {
//~~Start~~//
        }

        static void ReadFile()
        {
            try
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
                                .Replace(":", "") + ".txt");
                            Console.WriteLine("Saving Excel as Tab delimited at: " + sideFile);
                            workbook.SaveAs(sideFile, 20); // 20 = text, 6 = csv
                            workbook.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        excel.Quit();
                        AllText = File.ReadAllText(sideFile);
                        break;

                    default:
                        AllText = File.ReadAllText(InputFilePath);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static bool ProcessLine(string line, int number)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;
//~~ProcessLine~~//
            return true;
        }

        static void Finish()
        {
//~~Finish~~//
        }

    }
}