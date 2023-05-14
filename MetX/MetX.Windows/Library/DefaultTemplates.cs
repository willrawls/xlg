using MetX.Standard.Primary.Scripts;

namespace MetX.Windows.Library;

public static class DefaultTemplates
{
    public static void BuildDefaultUserScriptsFile(XlgQuickScriptFile xlgQuickScriptFile)
    {
        var script = new XlgQuickScript("First script", DefaultTemplates.FirstScript);
        xlgQuickScriptFile.Add(script);
        xlgQuickScriptFile.Default = script;

        script = new XlgQuickScript("Example / Tutorial", DefaultTemplates.ExampleTutorialScript);
        xlgQuickScriptFile.Add(script);

        script = new XlgQuickScript("Template for single file processing", DefaultTemplates.SingleFile);
        xlgQuickScriptFile.Add(script);

        script = new XlgQuickScript("Template for folder/ file list processing", DefaultTemplates.MultiFile);
        xlgQuickScriptFile.Add(script);

        script = new XlgQuickScript("Template for query processing", DefaultTemplates.DatabaseQuery);
        xlgQuickScriptFile.Add(script);
    }


    public const string SingleFile = @"
~~Using:


~~Members:


~~Start:


~~Line:


~~Finish:

";
    public const string MultiFile = @"
~~Using:


~~Members:


~~Start:


~~FileListStart:


~~FileStart:


~~Line:


~~FileFinish:


~~FileListFinish:


~~Finish:

";

    
    public const string DatabaseQuery = @"
~~Using:


~~Members:


~~Start:


~~QueryStart:


~~Record:


~~QueryFinish:


~~Finish:

";

    
    public const string FirstScript = @"
if(line.Length < 20)
	~~:%line%
else
	Output.AppendLine(line.Substring(0, 20));

~~Finish:
~~:This will be the last line output
";

    public const string ExampleTutorialScript = $@"
// Anything here is included under the Line section
// Each section is optional
// Each section can appear multiple times and generate from top to bottom 
//    order but in the same code block

~~Using:
// Put your framework assembly reference statements here. 
    using System.Diagnostics;

~~Members:
// Put any members to the class you want. Properties, sub classes, enums, fields, etc.
    Dictionary<string, string> d = new Dictionary<string, string>();

~~Start:
// Write a header
    ~~:Lines starting with ~~: Are shorthand for Output.AppendLine(...) with special expansion
    ~~:This makes it easier to write when encoding lines of C#.
    ~~:Example: Line # (First word): Line content

    Output.AppendLine(""Or if you prefer, you can simply write C# code"");
    d[""previous""] = ""Ready ""; // sets the ""Previous"" dictionary item to ""Ready ""

~~Line:
// This section is called for every non blank line in the source/clipboard
// Several variables are always defined including:
//   line is the string content of the current line
//   number is the current line number being processed
//   lineCount is the total number of lines to be processed.
//   d is a Dictionary<string, string> that persists across lines. Use it as you want
//   sb is a StringBuilder that you will write your output to (which goes in output/clipboard)

    string[] word = line.Split(' ');
    if(word.Length > 0) d[""previous""] += word[0] + "", "";

// The following two lines are equivalent
    Output.AppendLine(""\""Example\"":\t\"" + number + \"" (\"" + word[0] + ""): \"""" + line + ""\"""");
    ~~:""Example"":\t%number% (%word 0%): ""%line%""
// Note that you cannot say %word[0]% at this time.

~~Finish:
// After the last line of the file is processed, this section is called
    ~~:
    ~~:This is only written at the end
    ~~:
    ~~:""Previous"" dictionary entry is written out: 
        ~~:%d previous%
";
}