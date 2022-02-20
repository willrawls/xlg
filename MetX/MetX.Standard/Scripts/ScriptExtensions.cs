using System;
using System.Collections.Generic;
using System.Text;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using Microsoft.CodeAnalysis;

namespace MetX.Standard.Scripts
{
    // ReSharper disable once UnusedType.Global
    public static class ScriptExtensions
    {
        public static int Line(this Location location)
        {
            return location == null 
                ? 0 
                : location.GetMappedLineSpan().StartLinePosition.Line;
        }

        public static string ForDisplay(this IList<Diagnostic> failures, IList<string> lines)
        {
            if (failures == null || failures.Count == 0)
                return string.Empty;
            
            var sb = new StringBuilder();

            sb.AppendLine("Compilation failure. Errors found include:");
            sb.AppendLine();

            if (lines.IsEmpty())
            {
                for (var index = 0; index < failures.Count; index++)
                {
                    sb.AppendLine(index + 1 + ": Line "
                                              + failures[index]
                                                  .ToString()
                                                  .TokensAfterFirst("(")
                                                  .Replace(")", string.Empty));
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            

            for (var index = 0; index < failures.Count; index++)
            {
                var error = failures[index].ToString();
                if (error.Contains("("))
                {
                    error = error.TokensAfterFirst("(").Replace(")", string.Empty);
                }

                sb.AppendLine(index + 1 + ": Line " + error);
                sb.AppendLine();
                if (error.Contains(Environment.NewLine))
                {
                    lines[failures[index].Location.Line()] += "\t// " + error.Replace(Environment.NewLine, " ");
                    lines[failures[index].Location.Line()] += "\t// " + error.Replace(Environment.NewLine, " ");
                }
                else if (failures[index].Location.Line() == 0)
                {
                    lines[0] += "\t// " + error;
                }
                else
                {
                    lines[failures[index].Location.Line()] += "\t// " + error;
                }
            }
            return sb.ToString();
        }
    }
}