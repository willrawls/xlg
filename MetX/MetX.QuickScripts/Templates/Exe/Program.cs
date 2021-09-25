using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;

using MetX.Standard;
using MetX.Standard.IO;
using MetX.Standard.Data;
using MetX.Standard.Scripts;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Pipelines;
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
                if (args.Length > 2 && args[2].ToLower() == "open") 
                    processor.OpenNotepad = true;

                if (!processor.ReadInput()) return;
                if (!processor.Start()) return;
                if (!ProcessLines(processor)) return;
                if (!processor.Finish()) return;

                if (processor.Output == null || processor.Output.Length == 0) return;

                if (processor.DestinationFilePath == "clipboard")
                {
                    var clipboard = new ConsoleClipboard();
                    clipboard.SetText(processor.Output.ToString());
                }
                else if(processor.DestinationFilePath == "console")
                {
                    Console.WriteLine(processor.OutputStringBuilder);
                }
                else
                {
                    FileSystem.SafeDelete(processor.DestinationFilePath);
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

        public static bool ProcessLines(QuickScriptProcessor processor)
        {
            for(int number = 0; number < processor.Lines.Count; number++)
            {
                if (processor.DestinationFilePath != "console" && number > 0 && number % 100 == 0)
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
    }
}
