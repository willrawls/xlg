using System;
using System.Collections;
using System.Linq;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Standard.Primary.Scripts
{
    public static class Helpers
    {
        public const string SlashSlashBlockLeftDelimiter = "~~{";
        public const string SlashSlashBlockRightDelimiter = "}~~";

        public static string ExpandScriptLineToSourceCode(string scriptLine, int indent)
        {
            // backslash percent will translate to % after parsing
            scriptLine = scriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

            scriptLine = ExpandScriptLineVariables(scriptLine);

            scriptLine = $"Output.AppendLine(\"{scriptLine.Mid(3).Replace(@"\", @"\\")}\");";
            scriptLine =
                scriptLine.Replace("AppendLine(\" + ", "AppendLine(")
                    .Replace(" + \"\")", ")")
                    .Replace("Output.AppendLine(\"\")", "Output.AppendLine()");
            scriptLine = (indent > 0
                             ? new string(' ', indent + 12)
                             : string.Empty) +
                         scriptLine
                             .Replace(" + \"\" + ", string.Empty)
                             .Replace("\"\" + ", string.Empty)
                             .Replace(" + \"\"", string.Empty)
                             .Replace("~$~$", @"%")
                             .Replace("~#~$", "\\\"");
            return scriptLine;
        }

        public static string ExpandScriptLineVariables(this string scriptLine)
        {
            var keepGoing = true;
            var iterations = 0;
            var environmentVariables = Environment.GetEnvironmentVariables();

            scriptLine = scriptLine.Replace("%PERCENT%", "%");

            foreach (DictionaryEntry environmentVariable in environmentVariables)
            {
                var tag = $"%{environmentVariable.Key}%";
                if (scriptLine.IndexOf(tag, StringComparison.InvariantCultureIgnoreCase) <= -1) continue;

                var value = environmentVariable.Value.AsString("");
                scriptLine = scriptLine.ImplicitReplace(tag, value);
            }

            while (keepGoing && ++iterations < 1000 && scriptLine.Contains("%")
                   && scriptLine.TokenCount("%") > 1)
            {
                var variableContent = scriptLine.TokenAt(2, "%");
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
                            // Array index
                            resolvedContent = "\" + (" + variableName + ".Length <= " + variableIndex
                                              + " ? string.Empty : " + variableName + "[" + variableIndex + "]) + \"";
                        else
                            resolvedContent = "\" + " + variableName + "[\"" + variableIndex + "\"] + \"";
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

                scriptLine = scriptLine.Replace("%" + variableContent + "%", resolvedContent);
            }

            return scriptLine;
        }

        public static string FormatCSharpCode(string code)
        {
            return code ?? "";
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