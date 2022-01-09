using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Generics;
using MetX.Standard.Pipelines;

namespace MetX.Standard.Scripts
{
    [Serializable]
    public class XlgQuickScriptTemplate
    {
        public string Name { get; set; }
        public string TemplatePath { get; set; }

        public AssocArray<Asset> Assets = new();

        public XlgQuickScriptTemplate(string templatePath, string name = null)
        {
            TemplatePath = templatePath;
            Name = name ?? TemplatePath.LastPathToken();
            if (!Directory.Exists(TemplatePath)) return;

            ProcessPath(TemplatePath, TemplatePath);
        }

        private void ProcessPath(string originalPath, string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                var assetName = file.LastPathToken();
                var asset = Assets[assetName].Item;
                asset.Template = File.ReadAllText(file);
                asset.OriginalAssetFilename = assetName;
                asset.RelativePath = path.LastToken(originalPath);
                if (asset.RelativePath.StartsWith(@"\"))
                    asset.RelativePath = asset.RelativePath.Substring(1);
            }

            foreach (var subfolder in Directory.GetDirectories(path))
            {
                ProcessPath(originalPath, subfolder);
            }
        }

        public ActualizationResult ActualizeCode(ActualizationSettings settings)
        {
            if (!settings.Simulate)
                Directory.CreateDirectory(settings.ProjectFolder);
            var result = new ActualizationResult(settings);

            SetupAnswers(result);

            foreach (var asset in Assets)
            {
                var resolvedCode = asset.Item.ResolveVariables(result);

                if (resolvedCode.IsEmpty())
                {
                    result.ActualizeErrorText = $"{asset.Key} resolved to no code";
                    return result;
                }

                if (resolvedCode.Contains(Asset.LeftDelimiter) || resolvedCode.Contains(Asset.RightDelimiter))
                {
                    result.ActualizeErrorText = $"Not all variables in {asset.Key} file resolved";
                    return result;
                }

                var filePath = asset.Item.GetDestinationFilePath(settings);
                result.OutputFiles[asset.Item.RelativeFilePath].Value = resolvedCode;

                if (!settings.Simulate)
                    if (!FileSystem.SafelyDeleteFile(filePath))
                    {
                        result.ActualizeErrorText = $"Unable to write {asset.Key} to {filePath}";
                        return result;
                    }

                if (!settings.Simulate)
                {
                    var folder = filePath.TokensBeforeLast(@"\");
                    Directory.CreateDirectory(folder);
                    File.WriteAllText(filePath, resolvedCode);
                }
            }

            if (result.ActualizationSuccessful)
            {
                StageSupportFiles(result);

                var filename = settings.TemplateNameAsLegalFilenameWithoutExtension.AsFilename(settings.ForExecutable ? ".exe" : ".dll");
                result.DestinationExecutableFilePath = Path.Combine(settings.ProjectFolder, "bin", "Debug", "net5.0", filename);

            }

            return result;
        }

        public static void StageSupportFiles(ActualizationResult result)
        {
            return; // Currently unneeded

            // Stage support files (MetX.*.dll & .pdb)
            var contents = FileSystem.DeepContents(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));

            List<XlgFile> filesToCopy = new List<XlgFile>();

            contents.ForEachFile(func: file =>
            {
                if (file.Extension.ToLower() is ".pdb" or ".dll"
                    && file.Name.ToLower().Contains("metx."))
                    filesToCopy.Add(file);
                return true;
            });

            foreach (var file in filesToCopy)
            {
                file.CopyTo(result.Settings.ProjectFolder);
            }
        }

        private void SetupAnswers(ActualizationResult result)
        {
            var sourceInputFilePath = result.Settings.Script.InputFilePath;
            switch (result.Settings.Script.Input.ToLower())
            {
                case "console":
                case "clipboard":
                    sourceInputFilePath = result.Settings.Script.Input;
                    break;
            }
            result.Settings.Answers["InputFilePath"].Value = sourceInputFilePath;

            var sourceDestinationFilePath = result.Settings.Script.DestinationFilePath;
            switch (result.Settings.Script.Destination)
            {
                case QuickScriptDestination.Clipboard:
                    sourceDestinationFilePath = "clipboard";
                    break;
                case QuickScriptDestination.Notepad:
                    sourceDestinationFilePath = "open";
                    break;
                case QuickScriptDestination.TextBox:
                    sourceDestinationFilePath = "console";
                    break;
            }
            result.Settings.Answers["DestinationFilePath"].Value = sourceDestinationFilePath;

            result.Settings.Answers["Script Name"].Value = result.Settings.Script.Name;
            result.Settings.Answers["Script Id"].Value = result.Settings.Script.Id.ToString("B");
            result.Settings.Answers["Slice At"].Value = result.Settings.Script.SliceAt;
            result.Settings.Answers["Dice At"].Value = result.Settings.Script.DiceAt;
            result.Settings.Answers["Generated At"].Value = DateTime.Now.ToUniversalTime().ToString("s");
            result.Settings.Answers["UserName"].Value = Environment.UserName.LastToken(@"\").AsString("Unknown");

            result.Settings.Answers["NameInstance"].Value = result.Settings.TemplateNameAsLegalFilenameWithoutExtension.Replace("-", "").Replace(" ", "_");
            result.Settings.Answers["Project Name"].Value = result.Settings.TemplateNameAsLegalFilenameWithoutExtension;

            result.Settings.Answers["Guid Config"].Value = Guid.NewGuid().ToString("D");
            result.Settings.Answers["Guid Project 1"].Value = Guid.NewGuid().ToString("D");
            result.Settings.Answers["Guid Project 2"].Value = Guid.NewGuid().ToString("D");
            result.Settings.Answers["Guid Solution"].Value = Guid.NewGuid().ToString("D");

            var generationInstance = result.Settings.GeneratedAreas;
            generationInstance.ParseAndBuildAreas();

            foreach (var area in generationInstance)
            {
                var item = result.Settings.Answers[area.Name];
                if (item.Value.IsEmpty())
                {
                    item.Value = area.ToString();
                }
                else
                {

                }

            }
        }
    }
}