using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Macro
    {
        [XmlAttribute] public Guid guid;
        [XmlAttribute] public long color;
        [XmlAttribute] public bool original;
        [XmlAttribute] public string name;
        [XmlAttribute] public bool hidden;

        [XmlElement("keystroke")] public Keystroke Keystroke;
        [XmlElement("mousefunction")] public MouseFunction MouseFunction;
        [XmlElement("natural")] public Natural Natural;
        [XmlElement("textblock")] public TextBlock TextBlock;
        [XmlElement("hotkeys")] public Hotkeys Hotkeys;
        [XmlElement("multikey")] public Multikey Multikey;

        public Macro()
        {
        }

        public Macro(Guid guid, long color, bool original, string name, bool hidden)
        {
            this.guid = guid;
            this.color = color;
            this.original = original;
            this.name = name;
            this.hidden = hidden;
        }
    }
}