using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Firstscript;
using MetX.Standard;
using MetX.Standard.IO;
using MetX.Standard.Data;
using MetX.Standard.Scripts;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using Microsoft.CSharp;



namespace Firstscript
{
    public static class Program
    {
        [STAThread()] // Needed to access clipboard
        public static void Main(string[] args)
        {
            try
            {
                QuickScriptProcessor processor = new QuickScriptProcessor();

                processor.InputFilePath = @"clipboard";
                processor.DestinationFilePath = @"console";

                if (args.Length > 0) processor.InputFilePath = args[0];
                if (args.Length > 1) processor.DestinationFilePath = args[1];
                if (args.Length > 2 && args[2].ToLower() == "open") 
                    processor.OpenNotepad = true;

                if (!processor.ReadInput()) return;
                if (!processor.Start()) return;

                for(int number = 0; number < processor.Lines.Count; number++)
                {
                    if (number > 0 && number % 100 == 0) Console.Write(".");
                    string currLine = processor.Lines[number];
                    try
                    {
                        if(!processor.ProcessLine(currLine, number)) return;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(
                            $@"Error processing line {(number + 1)}:
-----[{currLine}]-----

CONTINUE PROCESSING ?");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        if (answer.Key.ToString().ToLower() == "n") return;
                    }
                }

                if (!processor.Finish()) return;

                if (processor.Output == null || processor.Output.Length == 0) return;

                if(processor.DestinationFilePath == "console")
                    Console.WriteLine(processor.OutputStringBuilder);
                else
                {
                    if (string.IsNullOrEmpty(processor.DestinationFilePath))
                        processor.DestinationFilePath = "Output.txt";
                    File.WriteAllText(processor.DestinationFilePath, processor.Output.ToString());
                    if(processor.OpenNotepad)
                        System.Diagnostics.Process.Start("notepad", processor.DestinationFilePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
