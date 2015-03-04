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
        private static string m_IndependentTemplate;

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

        public static string DependentTemplate
        {
            get
            {
                if (m_Template != null)
                {
                    return m_Template;
                }

                Assembly assembly = Assembly.GetAssembly(typeof(BaseLineProcessor));
                using (Stream stream = assembly.GetManifestResourceStream("MetX.Library.QuickScriptProcessor.cs"))
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

        public static string IndependentTemplate
        {
            get
            {
                if (m_IndependentTemplate != null)
                {
                    return m_IndependentTemplate;
                }

                Assembly assembly = Assembly.GetAssembly(typeof(BaseLineProcessor));
                using (Stream stream = assembly.GetManifestResourceStream("MetX.Library.IndependentQuickScriptProcessor.cs"))
                {
                    if (stream == null)
                    {
                        throw new MissingManifestResourceException("Independent Quick script processor resource missing.");
                    }
                    m_IndependentTemplate = new StreamReader(stream).ReadToEnd();
                }
                return m_IndependentTemplate;
            }
        }

        public static CompilerResults CompileSource(string source, bool independent)
        {
            CompilerParameters compilerParameters = new CompilerParameters
            {
                GenerateExecutable = independent,
                GenerateInMemory = !independent,
                IncludeDebugInformation = independent
            };
            if (independent)
            {
                compilerParameters.MainClass = "Processor.Program";
                compilerParameters.OutputAssembly =
                    Guid.NewGuid().ToString().Replace(new [] {"{","}","-"}, "").ToLower() + ".exe";
            }

            compilerParameters
                .ReferencedAssemblies
                .AddRange(
                    AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !a.IsDynamic)
                        .Select(a => a.Location)
                        .ToArray());
            CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters,
                source);
            return compilerResults;
        }

        public string ToCSharp(bool independent)
        {
            return new GenInstance(this, independent).CSharp;
        }

        public bool Load(string rawScript)
        {
            bool ret = false;
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

                StringBuilder sb = new StringBuilder();
                foreach (string line in rawScript.Lines())
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

        public static string ExpandScriptLineToSourceCode(string currScriptLine, int indent)
        {
            // backslash percent will translate to % after parsing
            currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

            bool keepGoing = true;
            int iterations = 0;
            while (keepGoing && ++iterations < 1000 && currScriptLine.Contains("%")
                   && currScriptLine.TokenCount("%") > 1)
            {
                string variableContent = currScriptLine.TokenAt(2, "%");
                string resolvedContent = String.Empty;
                if (variableContent.Length > 0)
                {
                    if (variableContent.Contains(" "))
                    {
                        string variableName = variableContent.FirstToken();
                        string variableIndex = variableContent.TokensAfterFirst();
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
    }

    public class GenArea
    {
        public readonly List<string> Lines = new List<string>();
        public readonly string Name;
        public int Indent = 12;

        public GenArea(string name, int indent, string lines = null)
        {
            Name = name;
            Indent = indent;
            if (!string.IsNullOrEmpty(lines)) 
                Lines = lines.LineList();
        }

        public GenArea(string name)
        {
            Name = name;
            Indent = 12;
        }
    
    }

    public class GenInstance : List<GenArea>
    {
        private readonly XlgQuickScript m_Quick;
        private readonly bool m_Independent;
        private GenArea m_CurrArea;

        public GenInstance(XlgQuickScript quick, bool independent)
        {
            m_Quick = quick;
            m_Independent = independent;
            
            m_CurrArea = new GenArea("ProcessLine");
            AddRange(new[]
                {
                    m_CurrArea,
                    new GenArea("ClassMembers", 8),
                    new GenArea("Start"),
                    new GenArea("Finish"),
                });
        }

        public string CSharp
        {
            get
            {
                foreach (string currScriptLine in m_Quick.Script.Lines())
                {
                    int indent = currScriptLine.Length - currScriptLine.Trim().Length;
                    if (currScriptLine.Contains("~~Start:") || currScriptLine.Contains("~~Begin:"))
                        SetArea("Start");
                    else if (currScriptLine.Contains("~~Finish:") || currScriptLine.Contains("~~Final:") || currScriptLine.Contains("~~End:"))
                        SetArea("Finish");
                    else if (currScriptLine.Contains("~~ClassMember:") || currScriptLine.Contains("~~ClassMembers:") || currScriptLine.Contains("~~Fields:") || currScriptLine.Contains("~~Field:") || currScriptLine.Contains("~~Members:") || currScriptLine.Contains("~~Member:"))
                        SetArea("ClassMembers");
                    else if (currScriptLine.Contains("~~ProcessLine:") || currScriptLine.Contains("~~ProcessLines:") || currScriptLine.Contains("~~Body:"))
                        SetArea("ProcessLine");
                    else if (currScriptLine.Contains("~~:"))
                        m_CurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, indent));
                    else
                        m_CurrArea.Lines.Add((new string(' ', indent + m_CurrArea.Indent)) + currScriptLine);
                }

                StringBuilder sb = new StringBuilder(m_Independent ? XlgQuickScript.IndependentTemplate : XlgQuickScript.DependentTemplate);
                foreach (GenArea area in this)
                    sb.Replace("//~~" + area.Name + "~~//", String.Join(Environment.NewLine, area.Lines));

                if (m_Independent)
                {
                    sb.Replace("//~~InputFilePath~~//", "\"" + m_Quick.InputFilePath.LastToken(@"\") + "\"");
                    sb.Replace("//~~Namespace~~//", (m_Quick.Name + "_" + DateTime.UtcNow.ToString("G") + "z").AsFilename());
                    sb.Replace("//~~NameInstance~~//", m_Quick.Name + " at " + DateTime.Now.ToString("G"));

                    switch (m_Quick.Destination)
                    {
                        case QuickScriptDestination.TextBox:
                        case QuickScriptDestination.Clipboard:
                        case QuickScriptDestination.Notepad:
                            sb.Replace("//~~DestinationFilePath~~//", "Path.GetTempFileName()");
                            break;

                        case QuickScriptDestination.File:
                            sb.Replace("//~~DestinationFilePath~~//", "\"" + m_Quick.DestinationFilePath.LastToken(@"\") + "\"");
                            break;
                    }
                }

                for (int i = 0; i < 10; i++)
                    sb.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                return sb.ToString();
            }
        }

        private void SetArea(string areaName)
        {
            foreach (GenArea area in this.Where(area => area.Name == areaName))
            {
                m_CurrArea = area;
                return;
            }
        }
    }
}