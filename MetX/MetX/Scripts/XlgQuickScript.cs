namespace MetX.Scripts
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    using Library;

    using Microsoft.CSharp;

    using NArrange.ConsoleApplication;
    using NArrange.Core;

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
        public string Template;

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
            Template = "Single file input";
        }

        public static CompilerResults CompileSource(string source, bool asExecutable, List<Assembly> additionalReferences)
        {
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = asExecutable,
                GenerateInMemory = !asExecutable,
                IncludeDebugInformation = asExecutable
            };
            if (asExecutable)
            {
                compilerParameters.MainClass = "Processor.Program";
                compilerParameters.OutputAssembly =
                    Guid.NewGuid().ToString().Replace(new[] { "{", "}", "-" }, string.Empty).ToLower() + ".exe";
            }

            compilerParameters
                .ReferencedAssemblies
                .AddRange(
                    AppDomain.CurrentDomain
                             .GetAssemblies()
                             .Where(a => !a.IsDynamic)
                             .Select(a => a.Location)
                             .ToArray());
            if ((additionalReferences != null) && (additionalReferences.Count > 0))
            {
                compilerParameters.ReferencedAssemblies.AddRange(
                    additionalReferences
                        .Select(a => a.Location)
                        .ToArray());
            }

            var compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters,
                source);
            return compilerResults;
        }

        public static string ExpandScriptLineToSourceCode(string currScriptLine, int indent)
        {
            // backslash percent will translate to % after parsing
            currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

            var keepGoing = true;
            var iterations = 0;
            while (keepGoing && (++iterations < 1000) && currScriptLine.Contains("%")
                   && (currScriptLine.TokenCount("%") > 1))
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

            currScriptLine = "Output.AppendLine(\"" + currScriptLine.Mid(3) + "\");";
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

        public static string FormatCSharpCode(string code)
        {
            var logger = new NArrangeTestLogger();
            var attempts = 0;
            while (++attempts < 2)
            {
                try
                {
                    var fileArranger = new FileArranger(null, logger);
                    var formattedCode = fileArranger.ArrangeSource(code);
                    return formattedCode ?? code;
                }
                catch (Exception ex)
                {
                    if (attempts == 2)
                    {
                        MessageBox.Show(ex.ToString());
                        return code;
                    }
                }
            }

            return code;
        }

        public static bool Run(ILogger logger, CommandArguments commandArgs)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (commandArgs == null)
            {
                throw new ArgumentNullException("commandArgs");
            }

            bool flag;
            if (commandArgs.Restore)
            {
                logger.LogMessage(LogLevel.Verbose, "Restoring {0}...", (object)commandArgs.Input);
                var fileNameKey = BackupUtilities.CreateFileNameKey(commandArgs.Input);
                try
                {
                    flag = BackupUtilities.RestoreFiles(BackupUtilities.BackupRoot, fileNameKey);
                }
                catch (Exception ex)
                {
                    logger.LogMessage(LogLevel.Warning, ex.Message);
                    flag = false;
                }

                if (flag)
                {
                    logger.LogMessage(LogLevel.Info, "Restored");
                }
                else
                {
                    logger.LogMessage(LogLevel.Error, "Restore failed");
                }
            }
            else
            {
                flag = new FileArranger(commandArgs.Configuration, logger).Arrange(commandArgs.Input, commandArgs.Output, commandArgs.Backup);
                if (!flag)
                {
                    logger.LogMessage(LogLevel.Error, "Unable to arrange {0}.", (object)commandArgs.Input);
                }
                else
                {
                    logger.LogMessage(LogLevel.Info, "Arrange successful.");
                }
            }

            return flag;
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
                Template = Template
            };
        }

        public bool Load(string rawScript)
        {
            var ret = false;
            SliceAt = "End of line";
            DiceAt = "Space";
            Template = "Single file input";

            if (string.IsNullOrEmpty(rawScript))
            {
                throw new ArgumentNullException("rawScript");
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
                foreach (var line in rawScript.Lines())
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
                        Template = line.TokensAfterFirst(":");
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

        public string ToCSharp(bool independent)
        {
            var code = new GenInstance(this, ContextBase.Default.Templates[Template], independent).CSharp;
            return code.IsEmpty()
                ? code
                : FormatCSharpCode(code);
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
                   "~~QuickScriptTemplate:" + Template + Environment.NewLine +
                   (isDefault
                       ? "~~QuickScriptDefault:" + Environment.NewLine
                       : string.Empty) +
                   Script.AsString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
