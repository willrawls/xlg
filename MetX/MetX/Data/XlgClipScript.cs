using System;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;

namespace MetX.Data
{
    /// <summary>
    /// Represents a clipboard processing script
    /// </summary>
    [Serializable, XmlType(Namespace = "", AnonymousType = true)]
    public class XlgClipScript
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Script;
        [XmlAttribute]
        public ClipScriptDestination Destination;

        public void Update(string name, string destination, string script)
        {
            Name = name;
            Script = script;
            Enum.TryParse(destination.Replace(" ", string.Empty), out Destination);
        }
    }
}