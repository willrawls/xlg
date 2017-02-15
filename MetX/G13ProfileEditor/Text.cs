using System;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Text
    {
        [XmlAttribute] public bool hasdelay;
        [XmlAttribute] public int delay;
        [XmlAttribute] public string playback;
        [XmlText] public string Content;

        public Text(bool hasdelay, int delay, string playback, string content)
        {
            this.hasdelay = hasdelay;
            this.delay = delay;
            this.playback = playback;
            Content = content;
        }

        public Text()
        {
        }
    }
}