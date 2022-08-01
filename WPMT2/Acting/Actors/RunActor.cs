using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MetX.Standard.Primary.IO;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using NHotPhrase.Phrase;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class RunActor : BaseActor
    {
        public string TargetExecutable { get; set; }

        public Verb Normal { get; set; }
        public Verb Maximize { get; set; }
        public Verb Minimize { get; set; }

        public string WorkingFolder { get; set; }
        public string Filename { get; set; }

        public RunActor()
        {
            ActionableType = ActionableType.Run;

            Minimize = AddLegalVerb("minimize");
            Maximize = AddLegalVerb("maximize");
            Normal = AddLegalVerb("normal");
            OnAct = Act;
            CanContinue = false;
            DefaultVerb = Normal;
        }

        public bool Act(PhraseEventArguments phraseEventArguments)
        {
            /*
            if (KeySequence.BackspaceCount > 0)
                Manager.SendBackspaces(KeySequence.BackspaceCount);
            */

            ProcessWindowStyle windowStyle;
            if (ExtractedVerbs.Contains(Maximize)) windowStyle = ProcessWindowStyle.Maximized;
            else if (ExtractedVerbs.Contains(Minimize)) windowStyle = ProcessWindowStyle.Minimized;
            else windowStyle = ProcessWindowStyle.Normal;

            var target = TargetExecutable.IsEmpty()
                ? (Filename.IsNotEmpty() ? Filename : "")
                : TargetExecutable;
            if(File.Exists(target))
            {
                var arguments = ResolveArguments(KeySequence.Arguments);
                FileSystem.FireAndForget(target, arguments, WorkingFolder, windowStyle);
            }

            return false;
        }

        public static string LastFilePath = "";
        public static string ResolveArguments(string arguments, string delimiter = "~")
        {
            if (arguments.IsEmpty()) return "";
            if (!arguments.Contains("~")) return arguments;

            var dialog = new OpenFileDialog
            {
                RestoreDirectory = true, CheckFileExists = true, CheckPathExists = true
            };
            if (LastFilePath.IsNotEmpty()) dialog.FileName = LastFilePath;

            var before = "";
            var after = "";
            var filename = "";
            if (arguments.TokenCount("~") > 2)
            {
                // ...~filename~...
                filename = arguments.TokenAt(2, "~");
                before = arguments.FirstToken("~");
                after = arguments.TokensAfter(2, "~");
            }
            else
            {
                before = arguments.FirstToken("~");
                after = arguments.TokensAfterFirst("~");
            }
            if(filename.IsNotEmpty()) dialog.FileName = filename;
            var result = dialog.ShowDialog(Win32Window.ActiveWindow);
            if (result != DialogResult.OK) return arguments;

            filename = dialog.FileName;
            LastFilePath = filename;
            if (before.IsNotEmpty() && before != "\"") before += " ";
            if (after.IsNotEmpty() && after != "\"") after = " " + after;
            if (filename.Contains(" ")) filename = $"\"{filename}\"";

            arguments = $"{before}{filename}{after}".Trim();
            return arguments;
        }

        public override bool Initialize(string item)
        {
            if (!base.Initialize(item))
                return false;

            if(!KeySequence.Name.StartsWith("Run "))
                KeySequence.Name = "Run " + KeySequence.Name;

            var argumentWorkspace = Arguments;
            if (argumentWorkspace.Trim().Length == 0)
                return false;

            if (argumentWorkspace.StartsWith(" "))
                argumentWorkspace = argumentWorkspace.Substring(1);

            if (!argumentWorkspace.Contains("\""))
            {
                Arguments = argumentWorkspace;
                return true;
            }

            TargetExecutable = argumentWorkspace.TokenAt(2, "\"");
            argumentWorkspace = argumentWorkspace.TokensAfter(2, "\"");

            if (argumentWorkspace.StartsWith(" "))
                argumentWorkspace = argumentWorkspace.Substring(1);

            if (argumentWorkspace.Contains("\""))
            {
                Filename = argumentWorkspace.TokenAt(1, "\"");
                argumentWorkspace = argumentWorkspace.TokenAt(2, "\"");
            }

            var customKeySequence = KeySequence;
            customKeySequence.ExecutablePath = TargetExecutable;
            Arguments = argumentWorkspace;
            if (Filename.Trim().Length > 0)
            {
                Filename = Filename.Trim();
                customKeySequence.Arguments = $"\"{Filename}\" \"{Arguments}\"";
            }
            else
                customKeySequence.Arguments = $"\"{Arguments}\"";

            if(!customKeySequence.Name.StartsWith("Run "))
                customKeySequence.Name = "Run " + customKeySequence.Name;
            return true;
        }
    }
}