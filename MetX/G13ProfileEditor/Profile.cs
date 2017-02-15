using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.G13ProfileEditor
{
    [Serializable]
    public class Profile
    {
        [XmlAttribute("lock")]
        public int Lock;

        [XmlAttribute]
        public string gameid;

        [XmlAttribute]
        public int gkeysdk;

        [XmlAttribute]
        public int gpasupported;

        [XmlAttribute]
        public Guid guid;

        [XmlAttribute]
        public DateTime lastpayeddate;

        public int launchable;

        [XmlAttribute]
        public string uriHostNameType;

        [XmlElement]
        public string description;

        [XmlElement("target")]
        public List<Target> targets;

        [XmlElement("signature")] public Signature signature;

        [XmlArray("macros")]
        [XmlArrayItem("macro")]
        public List<Macro> Macros;

        [XmlArray("assignments")]
        public List<Assignments> Assignments;

        [XmlArray("pointers")] [XmlArrayItem("pointer")] public List<Pointer> Pointers;
        [XmlElement] public List<Backlight> Backlights;

        [XmlElement("script")] public string Script;

        public Profile(Guid guid, int @lock, int gkeysdk, DateTime lastpayeddate, string uriHostNameType, int launchable, int gpasupported, string gameid)
        {
            this.guid = guid;
            this.Lock = @lock;
            this.gkeysdk = gkeysdk;
            this.lastpayeddate = lastpayeddate;
            this.uriHostNameType = uriHostNameType;
            this.launchable = launchable;
            this.gpasupported = gpasupported;
            this.gameid = gameid;
        }

        public Profile()
        {
        }
    }
}