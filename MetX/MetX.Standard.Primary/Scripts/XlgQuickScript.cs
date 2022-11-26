//using NArrange.Core.Configuration;

// ReSharper disable InconsistentNaming

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary.Scripts
{
    //using Microsoft.CSharp;

    //using NArrange.ConsoleApplication;
    //using NArrange.Core;

    /// <summary>
    ///     Represents a clipboard processing script
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScript
    {
        private string _destinationFilePath;

        [XmlAttribute] public QuickScriptDestination Destination;

        [XmlAttribute] public string DiceAt;

        [XmlAttribute] public Guid Id;

        [XmlAttribute] public string Input;

        [XmlAttribute] public string InputFilePath;

        [XmlAttribute] public string Name;

        [XmlAttribute] public string Script;

        [XmlAttribute] public string SliceAt;

        [XmlAttribute] public string TemplateName;

        [XmlAttribute]
        public string DestinationFilePath
        {
            get => _destinationFilePath;
            set => _destinationFilePath = value;
        }

        public XlgQuickScript()
        {
        }

        public XlgQuickScript(string name, string script = null)
        {
            Name = name;
            Script = script ?? string.Empty;
            Id = Guid.NewGuid();
            Destination = QuickScriptDestination.Notepad;
            SliceAt = "End of line";
            DiceAt = "Space";
            TemplateName = "Exe";
        }

        public XlgQuickScript Clone(string name)
        {
            return new XlgQuickScript(name, Script)
            {
                Destination = Destination,
                DestinationFilePath = DestinationFilePath,
                DiceAt = DiceAt,
                Input = Input,
                InputFilePath = InputFilePath,
                SliceAt = SliceAt,
                TemplateName = TemplateName
            };
        }

        public bool Load(string rawScript)
        {
            var ret = false;
            SliceAt = "End of line";
            DiceAt = "Space";
            TemplateName = "Exe";

            if (string.IsNullOrEmpty(rawScript)) throw new ArgumentNullException(nameof(rawScript));

            Name = rawScript.FirstToken(Environment.NewLine);
            if (Name.IsEmpty())
                return true;

            rawScript = rawScript.TokensAfterFirst(Environment.NewLine);
            if (!rawScript.Contains("~~QuickScript"))
            {
                Script = rawScript;
            }
            else
            {
                var sb = new StringBuilder();
                var lines = rawScript.LineList();
//                var lines = rawScript.Lines();
                foreach (var line in lines)
                    if (line.StartsWith("~~QuickScriptDefault:"))
                    {
                        ret = true;
                    }
                    else if (line.StartsWith("~~QuickScriptInput:"))
                    {
                        Input = line.TokensAfterFirst(":").Trim();
                        if (string.IsNullOrEmpty(Input)) Input = "Clipboard";
                    }
                    else if (line.StartsWith("~~QuickScriptDestination:"))
                    {
                        Destination = QuickScriptDestination.TextBox;
                        if (!Enum.TryParse(line.TokensAfterFirst(":"), out Destination))
                            Destination = QuickScriptDestination.TextBox;
                    }
                    else if (line.StartsWith("~~QuickScriptId:"))
                    {
                        if (!Guid.TryParse(line.TokensAfterFirst(":"), out Id)) Id = Guid.NewGuid();
                    }
                    else if (line.StartsWith("~~QuickScriptSliceAt:"))
                    {
                        SliceAt = line.TokensAfterFirst(":");
                    }
                    else if (line.StartsWith("~~QuickScriptTemplate:")
                             || line.StartsWith("~~QuickScriptNativeTemplate:"))
                    {
                        // Ignore: Depricated
                    }
                    else if (line.StartsWith("~~QuickScriptExeTemplate:")
                             || line.StartsWith("~~QuickScriptTemplate:"))
                    {
                        TemplateName = line.TokensAfterFirst(":");
                    }
                    else if (line.StartsWith("~~QuickScriptDiceAt:"))
                    {
                        DiceAt = line.TokensAfterFirst(":");
                    }
                    else if (line.StartsWith("~~QuickScriptInputFilePath:"))
                    {
                        InputFilePath = line.TokensAfterFirst(":");
                    }
                    else if (line.StartsWith("~~QuickScriptDestinationFilePath:"))
                    {
                        DestinationFilePath = line.TokensAfterFirst(":");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }

                Script = sb.ToString();
            }

            return ret;
        }

        public string ToFileFormat(bool isDefault)
        {
            return
                $@"
~~QuickScriptName:{Name.AsString()}
~~QuickScriptInput:{Input.AsString()}
~~QuickScriptDestination:{Destination.AsString()}
~~QuickScriptId:{Id.AsString()}
~~QuickScriptInputFilePath:{InputFilePath}
~~QuickScriptDestinationFilePath:{DestinationFilePath}
~~QuickScriptSliceAt:{SliceAt}
~~QuickScriptDiceAt:{DiceAt}
~~QuickScriptTemplate:{TemplateName}
{(isDefault ? "~~QuickScriptDefault:" + Environment.NewLine : string.Empty)}{Script.AsString()}";
        }

        public override string ToString()
        {
            return Name;
        }

        public string HandleSlashSlashBlock()
        {
            if (!Script.Contains(Helpers.SlashSlashBlockLeftDelimiter) || !Script.Contains(Helpers.SlashSlashBlockRightDelimiter))
                return Script;

            var expanded = Script.UpdateBetweenTokens(Helpers.SlashSlashBlockLeftDelimiter, Helpers.SlashSlashBlockRightDelimiter,
                true, Helpers.QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine);

            return expanded;
        }


        public string AsParameters()
        {
            string source;
            switch (Input.ToLower())
            {
                case "console":
                case "clipboard":
                    source = $"{Input}";
                    break;
                default:
                    source = $"\"{InputFilePath}\"";
                    break;
            }

            var args2 = "";
            string destination;
            switch (Destination)
            {
                case QuickScriptDestination.Clipboard:
                    destination = "clipboard";
                    break;
                case QuickScriptDestination.Notepad:
                    destination = "\"" + Path.Combine(Environment.GetEnvironmentVariable("TEMP"),
                        $"quickScriptOutput_{Guid.NewGuid():N}.txt") + "\"";
                    args2 = " open";
                    break;
                case QuickScriptDestination.TextBox:
                    destination = "console";
                    break;
                default:
                    destination = $"\"{DestinationFilePath}\"";
                    args2 = " open";
                    break;
            }

            var parameters = $"{source} {destination}{args2}";
            return parameters;
        }
    }
}