using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.CSharp;

//~~Usings~~//

namespace //~~NameInstance~~//
{
    public static class Program
    {
        [STAThread()] // Needed to access clipboard
        public static void Main(string[] args)
        {
            try
            {
                QuickScriptProcessor processor = new QuickScriptProcessor();

                processor.InputFilePath = @"//~~InputFilePath~~//";
                processor.DestinationFilePath = @"//~~DestinationFilePath~~//";

                if (args.Length > 0) processor.InputFilePath = args[0];
                if (args.Length > 1) processor.DestinationFilePath = args[1];

                bool writingToConsole = processor.DestinationFilePath.ToLower() == "console";

                if(!writingToConsole)
                {
                    Console.WriteLine();
                    Console.WriteLine("-----[ Exception Analysis 7 ]-----");
                    Console.WriteLine();
                    Console.WriteLine("Input:    " + processor.InputFilePath);
                    Console.WriteLine("Output:   " + processor.DestinationFilePath);
                }
                if (args.Length > 2 && args[2].ToLower() == "open")
                {
                    processor.OpenNotepad = true;
                    if(!writingToConsole)
                        Console.WriteLine("Then:     Open in notepad");
                }

                if(!writingToConsole) Console.Write("Progress: ");

                if(!writingToConsole) Console.Write("Read ");
                if (!processor.ReadInput()) return;

                if(!writingToConsole) Console.Write("Start ");
                if (!processor.Start()) return;

                if(!writingToConsole) Console.Write("Lines ");
                if (!ProcessLines(processor)) return;

                if(!writingToConsole) Console.Write(" Finish ");
                if (!processor.Finish()) return;

                if (processor.Output == null || processor.Output.Length == 0) return;

                if (processor.DestinationFilePath == "clipboard")
                {
                    Console.Write("To clipboard ");
                    var clipboard = new ConsoleClipboard();
                    clipboard.SetText(processor.OutputStringBuilder.ToString());
                }
                else if(writingToConsole)
                {
                    Console.WriteLine(processor.OutputStringBuilder);
                }
                else
                {
                    Console.Write("Write ");
                    SafelyDeleteFile(processor.DestinationFilePath);
                    File.WriteAllText(processor.DestinationFilePath, processor.OutputStringBuilder.ToString());
                    if(processor.OpenNotepad)
                    {
                        Console.Write("Notepad ");
                        System.Diagnostics.Process.Start("notepad", processor.DestinationFilePath);
                    }
                }
                if(!writingToConsole) 
                    Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e);
            }
        }

        public static bool ProcessLines(QuickScriptProcessor processor)
        {
            int progressInterval = 100;
            if (processor.Lines.Count > 1000000) progressInterval = 50000;
            if (processor.Lines.Count > 100000) progressInterval = 10000;
            if (processor.Lines.Count > 10000) progressInterval = 1000;

            for(int number = 0; number < processor.Lines.Count; number++)
            {
                if (processor.DestinationFilePath != "console" && number > 0 && number % progressInterval == 0)
                    Console.Write(".");

                string currLine = processor.Lines[number];
                try
                {
                    if(!processor.ProcessLine(currLine, number)) return false;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error processing line " + (number+1) + ":" 
                                      + Environment.NewLine 
                                      + currLine 
                                      + Environment.NewLine 
                                      + Environment.NewLine 
                                      + "CONTINUE PROCESSING ?");
                    ConsoleKeyInfo answer = Console.ReadKey();
                    if (answer.Key.ToString().ToLower() == "n") return false;
                }
            }

            return true;
        }

        public static bool SafelyDeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return true;

            try
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }
            catch
            {
                try
                {
                    File.Move(filePath, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".from.safedelete"));
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
