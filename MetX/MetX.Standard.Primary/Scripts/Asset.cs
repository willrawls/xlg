using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Standard.Primary.Scripts
{
    public class Asset
    {
        public List<string> Variables { get; set; }
        public const string LeftDelimiter = "//~~";
        public const string RightDelimiter = "~~//";

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
                    .EveryTokenBetween(LeftDelimiter, RightDelimiter)
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();
            }
        }

        public string OriginalAssetFilename { get; set; }
        public string RelativePath { get; set; }
        public string RelativeFilePath => RelativePath.IsEmpty()
            ? OriginalAssetFilename
            : Path.Combine(RelativePath, OriginalAssetFilename);

        public string GetDestinationFilePath(ActualizationSettings settings)
        {
            var filename = OriginalAssetFilename;
            if (filename.StartsWith("_"))
            {
                filename = settings.TemplateNameAsLegalFilenameWithoutExtension + filename.Substring(1);
            }

            return RelativePath.IsEmpty() 
                ? Path.Combine(settings.ProjectFolder, filename) 
                : Path.Combine(settings.ProjectFolder, RelativePath, filename);
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
                result.OutputFiles[RelativeFilePath].Value = GetDestinationFilePath(result.Settings);
            }

            if (resolvedCode.Contains(LeftDelimiter) || resolvedCode.Contains(RightDelimiter))
            {
                var unresolved = _template
                    .EveryTokenBetween(LeftDelimiter, RightDelimiter)
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();
                result.ActualizeErrorText = $"Unresolvable variables encountered: {string.Join("\n", unresolved)}";
                return "";
            }

            return resolvedCode;
        }
    }
}