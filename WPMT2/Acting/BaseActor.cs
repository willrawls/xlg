using System;
using System.Collections.Generic;
using System.Linq;
using MetX.Standard.Library.Strings;
using MetX.Standard.XDString.Generics;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Acting
{
    public class BaseActor
    {
        public Func<string, bool> OnContinue = (item) => false;
        public Func<PhraseEventArguments, bool> OnAct = _ => false;
        public Func<string, bool> Factory { get; set; } = _ => false;

        public string KeyText { get; set; }
        public string Arguments { get; set; }
        public string Separator { get; set; }
        public string Errors { get; set; } = "";
        public bool CanContinue { get; set; }
        public Verb DefaultVerb { get; set; }

        public ActionableType ActionableType { get; set; }
        public List<Verb> ExtractedVerbs { get; set; } = new List<Verb>();
        public CustomKeySequence KeySequence { get; set; }
        public AssocArray<Verb> LegalVerbs { get; set; } = new AssocArray<Verb>();
        public Guid ID { get; set; } = Guid.NewGuid();
        public CustomPhraseManager Manager { get; set; }

        public bool Has(Verb verb)
        {
            if (verb == null)
                return false;
            return ExtractedVerbs != null && ExtractedVerbs
                .Any(v => string
                .Equals(v.Name, verb.Name, StringComparison
                    .InvariantCultureIgnoreCase));
        }

        public virtual bool Initialize(string item)
        {
            var cleanItem = item
                .Replace("\r", "")
                .FirstToken("\n")
                .Trim();
            Separator = $" {ActionableType}";

            Arguments = cleanItem.TokensAfterFirst(Separator);

            if (Arguments.StartsWith(" "))
                Arguments = Arguments.Substring(1);

            KeyText = cleanItem.FirstToken(Separator);
            if (KeyText.ToLower().StartsWith("when ")) 
                KeyText = KeyText.TokensAfterFirst("when ");
            if (KeyText.ToLower().StartsWith("or "))
                if (CanContinue)
                    KeyText = KeyText.TokensAfterFirst("or ");

            if (!GetVerbs(item))
                return false;

            KeySequence = new CustomKeySequence(KeyText, Arguments, this);

            return true;
        }

        public bool GetVerbs(string item)
        {
            var tokens = Arguments.AllTokens();
            var firstToken = tokens[0].Trim();

            var tokensToRemove = 0;
            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (LegalVerbs.ContainsKey(token))
                {
                    var verb = LegalVerbs[token].Item;
                    verb.Mentioned = true;
                    ExtractedVerbs.Add(verb);
                    tokens[i] = "";
                    tokensToRemove++;
                }
                else
                {
                    break; // First non verb stops the extraction
                }
            }

            if (tokensToRemove > 0)
                Arguments = Arguments.TokensAfter(tokensToRemove);

            // Check bound conditions here (too many, exclusivity)
            var exclusivityBreached = ExtractedVerbs.Count > 1 
                && ExtractedVerbs.Any(v1 => v1.Excludes != null
                                  && ExtractedVerbs.Any(v2 => v2.Excludes != null && v2.Excludes.Name == v1.Excludes.Name));
            if (exclusivityBreached) Errors += "GetVerbs: Exclusivity breached";

            return true;
        }

        public Verb AddLegalVerb(string name, Verb excludesVerb = null)
        {
            return LegalVerbs[name].Item = Verb.Factory(name, excludesVerb);
        }
    }
}