using System;
using MetX.Standard.Strings;
using NHotPhrase.Phrase;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class SizeActor : BaseActor
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Verb To { get; set; }
        public Verb Percent { get; set; }
        public Verb Absolute { get; set; }
        public Verb Relative { get; set; }

        public SizeActor()
        {
            ActionableType = ActionableType.Size;

            To = AddLegalVerb("to");
            Percent = AddLegalVerb("percent", To);
            Relative = AddLegalVerb("relative");
            Absolute = AddLegalVerb("absolute", Relative);
            Tall = AddLegalVerb("tall");

            OnAct = Act;
            DefaultVerb = To;
            CanContinue = false;
        }

        public Verb Tall { get; set; }

        public override bool Initialize(string item)
        {
            if (!base.Initialize(item))
                return false;

            var tokens = Arguments.Replace(",", "").AllTokens(compare: StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Count == 2)
            {
                Width = tokens[0].AsInteger();
                Height = tokens[1].AsInteger();
            }
            else if (tokens.Count == 1)
            {
                if (ExtractedVerbs.Contains(Tall)) Height = tokens[0].AsInteger();
                else Width = tokens[0].AsInteger();
            }

            return true;
        }

        public bool Act(PhraseEventArguments phraseEventArguments)
        {
            return true;
        }
    }
}