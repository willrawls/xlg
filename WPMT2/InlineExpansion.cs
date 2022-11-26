using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using WilliamPersonalMultiTool.Custom;
using MetX.Standard.Library.Encryption;
using MetX.Standard.Strings;

namespace WilliamPersonalMultiTool
{
    public class InlineExpansion : System.Collections.Generic.List<InlinePiece> 
    {
        public CustomPhraseManager Manager { get; }

        public InlineExpansion(CustomPhraseManager manager, string target)
        {
            Clear();
            Manager = manager;
            target = target.Replace("}{", "}~~~~~{");
            var pieces = target.Splice("{", "}").ToArray();
            var isOdd = true;
            foreach (var piece in pieces)
            {
                if (isOdd)
                {
                    isOdd = false;
                    if(piece != "~~~~~")
                        Add(new InlinePiece(piece, false));
                }
                else
                {
                    var isCommand = piece.Contains(" ") || piece.ToLower().Contains("clipboard");
                    var inlinePiece = new InlinePiece(piece, isCommand);
                    Add(inlinePiece);
                    if(!isCommand)
                    {
                        var entry = SendKeyHelper.Entries.FirstOrDefault(x => string.Equals(x.Name, piece, StringComparison.InvariantCultureIgnoreCase));
                        if (entry != null)
                        {
                            inlinePiece.Command = "pkey";
                        }
                    }
                    isOdd = true;
                }
            }
        }

        public void Play()
        {
            var delay = 1;
            foreach (InlinePiece piece in this)
            {
                if (piece.Command.IsEmpty())
                {
                    Manager.SendString(piece.Contents, delay, true);
                }
                else
                {
                    switch (piece.Command)
                    {
                        case "pause":
                            var milliseconds = piece.Arguments.AsInteger(0);
                            if(milliseconds > 0)
                                Thread.Sleep(milliseconds);
                            break;

                        case "clipboard":
                            var textToType = Clipboard.GetText();
                            if (textToType.IsEmpty())
                                break;

                            Manager.SendString(textToType, delay, true);
                            break;

                        case "guid":
                            var format = piece.Arguments.IsEmpty() ? "N" : piece.Arguments;
                            var guid = Guid.NewGuid().ToString(format);
                            Manager.SendString(guid, delay, true);
                            break;

                        case "roll":
                            var count = int.Parse(piece.Arguments.FirstToken());
                            var sides = int.Parse(piece.Arguments.TokenAt(2));
                            var textToSend = SuperRandom.NextRoll(count, sides).ToString();
                            Manager.SendString(textToSend, delay, true);
                            break;

                        case "pkey":
                            Manager.SendString($"{{{piece.Contents}}}", delay, false);
                            break;

                        case "speed":
                        case "delay":
                            delay = piece.Arguments.AsInteger(5);
                            if (delay < 0)
                                delay = 0;
                            break;
                    }
                }
            }
        }
    }
}