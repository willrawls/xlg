using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

            foreach (var file in Directory.GetFiles(TemplatePath))
            {
                var assetName = file.LastPathToken();
                var asset = Assets[assetName].Item;
                asset.Template = File.ReadAllText(file);
                asset.OriginalAssetFilename = assetName;
            }
        }

        public ActualizationResult ActualizeCode(ActualizationSettings settings)
        {
            if (!settings.Simulate)
                Directory.CreateDirectory(settings.OutputFolder);
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

                var filePath = asset.Item.GetFilePath(settings);
                result.OutputFiles[asset.Item.OriginalAssetFilename].Value = resolvedCode;

                if (!settings.Simulate)
                    if (!FileSystem.SafeDelete(filePath))
                    {
                        result.ActualizeErrorText = $"Unable to write {asset.Key} to {filePath}";
                        return result;
                    }

                if (!settings.Simulate)
                    File.WriteAllText(filePath, resolvedCode);
            }

            if (result.ActualizationSuccessful)
            {
                // Stage support files (MetX.*.dll & .pdb)
                var contents = FileSystem.DeepContents(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));

                List<XlgFile> filesToCopy = new List<XlgFile>();

                bool FileMatcher(XlgFile file)
                {
                    if (file.Extension.ToLower() is ".pdb" or ".dll" 
                        && file.Name.ToLower().Contains("metx.")) 
                        filesToCopy.Add(file);
                    return true;
                }
                contents.ForEachFile(func: FileMatcher);

                foreach (var file in filesToCopy)
                {
                    file.CopyTo(result.Settings.OutputFolder);
                }
            }

            return result;
        }

        private void SetupAnswers(ActualizationResult result)
        {
            var answers = result.Settings.Answers;

            answers["DestinationFilePath"].Value = result.Settings.Source.DestinationFilePath;
            answers["InputFilePath"].Value = result.Settings.Source.InputFilePath;
            answers["NameInstance"].Value = result.Settings.TemplateNameAsLegalFilenameWithoutExtension;
            answers["Project Name"].Value = result.Settings.ProjectName;
            answers["UserName"].Value = Environment.UserName.LastToken(@"\").AsString("Unknown");
            answers["Guid Config"].Value = Guid.NewGuid().ToString("D");
            answers["Guid Project 1"].Value = Guid.NewGuid().ToString("D");
            answers["Guid Project 2"].Value = Guid.NewGuid().ToString("D");
            answers["Guid Solution"].Value = Guid.NewGuid().ToString("D");
            answers["Generated At"].Value = DateTime.Now.ToUniversalTime().ToString("s");

            var generationInstance = result.Settings.GeneratedAreas;
            generationInstance.ParseAndBuildAreas();

            foreach (var area in generationInstance)
                answers[area.Name].Value = area.ToString();
        }
    }
}