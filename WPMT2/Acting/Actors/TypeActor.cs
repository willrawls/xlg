using System.IO;
using System.Windows.Forms;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class TypeActor : BaseActor
    {
        public static Verb Expand { get; set; }
        public static Verb TypeOutTheClipboard { get; set; }
        public static Verb TypeContentsOfFile { get; set; }
        public static Verb TypeFast { get; set; }
        public static Verb TypeSlow { get; set; }
        public static Verb TypeSlowest { get; set; }
        public static Verb TypeSlower { get; set; }

        public string Filename { get; set; }
        public string TextToType { get; set; }
        public bool FromClipboard { get; set; }
        public int DelayInMilliseconds { get; set; } = 3;

        public TypeActor()
        {
            ActionableType = ActionableType.Type;

            // Type everything else, allowing send key translation (The default)
            Expand = AddLegalVerb("expand");

            // Types any text already on the clipboard
            TypeOutTheClipboard = AddLegalVerb("clipboard");

            // Types the contents of a file
            TypeContentsOfFile = AddLegalVerb("file", TypeOutTheClipboard);

            // Causes longer and more frequent delays while typing
            TypeSlow = AddLegalVerb("slow");

            // Causes longer and more frequent delays while typing
            TypeSlower = AddLegalVerb("slower", TypeSlow);

            // Causes longer and more frequent delays while typing
            TypeSlowest = AddLegalVerb("slowest", TypeSlower);

            // Causes quick typing (Fast is the default)
            TypeFast = AddLegalVerb("fast", TypeSlowest);

            OnAct = Act;
            OnContinue = Continue;
            DefaultVerb = Expand;
            CanContinue = true;
        }

        public bool Continue(string line)
        {
            if (TextToType.Trim().Length == 0)
                TextToType = line;
            else
                TextToType += "{ENTER}" + line;
            KeySequence.Name = TextToType;
            return true;
        }

        public override bool Initialize(string item)
        {
            if (!base.Initialize(item))
                return false;
            
            if (ExtractedVerbs.Contains(TypeContentsOfFile))
                Filename = Arguments;

            DelayInMilliseconds =
                ExtractedVerbs.WhenContains(TypeSlowest, 50) +
                ExtractedVerbs.WhenContains(TypeSlower, 10) +
                ExtractedVerbs.WhenContains(TypeSlow, 3) +
                ExtractedVerbs.WhenContains(TypeFast, 1);

            KeySequence.Name = Arguments.Trim();
            TextToType = Arguments;
            return true;
        }
        
        public string ClipboardText()
        {
            var text = "";
            try
            {
                text = Clipboard.GetText();
            }
            catch
            {
                // Ignored
            }
            return text;
        }

        public bool Act(PhraseEventArguments phraseEventArguments)
        {
            if (ExtractedVerbs.Contains(TypeContentsOfFile))
            {
                TextToType = File.ReadAllText(Filename);
            }
            else if (ExtractedVerbs.Contains(TypeOutTheClipboard))
            {
                TextToType = ClipboardText();
            }
            else   // Expand
            {
                TextToType = Arguments;
            }

            var customKeySequence = KeySequence;
            Manager.SendBackspaces(customKeySequence.BackspaceCount);
            Manager.InlineExpansionSendKeysAndWait(TextToType);

            return false;
        }
    }
}