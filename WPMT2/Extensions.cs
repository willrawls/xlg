using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
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