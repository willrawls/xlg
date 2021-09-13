using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Scripts
{
    public class Asset
    {
        public List<string> Variables { get; set; }
        public const string LeftDelimiter = "//~~";
        public const string rightDelimiter = "~~//";

        private string _template;
        public string Template
        {
            get => _template;
            set
            {
                if (_template == value)
                    return;

                _template = value ?? "";

                Variables = _template
                    .EveryTokenBetween(LeftDelimiter, rightDelimiter)
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();
            }
        }

        public string OriginalAssetFilename { get; set; }

        public string GetFilePath(ActualizationSettings settings)
        {
            var filename = OriginalAssetFilename;
            if (filename.StartsWith("_"))
            {
                filename = settings.ProjectName + filename.Substring(1);
            }

            return Path.Combine(settings.OutputFolder, filename);
        }

        public string ResolveVariables(ActualizationResult result)
        {
            if (_template.IsEmpty())
                return string.Empty;

            var resolvedCode = _template;
            foreach (var variable in Variables)
            {
                var tag = $"//~~{variable}~~//";
                var resolveTimeVariable = result.Settings.Answers[variable];
                resolveTimeVariable.Number += resolvedCode.TokenCount(tag);
                if (resolveTimeVariable.Value.IsEmpty())
                {
                    result.Warnings.Add($"Variable '{variable}' resolved to an empty string.");
                }
                resolvedCode = resolvedCode.Replace(tag, resolveTimeVariable.Value);
                result.OutputFiles[OriginalAssetFilename].Value = GetFilePath(result.Settings);
            }

            if (resolvedCode.Contains(LeftDelimiter) || resolvedCode.Contains(rightDelimiter))
            {
                var unresolved = _template
                    .EveryTokenBetween(LeftDelimiter, rightDelimiter)
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();
                result.ErrorText = $"Unresolvable variables encountered: {string.Join("\n", unresolved)}";
                return "";
            }

            return resolvedCode;
        }
    }
}