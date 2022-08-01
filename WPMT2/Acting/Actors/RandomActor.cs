using System;
using MetX.Standard.Library.Encryption;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using NHotPhrase.Phrase;

namespace WilliamPersonalMultiTool.Acting.Actors
{
    public class RandomActor : BaseActor
    {
        public int Count { get; set; }
        public int Sides { get; set; }
        public string Before { get; set; } = "";
        public string After { get; set; } = "";

        public Verb GuidVerb { get; set; }
        public Verb Hex { get; set; }
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
            Hex = AddLegalVerb("hex");
            GuidVerb = AddLegalVerb("guid");
            
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

            if (Letters.Mentioned || Digits.Mentioned)
            {
                textToSend = SuperRandom.NextString(Count, Letters.Mentioned, Digits.Mentioned, false, false);
            }
            else if (Dice.Mentioned)
            {
                textToSend = SuperRandom.NextRoll(Count, Sides).ToString();
            }
            else if (Hex.Mentioned)
            {
                textToSend = SuperRandom.NextHexString(Count);
            }
            else if (GuidVerb.Mentioned)
            {
                var format = "N";
                if (Arguments.IsNotEmpty() && Arguments is "N" or "D" or "B" or "P")
                    format = Arguments;

                textToSend = SuperRandom.NextGuid().ToString(format);
            }
            else if (ExtractedVerbs.Contains(Number))
            {
                textToSend = Math.Abs(SuperRandom.NextLong(0, Count)).ToString();
            }
            else
            {
                return true;
            }

            Manager.SendBackspaces(KeySequence.BackspaceCount);

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