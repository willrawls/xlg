using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetX.Library;

namespace MetX.Scripts
{
    public class GenInstance : List<GenArea>
    {
        public XlgQuickScriptTemplate Template;
        private readonly bool _mIndependent;
        private readonly XlgQuickScript _mQuick;
        private GenArea _mCurrArea;

        public GenInstance(XlgQuickScript quick, XlgQuickScriptTemplate template, bool independent)
        {
            _mQuick = quick;
            _mIndependent = independent;
            Template = template;

            _mCurrArea = new GenArea("ProcessLine");
            AddRange(new[]
            {
                _mCurrArea,
                new GenArea("Usings"),
                new GenArea("ClassMembers", 8),
                new GenArea("Start"),
                new GenArea("Finish"),
                new GenArea("ReadInput"),
            });
        }

        public string CSharp
        {
            get
            {
                var originalArea = string.Empty;
                foreach (var currScriptLine in _mQuick.Script.Lines())
                {
                    var indent = currScriptLine.Length - currScriptLine.Trim().Length;

                    if (currScriptLine.Trim() == string.Empty)
                    {
                        _mCurrArea.Lines.Add(string.Empty);
                    }
                    else if (currScriptLine.Contains("~~To:"))
                    {
                        ProcessTo(currScriptLine);
                    }
                    else if (currScriptLine.Contains("~~AppendTo:")
                        || currScriptLine.Contains("~~Append To:"))
                    {
                        ProcessAppendTo(currScriptLine);
                    }
                    else if (currScriptLine.Contains("~~Start:") || currScriptLine.Contains("~~Begin:"))
                    {
                        SetArea("Start");
                    }
                    else if (currScriptLine.Contains("~~Finish:") 
                             || currScriptLine.Contains("~~Final:") 
                             || currScriptLine.Contains("~~Last:")
                             || currScriptLine.Contains("~~End:"))
                    {
                        SetArea("Finish");
                    }
                    else if (currScriptLine.Contains("~~ReadInput:") 
                             || currScriptLine.Contains("~~Read:") 
                             || currScriptLine.Contains("~~Input:"))
                    {
                        SetArea("ReadInput");
                    }
                    else if (currScriptLine.Contains("~~Using:") || currScriptLine.Contains("~~Usings:"))
                    {
                        SetArea("Usings");
                    }
                    else if (currScriptLine.Contains("~~ClassMembers:")  || currScriptLine.Contains("~~ClassMember:") 
                             || currScriptLine.Contains("~~Properties:") || currScriptLine.Contains("~~Property:")
                             || currScriptLine.Contains("~~Fields:")     || currScriptLine.Contains("~~Field:")
                             || currScriptLine.Contains("~~Members:")    || currScriptLine.Contains("~~Member:"))
                    {
                        SetArea("ClassMembers");
                    }
                    else if (currScriptLine.Contains("~~ProcessLines:") || currScriptLine.Contains("~~ProcessLine:") 
                             || currScriptLine.Contains("~~Line:")
                             || currScriptLine.Contains("~~Body:"))
                    {
                        SetArea("ProcessLine");
                    }
                    else if (currScriptLine.Contains("~~BeginString:"))
                    {
                        if (!ProcessBeginString(currScriptLine)) continue;
                        originalArea = _mCurrArea.Name;
                    }
                    else if (currScriptLine.Contains("~~EndString:"))
                    {
                        ProcessEndString();
                        SetArea(originalArea);
                        originalArea = null;
                    }
                    else if (currScriptLine.Contains("~~:"))
                    {
                        if (_mCurrArea.Name == "Using" || _mCurrArea.Name == "Usings")
                        {
                            _mCurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, -1));
                        }
                        else if (_mCurrArea.Name == "ClassMembers")
                        {
                            _mCurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, -1));
                        }
                        else
                        {
                            _mCurrArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, indent));
                        }
                    }
                    else if (originalArea != null)
                    {
                        _mCurrArea.Lines.Add(currScriptLine);
                    }
                    else
                    {
                        _mCurrArea.Lines.Add((new string(' ', indent + _mCurrArea.Indent)) + currScriptLine);
                    }
                }

                var sb = new StringBuilder(Template.Views[_mIndependent ? "Exe" : "Native"]);
                foreach (var area in this)
                {
                    sb.Replace("//~~" + area.Name + "~~//", string.Join(Environment.NewLine, area.Lines));
                }

                if (_mIndependent)
                {
                    sb.Replace("//~~InputFilePath~~//", "\"" + _mQuick.InputFilePath.LastToken(@"\") + "\"");
                    sb.Replace("//~~Namespace~~//", (_mQuick.Name + "_" + DateTime.UtcNow.ToString("G") + "z").AsFilename());
                    sb.Replace("//~~NameInstance~~//", _mQuick.Name + " at " + DateTime.Now.ToString("G"));

                    switch (_mQuick.Destination)
                    {
                        case QuickScriptDestination.TextBox:
                        case QuickScriptDestination.Clipboard:
                        case QuickScriptDestination.Notepad:
                            sb.Replace("//~~DestinationFilePath~~//", "Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Left(13) + \".txt\")");
                            break;

                        case QuickScriptDestination.File:
                            sb.Replace("//~~DestinationFilePath~~//", "\"" + _mQuick.DestinationFilePath.LastToken(@"\") + "\"");
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

        private bool ProcessBeginString(string currScriptLine)
        {
            var stringName = currScriptLine.TokenAt(2, "~~BeginString:").Trim();
            if (string.IsNullOrEmpty(stringName))
                return false;

            _mCurrArea.Lines.Add($"string {stringName} = //~~String {stringName}~~//;");
            SetArea("String " + stringName);
            return true;
        }

        private void ProcessEndString()
        {
            if (_mCurrArea.Lines.IsEmpty())
            {
                _mCurrArea.Lines.Add("string.Empty");
            }
            else if (!_mCurrArea.Lines.Any(x => x.Contains("\"")))
            {
                _mCurrArea.Lines[0] = "@\"" + _mCurrArea.Lines[0];
                _mCurrArea.Lines[_mCurrArea.Lines.Count - 1] += "\"";
            }
            else if (!_mCurrArea.Lines.Any(x => x.Contains("`")))
            {
                _mCurrArea.Lines.TransformAllNotEmpty((line, index) => line.Replace("\"", "``"));
                _mCurrArea.Lines[0] = "@\"" + _mCurrArea.Lines[0];
                _mCurrArea.Lines[_mCurrArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
            }
            else if (!_mCurrArea.Lines.Any(x => x.Contains("`")))
            {
                _mCurrArea.Lines.TransformAllNotEmpty((line, index) =>
                    "\t\"" + line.Replace("\"", "\\\"") + "\" + Enviornment.NewLine;" + Environment.NewLine);
                _mCurrArea.Lines[0] = "@\"" + _mCurrArea.Lines[0];
                _mCurrArea.Lines[_mCurrArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
            }
        }

        public void ProcessTo(string currScriptLine)
        {
            var filePath = currScriptLine
                .Replace("~~To:", string.Empty)
                .Replace("/", @"\")
                .Replace("\"", string.Empty)
                .Trim();

            var specialInstruction = filePath.Replace(" ", string.Empty).ToLower();
            if (specialInstruction.Contains("outputpath")
                || specialInstruction.Contains("destination")
                || specialInstruction == "*")
            {
                if(_mQuick.DestinationFilePath.IsNotEmpty())
                    _mCurrArea.Lines.Add($"Output.AppendTo(@\"{_mQuick.DestinationFilePath}\");");
                else
                    _mCurrArea.Lines.Add("// ~~To found nothing to do");
            }
            else
            {
                var resolvedFilePath = XlgQuickScript.ExpandScriptLineVariables(filePath);
                _mCurrArea.Lines.Add($"Output.SwitchTo(@\"{resolvedFilePath}\");");
            }
        }

        public void ProcessAppendTo(string currScriptLine)
        {
            var filePath = currScriptLine
                .Replace("~~Append To:", string.Empty)
                .Replace("~~AppendTo:", string.Empty)
                .Replace("/", @"\")
                .Replace("\"", string.Empty)
                .Trim();

            var specialInstruction = filePath.Replace(" ", string.Empty).ToLower();
            if (specialInstruction.Contains("outputpath")
                || specialInstruction.Contains("destination")
                || specialInstruction == "*")
            {
                if(_mQuick.DestinationFilePath.IsNotEmpty())
                    _mCurrArea.Lines.Add($"Output.AppendTo(@\"{_mQuick.DestinationFilePath}\");");
                else
                    _mCurrArea.Lines.Add("// ~~To found nothing to do");
            }
            else
            {
                var resolvedFilePath = XlgQuickScript.ExpandScriptLineVariables(filePath);
                _mCurrArea.Lines.Add($"Output.AppendTo(@\"{resolvedFilePath}\");");
            }
        }

        public string ProcessCommandWithPathParameters(string command, string lineFunction, string currScriptLine)
        {
            var filePath = currScriptLine
                .Replace($"~~{command}:", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("/", @"\")
                .Trim();

            var resolvedFilePath = XlgQuickScript.ExpandScriptLineVariables(filePath);

            _mCurrArea.Lines.Add($"{lineFunction}(@\"{resolvedFilePath}\");");
            return resolvedFilePath;
        }

        private void SetArea(string areaName)
        {
            foreach (var area in this.Where(area => area.Name == areaName))
            {
                _mCurrArea = area;
                return;
            }
            _mCurrArea = new GenArea(areaName);
            Add(_mCurrArea);
        }
    }
}
