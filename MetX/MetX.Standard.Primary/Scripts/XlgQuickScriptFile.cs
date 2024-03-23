using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.Strings;

namespace MetX.Standard.Primary.Scripts
{
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScriptFile : List<XlgQuickScript>
    {
        public XlgQuickScript Default { get; set; }

        public string FilePath { get; set; }

        public XlgQuickScriptFile(string filePath) { FilePath = filePath; }

        public XlgQuickScript this[string scriptName]
        {
            get
            {
                if (scriptName.IsEmpty())
                    return null;
                
                return this.FirstOrDefault(s =>
                    string.Equals(scriptName, s.Name, StringComparison
                        .InvariantCultureIgnoreCase));
            }
        }

        public bool Save(string backupPath)
        {
            if (Count == 0 || string.IsNullOrEmpty(FilePath))
            {
                return true;
            }

            Directory.CreateDirectory(FilePath.TokensBeforeLast(@"\"));
            Directory.CreateDirectory(backupPath);

            if (File.Exists(FilePath))
            {
                var filename =  FilePath.LastPathToken().FirstToken(".")
                                      + "_" + DateTime.Now.ToString("s").Replace(":", "").Replace("-", "").Replace("T", " ") 
                                      +  ".xlgq";

                var backupFilePath = Path.Combine(backupPath, filename);
                File.Move(FilePath, backupFilePath);
            }
            var content = new StringBuilder();
            foreach (var script in this)
            {
                content.AppendLine(script.ToFileFormat(script.Id == Default.Id));
            }
            File.WriteAllText(FilePath, content.ToString());
            var history = Directory.GetFiles(FilePath.TokensBeforeLast(@"\"), "*_* *.xlgq");
            if (history.Length > 4)
            {
                Array.Sort(history);
                for (var i = 4; i < history.Length; i++)
                {
                    File.SetAttributes(history[i], FileAttributes.Normal);
                    try
                    {
                        File.Delete(history[i]);
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch { }
                }
            }
            return true;
        }

        const string scriptNameSection = "~~QuickScriptName:";

        public static XlgQuickScriptFile Load(string filePath)
        {
            if (filePath.IsEmpty() || !File.Exists(filePath))
            {
                return null;
            }

            var quickScriptFile = new XlgQuickScriptFile(filePath);

            if (filePath.ToLower().EndsWith(".fimm"))
            {
                var fimmScript = FimmFactory(File.ReadAllText(quickScriptFile.FilePath), quickScriptFile);
                quickScriptFile.Add(fimmScript);
                return quickScriptFile;
            }

            var rawScripts = File
                .ReadAllText(quickScriptFile.FilePath)
                .Split(new[] {scriptNameSection}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawScript in rawScripts)
            {
                if (string.IsNullOrWhiteSpace(rawScript)) continue;
                var script = new XlgQuickScript();
                var isDefault = script.Load(rawScript);
                quickScriptFile.Add(script);
                if (isDefault) quickScriptFile.Default = script;
            }
            quickScriptFile.Sort((script, quickScript) => string.CompareOrdinal(script.Name, quickScript.Name));
            if (quickScriptFile.Default == null && quickScriptFile.Count > 0)
            {
                quickScriptFile.Default = quickScriptFile[0];
            }
            return quickScriptFile;
        }

        public static string FimmFileFormatScriptFactory(string script)
        {
            if (script.Contains(scriptNameSection))
                return script;

            var xlgScriptFile = new XlgQuickScriptFile(Guid.NewGuid().ToString("N"));
            var xlgFimmScript = FimmFactory(script, xlgScriptFile);

            return xlgFimmScript.ToFileFormat(true);
        }

        public static XlgQuickScript FimmFactory(string fimmScript, XlgQuickScriptFile scriptFile)
        {
            if (!fimmScript.ToLower().Contains("~~line:"))
                fimmScript = $"~~Line:\n{fimmScript}";

            var xlgFimmScript =
                new XlgQuickScript(scriptFile.FilePath.LastPathToken().FirstToken("."), fimmScript)
                {
                    Input = "Clipboard",
                    Destination = QuickScriptDestination.Clipboard,
                    Id = Guid.NewGuid(),
                    TemplateName = "Exe",
                };
            return xlgFimmScript;
        }

        public string[] ScriptNames()
        {
            var names = new string[Count];
            for (var i = 0; i < Count; i++)
            {
                names[i] = this[i].Name;
            }

            return names;
        }
    }
}