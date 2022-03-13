using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;
using WilliamPersonalMultiTool.Acting;

namespace WilliamPersonalMultiTool.Custom
{
    public class CustomPhraseManager : HotPhraseManagerForWinForms
    {
        public Form Parent { get; }

        public bool InsideQuotedEntry { get; set; }
        public string CurrentEntry { get; set; }

        public CustomPhraseManager(Form parent, string textOfSequencesToAdd = null)
        {
            Parent = parent;
            if (textOfSequencesToAdd.IsNotEmpty())
                AddSet(textOfSequencesToAdd);
        }

        public void OnExpandToNameOfTrigger(object sender, PhraseEventArguments e)
        {
            var customKeySequence = (CustomKeySequence)e.State.KeySequence;
            SendBackspaces(customKeySequence.BackspaceCount);
            var textToSend = e.Name;
            if (e.State.KeySequence.WildcardMatchType != WildcardMatchType.None && e.State.MatchResult.Value.IsNotEmpty())
            {
                var templateToFind = WildcardTemplate(e.State.KeySequence);
                textToSend = textToSend.Replace(templateToFind, e.State.MatchResult.Value);
            }

            textToSend = textToSend
                    .Replace(@"\r", "{RETURN}")
                    .Replace(@"\n", "{RETURN}")
                    .Replace(@"\t", "{TAB}")
                    .Replace(@"\*", @"*")
                    .Replace(@"\\", @"\")
                ;

            InlineExpansionSendKeysAndWait(textToSend);
        }

        private string WildcardTemplate(KeySequence stateKeySequence)
        {
            return "~~~~"; // new string('*', stateKeySequence.WildcardCount);
        }

        public void AddOrReplace(string name, int backspaceCount, params PKey[] keys)
        {
            var customKeySequence = new CustomKeySequence(name, keys.ToList(), OnExpandToNameOfTrigger, backspaceCount);
            Keyboard.AddOrReplace(customKeySequence);
        }

        public void AddFromFile(string path)
        {
            InsideQuotedEntry = false;
            if (!File.Exists(path))
                return;

            AddSet(File.ReadAllText(path));
        }

        public CustomKeySequence Add(CustomKeySequence keySequence)
        {
            Keyboard.KeySequences.ReplaceMatching(keySequence);
            return keySequence;
        }

        public CustomKeySequence AddOrReplace(string keys)
        {
            var pKeyList = keys.ToPKeyList(null, out var wildcardMatchType, out var wildcardCount);
            var keySequence = new CustomKeySequence(keys, pKeyList, OnExpandToNameOfTrigger);

            Keyboard.KeySequences.ReplaceMatching(keySequence);

            if (wildcardCount <= 0) return keySequence;

            keySequence.WildcardMatchType = wildcardMatchType;
            keySequence.WildcardCount = wildcardCount;
            return keySequence;
        }

        public List<CustomKeySequence> AddSet(string text)
        {
            text = text.Replace("\r", "");
            while (text.StartsWith("\n")) text = text.Substring(1);
            while (text.EndsWith("\n")) text = text.Substring(0, text.Length - 1);

            var linesWithNoComments = text
                .Replace("\r", "")
                .LineList()
                .Where(line => !line.Trim().StartsWith("//"))
                .ToList();

            Actors ??= new List<BaseActor>();
            BaseActor previousActor = null;

            foreach (var line in linesWithNoComments)
            {
                var actor = ActorHelper.Factory(line, this, previousActor);
                if (actor == null || actor.ActionableType == ActionableType.Unknown)
                    throw new Exception($"Invalid Line: {line}");

                if (actor.ActionableType == ActionableType.Or)
                {
                    if(previousActor is {CanContinue: true})
                    {
                        
                        if (previousActor.OnContinue(line))
                            actor = null;
                    }
                    else
                    {
                        // Error condition
                    }
                }
                else if(previousActor == null || actor.ID != previousActor.ID)
                {
                    var existingActorWithSameKeySequence =
                        Actors.FirstOrDefault(a => a.KeySequence.Sequence.AreEqual(actor.KeySequence.Sequence));
                    if (existingActorWithSameKeySequence != null)
                        Actors.Remove(existingActorWithSameKeySequence);
                    Actors.Add(actor);
                }

                previousActor = actor?.CanContinue == true 
                    ? actor 
                    : null;
            }

            List<CustomKeySequence> keySequencesToAdd = Actors.Select(a => a.KeySequence).ToList();

            Keyboard.KeySequences ??= new KeySequenceList();
            Keyboard.KeySequences.AddRange(keySequencesToAdd);

            return keySequencesToAdd;
        }

        public List<BaseActor> Actors { get; set; }

        public void InlineExpansionSendKeysAndWait(string toSend)
        {
            var list = new InlineExpansion(this, toSend);
            list.Play();
        }

        public void NormalSendKeysAndWait(string toSend, int backspaceCount = 0)
        {
            try
            {
                if (backspaceCount > 0)
                    SendBackspaces(backspaceCount);

                foreach (var c in toSend)
                {
                    SendKeys.SendWait(c.ToString());
                    Thread.Sleep(5);
                }
                
            }
            catch
            {
                // Ignored
            }
        }

        public static KeySequence Factory(string name = null, string keys = null)
        {
            var keySequence = new KeySequence()
            {
                Name = name ?? Guid.NewGuid().ToString(),
            };
            if (keys.IsEmpty()) return keySequence;

            keySequence.Sequence = keys.ToPKeyList(null, out var wildcardMatchType, out var wildcardCount);
            keySequence.WildcardCount = wildcardCount;
            keySequence.WildcardMatchType = wildcardMatchType;
            return keySequence;
        }

        public override void SendString(string textToSend, int millisecondsBetweenKeys, bool sendAsIs)
        {
            if(textToSend.IsEmpty())
                return;

            if (sendAsIs && millisecondsBetweenKeys > 0)
            {
                foreach (var c in textToSend)
                {
                    if (c == '{')     SendKeys.SendWait("{{}");
                    else if(c == '}') SendKeys.SendWait("{}}");
                    else if(c == '(') SendKeys.SendWait("{(}");
                    else if(c == ')') SendKeys.SendWait("{)}");
                    else if(c == '+') SendKeys.SendWait("{+}");
                    else if(c == '^') SendKeys.SendWait("{^}");
                    else if(c == '%') SendKeys.SendWait("{%}");
                    else if(c == '~') SendKeys.SendWait("{~}");
                    else              SendKeys.SendWait(c.ToString());
                    Thread.Sleep(millisecondsBetweenKeys);
                }
                return;
            }

            SendKeys.SendWait(textToSend);
            if(millisecondsBetweenKeys > 0)
                Thread.Sleep(millisecondsBetweenKeys);
        }
    }
}