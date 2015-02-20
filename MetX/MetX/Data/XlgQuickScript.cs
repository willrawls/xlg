using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Data
{
    /// <summary>
    /// Represents a clipboard processing script
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScript
    {
        [XmlAttribute] public string Name;
        [XmlAttribute] public QuickScriptDestination Destination;
        [XmlAttribute] public Guid Id;
        [XmlAttribute] public string Input;
        [XmlAttribute] public string SliceAt;
        [XmlAttribute] public string DiceAt;
        [XmlAttribute] public string Script;
        
        public XlgQuickScript(string name = null, string script = "")
        {
            Name = name;
            Script = script;
            Id = Guid.NewGuid();
            Destination = QuickScriptDestination.TextBox;
            SliceAt = "End of line";
            DiceAt = "Space";
        }

        public bool Parse(string rawScript)
        {
            bool ret = false;
            SliceAt = "End of line";
            DiceAt = "Space";
            if (string.IsNullOrEmpty(rawScript)) throw new ArgumentNullException("rawScript");
            Name = rawScript.FirstToken(Environment.NewLine);
            if (string.IsNullOrEmpty(Name)) Name = "Unnamed " + Guid.NewGuid();

            rawScript = rawScript.TokensAfterFirst(Environment.NewLine);
            if (!rawScript.Contains("~~QuickScript"))
            {
                Script = rawScript;
            }
            else
            {
                if(rawScript.Contains("~~QuickScriptInput"))
                {
                    Input =
                        rawScript.TokensAfterFirst("~~QuickScriptInputStart:")
                                 .TokensBeforeLast("~~QuickScriptInputEnd:");
                    rawScript = rawScript.TokensAround("~~QuickScriptInputStart:", "~~QuickScriptInputEnd:" + Environment.NewLine);
                }

                StringBuilder sb = new StringBuilder();
                foreach (string line in rawScript.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    if (line.StartsWith("~~QuickScriptDefault:"))
                    {
                        ret = true;
                    }
                    else if (line.StartsWith("~~QuickScriptDestination:"))
                    {
                        Destination = QuickScriptDestination.TextBox;
                        if (!Enum.TryParse(line.TokensAfterFirst(":"), out Destination))
                        {
                            Destination = QuickScriptDestination.TextBox;
                        }
                    }
                    else if (line.StartsWith("~~QuickScriptId:"))
                    {
                        if (!Guid.TryParse(line.TokensAfterFirst(":"), out Id))
                        {
                            Id = Guid.NewGuid();
                        }
                    }
                    else if (line.StartsWith("~~QuickScriptSliceAt:"))
                    {
                        SliceAt = line.TokensAfterFirst(":");
                    }
                    else if (line.StartsWith("~~QuickScriptDiceAt:"))
                    {
                        DiceAt = line.TokensAfterFirst(":");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
                Script = sb.ToString();
            }
            //if (string.IsNullOrEmpty(Script)) throw new InvalidDataException("Script missing");

            return ret;
        }

        public string ToFileFormat(bool isDefault)
        {
            return "~~QuickScriptName:" + Name.AsString() + Environment.NewLine +
                   "~~QuickScriptDestination:" + Destination.AsString() + Environment.NewLine +
                   "~~QuickScriptId:" + Id.AsString() + Environment.NewLine +
                   (isDefault ? "~~QuickScriptDefault:" + Environment.NewLine : string.Empty) +
                   (string.IsNullOrEmpty(Input) ? string.Empty : 
                    "~~QuickScriptInputStart:" + Environment.NewLine + Input + "~~QuickScriptInputEnd:" + Environment.NewLine) +
                   Script.AsString();
        }

        public override string ToString() { return Name; }
    }
}