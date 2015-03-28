using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetX.Library;

namespace MetX.Data
{
    public class GenInstance : List<GenArea>
    {
        private readonly bool m_Independent;
        private readonly XlgQuickScript m_Quick;
        private GenArea m_CurrArea;
        public XlgQuickScriptTemplate Template;

        public GenInstance(XlgQuickScript quick, XlgQuickScriptTemplate template, bool independent)
        {
            m_Quick = quick;
            m_Independent = independent;
            Template = template;

            m_CurrArea = new GenArea("ProcessLine");
            AddRange(new[]
            {
                m_CurrArea,
                new GenArea("ClassMembers", 8),
                new GenArea("Start"),
                new GenArea("Finish"),
                new GenArea("ReadInput")
            });
        }

        public string CSharp
        {
            get
            {
                var originalArea = string.Empty;
                foreach (var currScriptLine in m_Quick.Script.Lines())
                {
                    var indent = currScriptLine.Length - currScriptLine.Trim().Length;
                    if (currScriptLine.Contains("~~Start:") || currScriptLine.Contains("~~Begin:"))
                    {
                        SetArea("Start");
                    }
                    else if (currScriptLine.Contains("~~Finish:") || currScriptLine.Contains("~~Final:") || currScriptLine.Contains("~~End:"))
                    {
                        SetArea("Finish");
                    }
                    else if (currScriptLine.Contains("~~ReadInput:") || currScriptLine.Contains("~~Read:") || currScriptLine.Contains("~~Input:"))
                    {
                        SetArea("ReadInput");
                    }
                    else if (currScriptLine.Contains("~~ClassMember:") || currScriptLine.Contains("~~ClassMembers:") || currScriptLine.Contains("~~Fields:") || currScriptLine.Contains("~~Field:")
                             || currScriptLine.Contains("~~Members:") || currScriptLine.Contains("~~Member:"))
                    {
                        SetArea("ClassMembers");
                    }
                    else if (currScriptLine.Contains("~~ProcessLine:") || currScriptLine.Contains("~~ProcessLines:") || currScriptLine.Contains("~~Body:"))
                    {
                        SetArea("ProcessLine");
                    }
                    else if (currScriptLine.Contains("~~BeginString:"))
                    {
                        var stringName = currScriptLine.TokenAt(2, "~~BeginString:").Trim();
                        if (string.IsNullOrEmpty(stringName))
                        {
                            continue;
                        }
                        m_CurrArea.Lines.Add("string " + stringName + " = //~~String " + stringName + "~~//;");
                        originalArea = m_CurrArea.Name;
                        SetArea("String " + stringName);
                    }
                    else if (currScriptLine.Contains("~~EndString:"))
                    {
                        if (m_CurrArea.Lines.IsNullOrEmpty())
                        {
                            m_CurrArea.Lines.Add("string.Empty");
                        }
                        else if (!m_CurrArea.Lines.Any(x => x.Contains("\"")))
                        {
                            m_CurrArea.Lines[0] = "@\"" + m_CurrArea.Lines[0];
                            m_CurrArea.Lines[m_CurrArea.Lines.Count - 1] += "\"";
                        }
                        else if (!m_CurrArea.Lines.Any(x => x.Contains("`")))
                        {
                            m_CurrArea.Lines.TransformAllNotEmpty((line, index) => line.Replace("\"", "``"));
                            m_CurrArea.Lines[0] = "@\"" + m_CurrArea.Lines[0];
                            m_CurrArea.Lines[m_CurrArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
                        }
                        else if (!m_CurrArea.Lines.Any(x => x.Contains("`")))
                        {
                            m_CurrArea.Lines.TransformAllNotEmpty((line, index) => "\t\"" + line.Replace("\"", "\\\"") + "\" + Enviornment.NewLine;" + Environment.NewLine);
                            m_CurrArea.Lines[0] = "@\"" + m_CurrArea.Lines[0];
                            m_CurrArea.Lines[m_CurrArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
                        }
                        SetArea(originalArea);
                        originalArea = null;
                    }
                    else if (currScriptLine.Contains("~~:"))
                    {
                        if (m_CurrArea.Name == "ClassMembers")
                        {
                            m_CurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, -1));
                        }
                        else
                        {
                            m_CurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, indent));
                        }
                    }
                    else if (originalArea != null)
                    {
                        m_CurrArea.Lines.Add(currScriptLine);
                    }
                    else
                    {
                        m_CurrArea.Lines.Add((new string(' ', indent + m_CurrArea.Indent)) + currScriptLine);
                    }
                }

                var sb = new StringBuilder(Template.Views.View(m_Independent ? "Exe" : "Native"));
                foreach (var area in this)
                {
                    sb.Replace("//~~" + area.Name + "~~//", String.Join(Environment.NewLine, area.Lines));
                }

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

                for (var i = 0; i < 10; i++)
                {
                    sb.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                }
                return sb.ToString();
            }
        }

        private void SetArea(string areaName)
        {
            foreach (var area in this.Where(area => area.Name == areaName))
            {
                m_CurrArea = area;
                return;
            }
            m_CurrArea = new GenArea(areaName);
            Add(m_CurrArea);
        }
    }
}