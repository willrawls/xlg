//using NArrange.Core.Configuration;

// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
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
        [XmlAttribute]
        public QuickScriptDestination Destination;

        [XmlAttribute]
        public string DestinationFilePath;

        [XmlAttribute]
        public string DiceAt;

        [XmlAttribute]
        public Guid Id;

        [XmlAttribute]
        public string Input;

        [XmlAttribute]
        public string InputFilePath;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Script;

        [XmlAttribute]
        public string SliceAt;

        [XmlAttribute]
        public string TemplateName;

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
            TemplateName = "Native";
        }

        public static string ExpandScriptLineToSourceCode(string currScriptLine, int indent)
        {
            // backslash percent will translate to % after parsing
            currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

            currScriptLine = ExpandScriptLineVariables(currScriptLine);

            currScriptLine = $"Output.AppendLine(\"{currScriptLine.Mid(3).Replace(@"\", @"\\")}\");";
            currScriptLine =
                currScriptLine.Replace("AppendLine(\" + ", "AppendLine(")
                              .Replace(" + \"\")", ")")
                              .Replace("Output.AppendLine(\"\")", "Output.AppendLine()");
            currScriptLine = (indent > 0
                ? new string(' ', indent + 12)
                : string.Empty) +
                             currScriptLine
                                 .Replace(" + \"\" + ", string.Empty)
                                 .Replace("\"\" + ", string.Empty)
                                 .Replace(" + \"\"", string.Empty)
                                 .Replace("~$~$", @"%")
                                 .Replace("~#~$", "\\\"");
            return currScriptLine;
        }

        public static string ExpandScriptLineVariables(string currScriptLine)
        {
            var keepGoing = true;
            var iterations = 0;
            while (keepGoing && ++iterations < 1000 && currScriptLine.Contains("%")
                   && currScriptLine.TokenCount("%") > 1)
            {
                var variableContent = currScriptLine.TokenAt(2, "%");
                var resolvedContent = string.Empty;
                if (variableContent.Length > 0)
                {
                    if (variableContent.Contains(" "))
                    {
                        var variableName = variableContent.FirstToken();
                        var variableIndex = variableContent.TokensAfterFirst();
                        // ReSharper disable once NotAccessedVariable
                        int actualIndex;
                        if (int.TryParse(variableIndex, out actualIndex))
                        {
                            // Array index
                            resolvedContent = "\" + (" + variableName + ".Length <= " + variableIndex
                                              + " ? string.Empty : " + variableName + "[" + variableIndex + "]) + \"";
                        }
                        else
                        {
                            resolvedContent = "\" + " + variableName + "[\"" + variableIndex + "\"] + \"";
                        }
                    }
                    else
                    {
                        resolvedContent = "\" + " + variableContent + " + \"";
                    }
                }
                else
                {
                    keepGoing = false;
                }

                currScriptLine = currScriptLine.Replace("%" + variableContent + "%", resolvedContent);
            }

            return currScriptLine;
        }

        public static string FormatCSharpCode(string code)
        {
            /*
            var logger = new NArrangeTestLogger();
            var attempts = 0;
            while (++attempts < 2)
            {
                try
                {
                    // TODO Recompile NArrange for .net 5
                    var arranger = new StringArranger(null, logger);
                    arranger.Arrange("virtual.cs", code, out var formattedCode);
                    return formattedCode ?? code;
                }
                catch (Exception ex)
                {
                    if (attempts == 2)
                    {
                        Host.MessageBox.Show(ex.ToString());
                        return code;
                    }
                }
            }
            */

            return code;
        }

        public XlgQuickScript Clone(string name)
        {
            return new(name, Script)
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
            TemplateName = "Native";

            if (string.IsNullOrEmpty(rawScript))
            {
                throw new ArgumentNullException(nameof(rawScript));
            }

            Name = rawScript.FirstToken(Environment.NewLine);
            if (string.IsNullOrEmpty(Name))
            {
                Name = "Unnamed " + Guid.NewGuid();
            }

            rawScript = rawScript.TokensAfterFirst(Environment.NewLine);
            if (!rawScript.Contains("~~QuickScript"))
            {
                Script = rawScript;
            }
            else
            {
                /*
                                if (rawScript.Contains("~~QuickScriptInput"))
                                {
                                    Input =
                                        rawScriptFromFile.TokensAfterFirst("~~QuickScriptInputStart:")
                                                 .TokensBeforeLast("~~QuickScriptInputEnd:");
                                    rawScript = rawScript.TokensAround("~~QuickScriptInputStart:",
                                        "~~QuickScriptInputEnd:" + Environment.NewLine);
                                }
                */
                var sb = new StringBuilder();
                var lines = rawScript.LineList();
//                var lines = rawScript.Lines();
                foreach (var line in lines)
                {
                    if (line.StartsWith("~~QuickScriptDefault:"))
                    {
                        ret = true;
                    }
                    else if (line.StartsWith("~~QuickScriptInput:"))
                    {
                        Input = line.TokensAfterFirst(":").Trim();
                        if (string.IsNullOrEmpty(Input))
                        {
                            Input = "Clipboard";
                        }
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
                    else if (line.StartsWith("~~QuickScriptTemplate:"))
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
                }

                Script = sb.ToString();
            }

            return ret;
        }

        public string ToCSharp(bool independent, XlgQuickScriptTemplate xlgQuickScriptTemplate)
        {
            if(xlgQuickScriptTemplate == null)
                xlgQuickScriptTemplate = ContextBase.Default.Templates[TemplateName];

            var code = new GenInstance(this, xlgQuickScriptTemplate, independent).CSharp;
            return code.IsEmpty()
                ? code
                : FormatCSharpCode(code);
        }

        public GenInstance ToGenInstance(bool independent, XlgQuickScriptTemplate xlgQuickScriptTemplate)
        {
            if(xlgQuickScriptTemplate == null)
                xlgQuickScriptTemplate = ContextBase.Default.Templates[TemplateName];

            return new GenInstance(this, xlgQuickScriptTemplate, independent);
        }

        public string ToFileFormat(bool isDefault)
        {
            return "~~QuickScriptName:" + Name.AsString() + Environment.NewLine +
                   "~~QuickScriptInput:" + Input.AsString() + Environment.NewLine +
                   "~~QuickScriptDestination:" + Destination.AsString() + Environment.NewLine +
                   "~~QuickScriptId:" + Id.AsString() + Environment.NewLine +
                   "~~QuickScriptInputFilePath:" + InputFilePath + Environment.NewLine +
                   "~~QuickScriptDestinationFilePath:" + DestinationFilePath + Environment.NewLine +
                   "~~QuickScriptSliceAt:" + SliceAt + Environment.NewLine +
                   "~~QuickScriptDiceAt:" + DiceAt + Environment.NewLine +
                   "~~QuickScriptTemplate:" + TemplateName + Environment.NewLine +
                   (isDefault
                       ? "~~QuickScriptDefault:" + Environment.NewLine
                       : string.Empty) +
                   Script.AsString();
        }

        public override string ToString()
        {
            return Name;
        }

        public string HandleSlashSlashBlock()
        {
            const string leftDelimiter = "~~{";
            const string rightDelimiter = "}~~";
            
            if (!Script.Contains(leftDelimiter) || !Script.Contains(rightDelimiter))
                return Script;

            var expanded = Script.UpdateBetweenTokens(
                leftDelimiter, rightDelimiter, true, 
                QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine);
            
            return expanded;
        }
        
        public static string QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine(string target)
        {
            if (target.IsEmpty())
                return "";

            var result = target.Replace("\r", "")
                .LineList()
                .Select(line => $"~~:{line}")
                .ToList()
                .AsString("\n");
            if (!result.EndsWith("\n"))
                result += "\n";
            return result;
        }
    }
}
