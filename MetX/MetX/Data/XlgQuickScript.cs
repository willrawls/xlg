using System;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Data
{
    /// <summary>
    /// Represents a clipboard processing script
    /// </summary>
    [Serializable, XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScript
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Script;
        [XmlAttribute]
        public ClipScriptDestination Destination;

        public XlgQuickScript() { Script = string.Empty; Id = Guid.NewGuid(); }
        [XmlAttribute]
        public Guid Id;

        public string Input;

        public void Update(string name, string script, string destination)
        {
            Name = name;
            Script = script;
            Enum.TryParse(destination.Replace(" ", string.Empty), out Destination);
        }

        public string ToString(bool isDefault)
        {
            return "~~QuickScriptName:" + Name + Environment.NewLine +
                   "~~QuickScriptDestination:" + Destination + Environment.NewLine +
                   "~~QuickScriptId:" + Id + Environment.NewLine +
                   (isDefault ? "~~QuickScriptDefault:" + Environment.NewLine : string.Empty) +
                   Script;
        }

        public static void LoadLineFromFile(XlgQuickScriptFile ret, ref XlgQuickScript currScript, string line)
        {
            if (line.StartsWith("~~QuickScriptName:"))
            {
                if (!string.IsNullOrEmpty(currScript.Name))
                {
                    ret.Add(currScript);
                    currScript = new XlgQuickScript();
                }
                currScript.Name = line.TokensAfterFirst(":");
                if (string.IsNullOrEmpty(currScript.Name))
                {
                    currScript.Name = "Unnamed " + Guid.NewGuid();
                }
            }
            else if (line.StartsWith("~~QuickScriptDefault:"))
            {
                ret.Default = currScript;
            }
            else if (line.StartsWith("~~QuickScriptDestination:"))
            {
                if (!Enum.TryParse(line.TokensAfterFirst(":"), out currScript.Destination))
                    currScript.Destination = ClipScriptDestination.TextBox;
            }
            else if (line.StartsWith("~~QuickScriptInput:"))
            {
                currScript.Input = line.TokensAfterFirst(":");
            }
            else if (line.StartsWith("~~QuickScriptId:"))
            {
                if (!Guid.TryParse(line.TokensAfterFirst(":"), out currScript.Id)) currScript.Id = Guid.NewGuid();
            }
            else
            {
                currScript.Script += line + Environment.NewLine;
            }
        }
    }
}