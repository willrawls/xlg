using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.CSharp;
using System.Linq;

//~~Usings~~//

namespace //~~NameInstance~~//
{
/*
    Script name: //~~Script name~~//
    Script Id:   //~~Script Id~~//
    Generated:   //~~Generated At~~//
    By:          //~~UserName~~//
*/            
    public static class Program
    {
        public static bool CheckForArgument(ref string[] args, string name)
        {
            var found = args.Any(a => a.ToLower() == name);
            if (!found)
                return false;
            args = args.Where(a => a.ToLower() == name).ToArray();
            return true;
        }

        [STAThread()] // Needed to access clipboard
        public static void Main(string[] args)
        {
            try
            {
                XlgTemplateSetProcessor processor = new ();
                
                processor.InputFilePath = @"//~~InputFilePath~~//";
                processor.DestinationFilePath = @"//~~DestinationFilePath~~//";

                processor.WritingToConsole = CheckForArgument(ref args, "console");
                processor.WritingToClipboard = CheckForArgument(ref args, "clipboard");
                processor.OpenNotepad = CheckForArgument(ref args, "notepad") || CheckForArgument(ref args, "open");

                if (args.Length > 0) processor.InputFilePath = args[0];
                if (args.Length > 1) processor.DestinationFilePath = args[1];

                if (!processor.WritingToConsole)
                {
                    Console.WriteLine();
                    Console.WriteLine("-----[ //~~Script Name~~// ]-----");
                    Console.WriteLine();
                    Console.WriteLine("Input:    " + processor.InputFilePath);
                    Console.WriteLine("Output:   " + processor.DestinationFilePath);
                    if (processor.OpenNotepad)
                    {
                        if (!processor.WritingToConsole)
                            Console.WriteLine("Then:     Open in notepad");
                    }
                }

                if (!processor.WritingToConsole)
                {
                    Console.Write("Progress: ");
                    Console.Write("Read ");
                }

                if (!processor.ReadInput()) return;

                if(!processor.WritingToConsole) Console.Write("Start ");
                if (!processor.Start()) return;

                if(!processor.WritingToConsole) Console.Write("Tables ");
                if (!ProcessTables(processor)) return;

                if(!processor.WritingToConsole) Console.Write("Stored Procedures ");
                if (!ProcessStoredProcedures(processor)) return;

                if(!processor.WritingToConsole) Console.Write("Relationships ");
                if (!ProcessRelationships(processor)) return;

                if(!processor.WritingToConsole) Console.Write(" Finish ");
                if (!processor.Finish()) return;

                if (processor.Output == null || processor.Output.Length == 0) return;

                if (processor.WritingToClipboard)
                {
                    if(processor.WritingToConsole)
                        Console.Write("To clipboard ");

                    var clipboard = new ConsoleClipboard();
                    clipboard.Set(processor.OutputStringBuilder.ToString());
                }
                else if(processor.WritingToConsole)
                {
                    Console.WriteLine(processor.OutputStringBuilder);
                }
                else if (Console.IsOutputRedirected)
                {
                    Console.Out.Write(processor.OutputStringBuilder);
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
                if(!processor.WritingToConsole) 
                    Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e);
            }
        }

        public static bool ProcessLines(XlgTemplateSetProcessor processor)
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
                    if (processor.WritingToConsole)
                    {
                        Console.WriteLine("Error processing line " + (number + 1) + ":"
                                          + Environment.NewLine
                                          + currLine
                                          + Environment.NewLine
                                          + Environment.NewLine
                                          + "CONTINUE PROCESSING ?");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        if (answer.Key.ToString().ToLower() == "n") return false;
                    }
                    else return false;
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
