using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MetX.Library
{
    public class QuickScriptWorker
    {
        public static void ViewTextInNotepad(string source)
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, source);
                Process.Start("notepad", tempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void ViewFileInNotepad(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
                Process.Start("notepad", filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public const string FirstScript = "~~:%line.Left(20)%";

        public static readonly string ExampleTutorialScript = @"
// These lines are called for every non blank line in the source/clipboard
// Several variables are always defined including:
//   line is the string content of the current line
//   number is the current line number being processed
//   lineCount is the total number of lines to be processed.
//   d is a Dictionary<string, string> that perists across lines. Use it as you want
//   sb is a StringBuilder that you will write your output to (which goes in output/clipboard)

// Write a header
if(number == 0) 
{
  ~~:Lines starting with ~~: Are shorthand for sb.AppendLine(...) with special expansion
  ~~:This makes it easier to write when encoding lines of C#.
  ~~:Example: Line # (First word): Line content
  sb.AppendLine(""Or if you prefer, you can simply write C# code"");" + Environment.NewLine +
                                                              "  d[\"previous\"] = \"Ready \"; // sets the \"Previous\" dictionary item to \"Ready \"" + Environment.NewLine + @"
}

string[] word = line.Split(' ');
if(word.Length > 0) d[" + "\"previous\"] += word[0] + \", \";" + Environment.NewLine + @"

// The following two lines are equivalent
// sb.AppendLine(" + "\"\\\"Example\\\":\\t\\\" + number + \\\" (\\\" + word[0] + \"): \\\"\" + line + \"\\\"\");" + Environment.NewLine +
                                                              "~~:\"Example\":\\t%number% (%word 0%): \"%line%\"" + @"

if(number == lineCount - 1) 
{ 
  // After the last line
  ~~:
  ~~:This is only written at the end
  ~~:
  ~~: " + "\"Previous\" dictionary entry is written out: " + Environment.NewLine +
                                                              "  ~~:%d \"previous\"%" + Environment.NewLine + "}";
    }
}