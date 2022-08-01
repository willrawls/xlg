using System;
using System.Collections.Generic;
using System.Drawing;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Acting;

namespace WilliamPersonalMultiTool.Custom
{
    public class CustomKeySequence : KeySequence
    {
        public static Color DefaultBackColor = Color.FromArgb(14, 14, 117);
        public static Color DefaultForeColor = Color.FromArgb(227, 227, 56);
        public int BackspaceCount { get; set; }
        public string Arguments { get; set; }
        public string ExecutablePath { get; set; }
        public Color BackColor { get; set; } = DefaultBackColor;
        public Color ForeColor { get; set; } = DefaultForeColor;
        public List<CustomKeySequenceChoice> Choices { get; set; }
        public BaseActor Actor { get; set; }

        public CustomKeySequence(string name, List<PKey> keys, EventHandler<PhraseEventArguments> hotPhraseEventArgs, 
            int backspaceCount = 0, Color? backColor = null, Color? foreColor = null)
            : base(name, keys, hotPhraseEventArgs)
        {
            if (backspaceCount < 0)
                backspaceCount = 0;
            BackspaceCount = backspaceCount;
            BackColor = backColor ?? DefaultBackColor;
            ForeColor = foreColor ?? DefaultForeColor;
        }

        public CustomKeySequence(string keyText, string arguments, BaseActor actor)
        {
            Actor = actor;
            Arguments = arguments;
            Name = arguments;
            Sequence = keyText.ToPKeyList(null, out var wildcardMatchType, out var wildcardCount);
            WildcardCount = wildcardCount;
            WildcardMatchType = wildcardMatchType;
            ThenCall((sender, eventArguments) => actor.OnAct(eventArguments));
            BackspaceCount = this.ToBackspaceCount(Sequence);
        }
    }
}