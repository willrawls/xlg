using System;
using System.IO;
using MetX.Standard.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Generics;

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

        public ActualizationResult Actualize(ActualizationSettings settings)
        {
            if(!settings.Simulate)
                Directory.CreateDirectory(settings.OutputFolder);
            var result = new ActualizationResult(settings);

            foreach (var asset in Assets)
            {
                var resolvedCode = asset.Item.ResolveVariables(result);

                if (resolvedCode.IsEmpty())
                {
                    result.ErrorText = $"{asset.Key} resolved to no code";
                    return result;
                }

                if(resolvedCode.Contains("//~~") || resolvedCode.Contains("~~//"))
                {
                    result.ErrorText = $"Not all variables in {asset.Key} file resolved";
                    return result;
                }

                var filePath = asset.Item.GetFilePath(settings);
                result.OutputFiles[asset.Item.OriginalAssetFilename].Value = resolvedCode;
                
                if(!settings.Simulate)
                    if (!FileSystem.SafeDelete(filePath))
                    {
                        result.ErrorText = $"Unable to write {asset.Key} to {filePath}";
                        return result;
                    }

                if(!settings.Simulate)
                    File.WriteAllText(filePath, resolvedCode);
            }

            return result;

        }
    }
}