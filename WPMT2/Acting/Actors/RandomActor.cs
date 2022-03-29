using MetX.Standard.Library.Encryption;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using NHotPhrase.Phrase;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class RandomActor : BaseActor
    {
        public int Count { get; set; }
        public int Sides { get; set; }
        public string Before { get; set; } = "";
        public string After { get; set; } = "";

        public Verb Dice { get; set; }
        public Verb Number { get; set; }
        public Verb Digits { get; set; }
        public Verb Letters { get; set; }


        public RandomActor()
        {
            ActionableType = ActionableType.Random;

            Letters = AddLegalVerb("letters");
            Digits = AddLegalVerb("digits");
            Number = AddLegalVerb("number");
            Dice = AddLegalVerb("dice");
            
            OnAct = Act;
            DefaultVerb = Number;
            CanContinue = false;
        }

        public override bool Initialize(string item)
        {
            if (!base.Initialize(item))
                return false;

            Count = Arguments.FirstToken().AsInteger(1);
            Sides = Arguments.TokenAt(2).AsInteger(-1);

            if(Arguments.Contains("\""))
            {
                Before = Arguments.TokenAt(2, "\"");
                After = Arguments.TokenAt(4, "\"");
                Arguments = "";
            }

            if (ExtractedVerbs.Count == 0)
                Number.Mentioned = true;
            return true;
        }

        public bool Act(PhraseEventArguments phraseEventArguments)
        {
            string textToSend = null;
            if (ExtractedVerbs.Contains(Letters))
            {
                textToSend = SuperRandom.NextString(Count, true, false, false, false);
            }
            else if (ExtractedVerbs.Contains(Digits))
            {
                textToSend = SuperRandom.NextString(Count, false, true, false, false);
            }
            else if (ExtractedVerbs.Contains(Dice))
            {
                textToSend = SuperRandom.NextRoll(Count, Sides).ToString();
            }
            else if (ExtractedVerbs.Contains(Number))
            {
                textToSend = SuperRandom.NextLong(Count, Sides).ToString();
            }
            else
            {
                return true;
            }

            if(textToSend.IsNotEmpty())
            {
                if (Before.IsNotEmpty())
                    textToSend = Before + textToSend;
                if (After.IsNotEmpty())
                    textToSend += After;

                Manager.NormalSendKeysAndWait(textToSend);
            }
            return true;
        }
    }
}