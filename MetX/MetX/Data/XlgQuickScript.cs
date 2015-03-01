using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml.Serialization;
using MetX.Library;
using Microsoft.CSharp;

namespace MetX.Data
{
    /// <summary>
    ///     Represents a clipboard processing script
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScript
    {
        private static string m_Template;
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
        }

        public static string Template
        {
            get
            {
                if (m_Template != null)
                {
                    return m_Template;
                }

                var assembly = Assembly.GetAssembly(typeof(BaseLineProcessor));
                using (var stream = assembly.GetManifestResourceStream("MetX.Library.QuickScriptProcessor.cs"))
                {
                    if (stream == null)
                    {
                        throw new MissingManifestResourceException("Quick script processor resource missing.");
                    }
                    m_Template = new StreamReader(stream).ReadToEnd();
                }
                return m_Template;
            }
        }

        public static CompilerResults CompileSource(string source)
        {
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };
            compilerParameters
                .ReferencedAssemblies
                .AddRange(
                    AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !a.IsDynamic)
                        .Select(a => a.Location)
                        .ToArray());
            var compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters,
                source);
            return compilerResults;
        }

        public string ToCSharp()
        {
            return new GenInstance(this).CSharp;
        }

        public bool Load(string rawScript)
        {
            var ret = false;
            SliceAt = "End of line";
            DiceAt = "Space";
            if (String.IsNullOrEmpty(rawScript))
            {
                throw new ArgumentNullException("rawScript");
            }
            Name = rawScript.FirstToken(Environment.NewLine);
            if (String.IsNullOrEmpty(Name))
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
                foreach (var line in rawScript.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    if (line.StartsWith("~~QuickScriptDefault:"))
                    {
                        ret = true;
                    }
                    else if (line.StartsWith("~~QuickScriptInput:"))
                    {
                        Input = line.TokensAfterFirst(":").Trim();
                        if (string.IsNullOrEmpty(Input))
                            Input = "Clipboard";
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

        public string ToFileFormat(bool isDefault)
        {
            return "~~QuickScriptName:" + Name.AsString() + Environment.NewLine +
                   "~~QuickScriptInput:" + Input.AsString() + Environment.NewLine +
                   "~~QuickScriptDestination:" + Destination.AsString() + Environment.NewLine +
                   "~~QuickScriptId:" + Id.AsString() + Environment.NewLine +
                   "~~QuickScriptInputFilePath:" + InputFilePath + Environment.NewLine +
                   "~~QuickScriptDestinationFilePath:" + DestinationFilePath + Environment.NewLine +
                   (isDefault
                       ? "~~QuickScriptDefault:" + Environment.NewLine
                       : String.Empty) +
                /*
                                (String.IsNullOrEmpty(Input)
                                    ? String.Empty
                                    : "~~QuickScriptInputStart:" + Environment.NewLine + Input + "~~QuickScriptInputEnd:"
                                      + Environment.NewLine) +
             */
                   Script.AsString();
        }

        public override string ToString()
        {
            return Name;
        }

        private static string ExpandScriptLineToSourceCode(string currScriptLine, int indent)
        {
            // backslash percent will translate to % after parsing
            currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

            var keepGoing = true;
            var iterations = 0;
            while (keepGoing && ++iterations < 1000 && currScriptLine.Contains("%")
                   && currScriptLine.TokenCount("%") > 1)
            {
                var variableContent = currScriptLine.TokenAt(2, "%");
                var resolvedContent = String.Empty;
                if (variableContent.Length > 0)
                {
                    if (variableContent.Contains(" "))
                    {
                        var variableName = variableContent.FirstToken();
                        var variableIndex = variableContent.TokensAfterFirst();
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
            currScriptLine = "Output.AppendLine(\"" + currScriptLine.Mid(3) + "\");";
            currScriptLine =
                currScriptLine.Replace("AppendLine(\" + ", "AppendLine(")
                    .Replace(" + \"\")", ")")
                    .Replace("Output.AppendLine(\"\")", "Output.AppendLine()");
            currScriptLine = (new string(' ', indent + 12)) +
                             currScriptLine
                                 .Replace(" + \"\" + ", String.Empty)
                                 .Replace("\"\" + ", String.Empty)
                                 .Replace(" + \"\"", String.Empty)
                                 .Replace("~$~$", @"%")
                                 .Replace("~#~$", "\\\"");
            return currScriptLine;
        }

        private enum CurrGenArea
        {
            Body,
            ClassMember,
            Start,
            Finish
        }

        private class GenArea
        {
            public readonly List<string> Lines = new List<string>();
            public readonly string Name;
            public int Indent = 12;

            public GenArea(string name)
            {
                Name = name;
            }
        }

        private class GenInstance : List<GenArea>
        {
            public readonly XlgQuickScript Parent;
            public GenArea CurrArea;

            public GenInstance(XlgQuickScript parent)
            {
                Parent = parent;
                CurrArea = new GenArea("ProcessLine");
                AddRange(new[]
                {
                    CurrArea,
                    new GenArea("ClassMembers") {Indent = 8},
                    new GenArea("Start"),
                    new GenArea("Finish")
                });
            }

            public string CSharp
            {
                get
                {
                    foreach (var currScriptLine in Parent.Script.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        var indent = currScriptLine.Length - currScriptLine.Trim().Length;
                        if (currScriptLine.Contains("~~Start:") || currScriptLine.Contains("~~Begin:"))
                            SetArea("Start");
                        else if (currScriptLine.Contains("~~Finish:") || currScriptLine.Contains("~~Final:") || currScriptLine.Contains("~~End:"))
                            SetArea("Finish");
                        else if (currScriptLine.Contains("~~ClassMember:") || currScriptLine.Contains("~~ClassMembers:"))
                            SetArea("ClassMembers");
                        else if (currScriptLine.Contains("~~ProcessLine:") || currScriptLine.Contains("~~ProcessLines:") || currScriptLine.Contains("~~Body:"))
                            SetArea("ProcessLine");
                        else if (currScriptLine.Contains("~~:"))
                            CurrArea.Lines.Add(ExpandScriptLineToSourceCode(currScriptLine, indent));
                        else
                            CurrArea.Lines.Add((new string(' ', indent + CurrArea.Indent)) + currScriptLine);
                    }

                    var sb = new StringBuilder(Template);
                    foreach (var area in this)
                        sb.Replace("//~~" + area.Name + "~~//", String.Join(Environment.NewLine, area.Lines));
                    for (var i = 0; i < 10; i++)
                        sb.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                    return sb.ToString();
                }
            }

            public void SetArea(string areaName)
            {
                foreach (var area in this.Where(area => area.Name == areaName))
                {
                    CurrArea = area;
                    return;
                }
            }
        }
    }
}