using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using MetX.Standard;
using MetX.Standard.IO;
using MetX.Standard.Data;
using MetX.Standard.Scripts;
using MetX.Standard.Library;
using MetX.Standard.Pipelines;
using MetX.Standard.Library.Extensions;
using Microsoft.CSharp;

//~~Usings~~//

namespace //~~NameInstance~~//
{
    public class QuickScriptProcessor
    {
        public readonly StringBuilder OutputStringBuilder = new StringBuilder();
        public readonly StreamBuilder Output;
        public readonly List<string> Files = new List<string>();
        public List<string> Lines = new List<string>();
        public int LineCount { get; set; }
        public string InputText;
        public string DestinationFilePath;
        public string InputFilePath;

        public bool OpenNotepad;

        //~~ClassMembers~~//

        public QuickScriptProcessor()
        {
            Output = new StreamBuilder(OutputStringBuilder);
        }
        
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
                switch (InputFilePath.ToLower())
                {
                    case "none": // This is the equivalent of reading an empty file
                        InputText = string.Empty;
                        Lines = new List<string> {string.Empty};
                        LineCount = 1;
                        return true;

                    case "clipboard":
                        InputText = Clipboard.GetText();
                        break;

                    default:
                        if (InputFilePath.StartsWith("http"))
                        {
                            InputText = MetX.Standard.IO.Http.GetUrl(InputFilePath);
                        }
                        else if (!File.Exists(InputFilePath))
                        {
                            Console.WriteLine("Input file missing: " + InputFilePath);
                            return false;
                        }
                        else
                        {
                            InputText = File.ReadAllText(InputFilePath);
                            Files.Add(InputFilePath);
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            if (string.IsNullOrEmpty(InputText))
            {
                Console.WriteLine("Input empty.");
                return false;
            }

            Lines = new List<string>(InputText
                .Replace("\r", string.Empty)
                .Split(new[] { '\n' }, StringSplitOptions
                .RemoveEmptyEntries));

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
            MetX.Standard.Pipelines.MessageBoxResult messageBoxResult = value.IsNotEmpty() 
                ? MetX.Standard.Pipelines.MessageBoxResult.OK 
                : MetX.Standard.Pipelines.MessageBoxResult.Cancel;
            return messageBoxResult;
        }
    }
}