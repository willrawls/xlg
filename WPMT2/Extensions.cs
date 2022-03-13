using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Acting;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool
{
    public static class Extensions
    {
        public static int ToBackspaceCount(this CustomKeySequence target, List<PKey> pKeyList)
        {
            var count = pKeyList.Count(key => key is >= PKey.D0 and <= PKey.Z or >= PKey.NumPad0 and <= PKey.NumPad9);
            if (target.WildcardCount > 0)
                count += target.WildcardCount;
            return count;
        }

        public static TActor Act<TActor>(this CustomKeySequence target) where TActor : BaseActor
        {
            if (target.Actor.OnAct(target.BlankPhraseEventArguments()))
                return (TActor) target.Actor;
            return null;
        }
        public static bool Act(this CustomKeySequence target)
        {
            if (target?.Actor == null)
                return false;

            return target.Actor.OnAct(target.BlankPhraseEventArguments());
        }

        public static PhraseEventArguments BlankPhraseEventArguments(this CustomKeySequence target)
        {
            var action = new PhraseAction(target);
            var matchResult = new MatchResult(target, "");
            var state = new PhraseActionRunState(target, matchResult);
            var keysToSend = new List<PKey>();
            return new PhraseEventArguments(action, state, keysToSend);
        }

        public static int Index(this Screen target)
        {
            if (target == null)
                return 0;
            
            for (var index = 0; index < Screen.AllScreens.Length; index++)
            {
                var x = Screen.AllScreens[index];
                if (x.WorkingArea == target.WorkingArea)
                    return index;
            }

            return 0;
        }

        public static int PercentX(this Screen target, int percent = 1)
        {
            return (int) (target.Bounds.Width / 100.0) * percent;
        }

        public static int PercentY(this Screen target, int percent = 1)
        {
            return (int) (target.Bounds.Height / 100.0) * percent;
        }

        public static int WhenContains(this List<Verb> target, Verb verb, int value)
        {
            if (target.Contains(verb))
            {
                return value;
            }

            return 0;
        }

        public static bool Contains(this List<Verb> target, Verb verb)
        {
            if (target.IsEmpty() || verb == null)
                return false;

            return target
                .Any(v => string
                    .Equals(v.Name, verb.Name, StringComparison
                        .InvariantCultureIgnoreCase));
        }

        public static bool ContainsAny(this List<Verb> target, List<Verb> verbs)
        {
            if (target.IsEmpty() || verbs.IsEmpty())
                return false;

            return target
                .Any(v1 => verbs
                    .Any(v2 => string
                        .Equals(v1.Name, v2.Name, StringComparison
                            .InvariantCultureIgnoreCase)));
        }

        public static List<PKey> ToPKeyList(
            this string keys,
            List<PKey> prepend,
            out WildcardMatchType wildcardMatchType,
            out int wildcardCount)
        {
            var pKeyList = prepend.IsEmpty()
                ? new List<PKey>()
                : new List<PKey>(prepend);

            wildcardMatchType = WildcardMatchType.None;
            wildcardCount = 0;

            if (keys.IsEmpty()) return pKeyList;

            List<string> keyParts;
            if (keys.StartsWith("When")) keyParts = keys[4..].TrimStart().AllTokens();
            else if (keys.StartsWith("Or")) keyParts = keys[2..].TrimStart().AllTokens();
            else keyParts = keys.AllTokens();

            foreach (var keyPart in keyParts)
            {
                List<PKey> additionalKeys = new List<PKey>();
                var pKey = ToPKey(keyPart, out var wildcardMatchTypeInner, out var wildcardCountInner, additionalKeys);

                if (wildcardCountInner < 1)
                {
                    if (pKey != PKey.None)
                        pKeyList.Add(pKey);
                    if (additionalKeys.IsNotEmpty())
                        pKeyList.AddRange(additionalKeys);
                }
                else
                {
                    wildcardMatchType = wildcardMatchTypeInner;
                    wildcardCount = wildcardCountInner;
                }
            }
            return pKeyList;
        }

        public static void ReplaceMatching(this List<KeySequence> keySequences, CustomKeySequence sequence)
        {
            if (keySequences == null || sequence == null)
                return;

            if (keySequences?.Count == 0)
            {
                keySequences.Add(sequence);
                return;
            }

            for (var index = 0; index < keySequences.Count; index++)
            {
                var keySequence = keySequences[index];
                if (!AreEqual(keySequence.Sequence, sequence.Sequence))
                    continue;

                keySequences.RemoveAt(index);
            }

            keySequences.Add(sequence);
        }


        public static PKey ToPKey(this string singlePKeyText, out WildcardMatchType wildcardMatchType, out int wildcardCount, List<PKey> additionalKeysFound)
        {
            wildcardMatchType = WildcardMatchType.None;
            wildcardCount = 0;

            if (singlePKeyText.IsEmpty()) return PKey.None;

            if(singlePKeyText.Length == 1 && char.IsLower(singlePKeyText[0]))
                singlePKeyText = singlePKeyText.ToUpper();

            if (singlePKeyText.All(x => x == '#'))
            {
                wildcardMatchType = WildcardMatchType.Digits;
                wildcardCount = singlePKeyText.Length;
                return PKey.None;
            }

            if (singlePKeyText.All(x => x == '*'))
            {
                wildcardMatchType = WildcardMatchType.AlphaNumeric;
                wildcardCount = singlePKeyText.Length;
                return PKey.None;
            }

            switch (singlePKeyText.ToUpper())
            {
                case "0": return PKey.D0;
                case "1": return PKey.D1;
                case "2": return PKey.D2;
                case "3": return PKey.D3;
                case "4": return PKey.D4;
                case "5": return PKey.D5;
                case "6": return PKey.D6;
                case "7": return PKey.D7;
                case "8": return PKey.D8;
                case "9": return PKey.D9;
            }

            if (Enum.TryParse(typeof(PKey), singlePKeyText, true, out object pKeyObject))
            {
                if (pKeyObject != null)
                {
                    PKey pKey = (PKey)pKeyObject;
                    if (pKey.ToString() == singlePKeyText)
                        return pKey;

                    if (pKey == PKey.CapsLock
                       && (singlePKeyText.ToLower() == "capslock"
                        || singlePKeyText.ToLower() == "captial"))
                        return pKey; // Special case

                    pKey = pKey.FilterDuplicateEnumEntryNames();
                    if (pKey.ToString() == singlePKeyText)
                        return pKey;
                }
            }
            if (singlePKeyText.Length > 0)
            {
                // Treat something like "123" as "1 2 3"
                foreach (var c in singlePKeyText)
                {
                    var key = c.ToString();
                    if (char.IsDigit(c))
                        key = "D" + c;
                    if (!Enum.TryParse(typeof(PKey), key, true, out pKeyObject)) continue;
                    if (pKeyObject == null) continue;

                    var pKey = (PKey)pKeyObject;
                    if (pKey.ToString() == key) 
                        additionalKeysFound?.Add(pKey);
                    else
                    {
                        pKey = pKey.FilterDuplicateEnumEntryNames();
                        if (pKey.ToString() == key) additionalKeysFound?.Add(pKey);
                    }
                }
            }

            return PKey.None;
        }

        public static bool AreEqual(this List<PKey> expected, List<PKey> actual)
        {
            if (expected == null || actual == null)
                return false;

            if (expected.Count != actual.Count)
                return false;

            for (var i = 0; i < expected.Count; i++)
                if (actual[i] != expected[i])
                    return false;

            return true;
        }
    }
}