using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public class GenInstance : List<GenArea>
    {
        public XlgQuickScriptTemplate Template;
        private readonly bool _mIndependent;
        private readonly XlgQuickScript _targetScript;
        private GenArea _targetGenArea;

        public GenInstance(XlgQuickScript quick, XlgQuickScriptTemplate template, bool independent)
        {
            _targetScript = quick;
            _mIndependent = independent;
            Template = template;

            _targetGenArea = new GenArea("ProcessLine");
            AddRange(new[]
            {
                _targetGenArea,
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
                var massagedScript = _targetScript.HandleSlashSlashBlock(); //    //~{ xyz }~//
                
                foreach (var currScriptLine in _targetScript.Script.Lines())
                {
                    var indent = currScriptLine.Length - currScriptLine.Trim().Length;
                    
                    if (currScriptLine.Trim() == string.Empty)
                    {
                        _targetGenArea.Lines.Add(string.Empty);
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
                        originalArea = _targetGenArea.Name;
                    }
                    else if (currScriptLine.Contains("~~EndString:"))
                    {
                        ProcessEndString();
                        SetArea(originalArea);
                        originalArea = null;
                    }
                    else if (currScriptLine.Contains("~~:"))
                    {
                        if (_targetGenArea.Name == "Using" || _targetGenArea.Name == "Usings")
                        {
                            _targetGenArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, -1));
                        }
                        else if (_targetGenArea.Name == "ClassMembers")
                        {
                            _targetGenArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, -1));
                        }
                        else
                        {
                            _targetGenArea.Lines.Add(XlgQuickScript.ExpandScriptLineToSourceCode(currScriptLine, indent));
                        }
                    }
                    else if (originalArea != null)
                    {
                        _targetGenArea.Lines.Add(currScriptLine);
                    }
                    else
                    {
                        _targetGenArea.Lines.Add(new string(' ', indent + _targetGenArea.Indent) + currScriptLine);
                    }
                }

                var sb = new StringBuilder(Template.Assets[_mIndependent ? "Exe" : "Native"].Value);
                foreach (var area in this)
                {
                    sb.Replace(Asset.LeftDelimiter + area.Name + Asset.RightDelimiter, string.Join(Environment.NewLine, area.Lines));
                }

                if (_mIndependent)
                {
                    sb.Replace("//~~InputFilePath~~//", "\"" + _targetScript.InputFilePath.LastToken(@"\") + "\"");
                    sb.Replace("//~~Namespace~~//", (_targetScript.Name + "_" + DateTime.UtcNow.ToString("G") + "z").AsFilename());
                    sb.Replace("//~~NameInstance~~//", _targetScript.Name + " at " + DateTime.Now.ToString("G"));

                    switch (_targetScript.Destination)
                    {
                        case QuickScriptDestination.TextBox:
                        case QuickScriptDestination.Clipboard:
                        case QuickScriptDestination.Notepad:
                            sb.Replace("//~~DestinationFilePath~~//", "Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Left(13) + \".txt\")");
                            break;

                        case QuickScriptDestination.File:
                            sb.Replace("//~~DestinationFilePath~~//", "\"" + _targetScript.DestinationFilePath.LastToken(@"\") + "\"");
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

            _targetGenArea.Lines.Add($"string {stringName} = //~~String {stringName}~~//;");
            SetArea("String " + stringName);
            return true;
        }

        private void ProcessEndString()
        {
            if (_targetGenArea.Lines.IsEmpty())
            {
                _targetGenArea.Lines.Add("string.Empty");
            }
            else if (!_targetGenArea.Lines.Any(x => x.Contains("\"")))
            {
                _targetGenArea.Lines[0] = "@\"" + _targetGenArea.Lines[0];
                _targetGenArea.Lines[_targetGenArea.Lines.Count - 1] += "\"";
            }
            else if (!_targetGenArea.Lines.Any(x => x.Contains("`")))
            {
                _targetGenArea.Lines.TransformAllNotEmpty(line => line.Replace("\"", "``"));
                _targetGenArea.Lines[0] = "@\"" + _targetGenArea.Lines[0];
                _targetGenArea.Lines[_targetGenArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
            }
            else if (!_targetGenArea.Lines.Any(x => x.Contains("`")))
            {
                _targetGenArea.Lines.TransformAllNotEmpty(line =>
                    "\t\"" + line.Replace("\"", "\\\"") + "\" + Environment.NewLine;" + Environment.NewLine);
                _targetGenArea.Lines[0] = "@\"" + _targetGenArea.Lines[0];
                _targetGenArea.Lines[_targetGenArea.Lines.Count - 1] += "\".Replace(\"``\",\"\\\"\")";
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
                if(_targetScript.DestinationFilePath.IsNotEmpty())
                    _targetGenArea.Lines.Add($"Output.AppendTo(@\"{_targetScript.DestinationFilePath}\");");
                else
                    _targetGenArea.Lines.Add("// ~~To found nothing to do");
            }
            else
            {
                var resolvedFilePath = XlgQuickScript.ExpandScriptLineVariables(filePath);
                _targetGenArea.Lines.Add($"Output.SwitchTo(@\"{resolvedFilePath}\");");
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
                if(_targetScript.DestinationFilePath.IsNotEmpty())
                    _targetGenArea.Lines.Add($"Output.AppendTo(@\"{_targetScript.DestinationFilePath}\");");
                else
                    _targetGenArea.Lines.Add("// ~~To found nothing to do");
            }
            else
            {
                var resolvedFilePath = XlgQuickScript.ExpandScriptLineVariables(filePath);
                _targetGenArea.Lines.Add($"Output.AppendTo(@\"{resolvedFilePath}\");");
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

            _targetGenArea.Lines.Add($"{lineFunction}(@\"{resolvedFilePath}\");");
            return resolvedFilePath;
        }

        private void SetArea(string areaName)
        {
            foreach (var area in this.Where(area => area.Name == areaName))
            {
                _targetGenArea = area;
                return;
            }
            _targetGenArea = new GenArea(areaName);
            Add(_targetGenArea);
        }
    }
}
