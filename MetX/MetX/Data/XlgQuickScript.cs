using System;
using System.CodeDom.Compiler;
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
            if (String.IsNullOrEmpty(rawScript)) throw new ArgumentNullException("rawScript");
            Name = rawScript.FirstToken(Environment.NewLine);
            if (String.IsNullOrEmpty(Name)) Name = "Unnamed " + Guid.NewGuid();

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
                   (isDefault ? "~~QuickScriptDefault:" + Environment.NewLine : String.Empty) +
                   (String.IsNullOrEmpty(Input) ? String.Empty : 
                    "~~QuickScriptInputStart:" + Environment.NewLine + Input + "~~QuickScriptInputEnd:" + Environment.NewLine) +
                   Script.AsString();
        }

        public override string ToString() { return Name; }

        public string ConvertQuickScriptToCSharp()
        {
            string[] scriptLines = Script.Split(new[]{Environment.NewLine}, StringSplitOptions.None);
            for (int i = 0; i < scriptLines.Length; i++)
            {
                string currScriptLine = scriptLines[i];
                int indent = currScriptLine.Length - currScriptLine.Trim().Length;

                if (currScriptLine.Contains("~~:"))
                {
                    // backslash percent will translate to % after parsing
                    currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

                    bool keepGoing = true;
                    int iterations = 0;
                    while (keepGoing && ++iterations < 1000 && currScriptLine.Contains("%") && currScriptLine.TokenCount("%") > 1)
                    {
                        string variableContent = currScriptLine.TokenAt(2, "%");
                        string resolvedContent = String.Empty;
                        if (variableContent.Length > 0)
                        {
                            if (variableContent.Contains(" "))
                            {
                                string variableName = variableContent.FirstToken();
                                string variableIndex = variableContent.TokenAt(2);
                                if (variableName != "d")
                                {
                                    resolvedContent = "\" + (" + variableName + ".Length <= " + variableIndex
                                                      + " ? string.Empty : " + variableName + "[" + variableIndex + "]) + \"";
                                }
                                else
                                {
                                    resolvedContent = "\" + " + variableName + "[" + variableIndex.Replace("~#~$", "\"")
                                                      + "] + \"";
                                }
                            }
                            else
                            {
                                resolvedContent = "\" + " + variableContent + " + \"";
                            }
                        }
                        else
                            keepGoing = false;
                        currScriptLine = currScriptLine.Replace("%" + variableContent + "%", resolvedContent);
                    }
                    currScriptLine = "sb.AppendLine(\"" + currScriptLine.Mid(3) + "\");";
                    currScriptLine =
                        currScriptLine.Replace("AppendLine(\" + ", "AppendLine(")
                                      .Replace(" + \"\")", ")")
                                      .Replace("sb.AppendLine(\"\")", "sb.AppendLine()");
                    currScriptLine = (new string(' ', indent)) + "            " +
                                     currScriptLine
                                         .Replace(" + \"\" + ", String.Empty)
                                         .Replace("\"\" + ", String.Empty)
                                         .Replace(" + \"\"", String.Empty)
                                         .Replace("~$~$", @"%")
                                         .Replace("~#~$", "\\\"");
                    scriptLines[i] = currScriptLine;
                }
                else
                {
                    scriptLines[i] = (new string(' ', indent)) + "            " + currScriptLine;
                }
            }
            string source = Template.Replace("//~~ProcessLine~~//", String.Join(Environment.NewLine, scriptLines));
            return source;
        }

        private static string m_Template = null;
        public static string Template
        {
            get
            {
                if (m_Template != null) return m_Template;

                Assembly assembly = Assembly.GetAssembly(typeof(IProcessLine));
                using (Stream stream = assembly.GetManifestResourceStream("MetX.Library.QuickScriptProcessor.cs"))
                {
                    if (stream == null) throw new MissingManifestResourceException("Quick script processor resource missing.");
                    m_Template = new StreamReader(stream).ReadToEnd();
                }
                return m_Template;
            }
        }

        public static CompilerResults CompileSource(string source)
        {
            CompilerParameters compilerParameters = new CompilerParameters
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
            CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters, source);
            return compilerResults;
        }
    }
}