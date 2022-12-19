using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
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

        public static XlgQuickScriptFile Load(string filePath)
        {
            var scriptFile = new XlgQuickScriptFile(filePath);
            if (!File.Exists(scriptFile.FilePath))
            {
                return scriptFile;
            }

            var rawScripts = new string[1];

            var scriptNameSection = "~~QuickScriptName:";
            if (filePath.ToLower().EndsWith(".fimm"))
            {
                var fimmScript = File.ReadAllText(scriptFile.FilePath);
                if (fimmScript.Contains(scriptNameSection))
                    rawScripts[0] = $"{fimmScript}";
                else
                {
                    if (!fimmScript.ToLower().Contains("~~Line:"))
                        fimmScript = $"~~Line:\n{fimmScript}";

                    var xlgFimmScript =
                        new XlgQuickScript(scriptFile.FilePath.LastPathToken().FirstToken("."), fimmScript)
                        {
                            Input = "Clipboard",
                            Destination = QuickScriptDestination.Clipboard,
                            Id = Guid.NewGuid(),
                            TemplateName = "Exe",
                        };

                    scriptFile.Add(xlgFimmScript);
                    return scriptFile;
                    /*
rawScripts[0] = xlgFimmScript.ToFileFormat(true);
$@"
{scriptNameSection} {scriptFile.FilePath.LastPathToken().FirstToken(".")}
~~QuickScriptID: {Guid.NewGuid():B}
~~QuickScriptInput: Clipboard
~~QuickScriptDestination: Clipboard
__QuickScriptDefault:
~~
~~Line:
{fimmScript}";
                     */
                }

            }
            else
            {
                rawScripts = File
                    .ReadAllText(scriptFile.FilePath)
                    .Split(new[] {scriptNameSection}, StringSplitOptions.RemoveEmptyEntries);
            }            
            foreach (var rawScript in rawScripts)
            {
                if (string.IsNullOrWhiteSpace(rawScript)) continue;
                var script = new XlgQuickScript();
                var isDefault = script.Load(rawScript);
                scriptFile.Add(script);
                if (isDefault) scriptFile.Default = script;
            }
            scriptFile.Sort((script, quickScript) => string.CompareOrdinal(script.Name, quickScript.Name));
            if (scriptFile.Default == null && scriptFile.Count > 0)
            {
                scriptFile.Default = scriptFile[0];
            }
            return scriptFile;
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