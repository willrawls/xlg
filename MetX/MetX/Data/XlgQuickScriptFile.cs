using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Data
{
    [Serializable]
    [XmlType(Namespace = "", AnonymousType = true)]
    public class XlgQuickScriptFile : List<XlgQuickScript>
    {
        public XlgQuickScript Default { get; set; }
        public string FilePath { get; set; }
        public XlgQuickScriptFile(string filePath) { FilePath = filePath; }

        public bool Save()
        {
            if (Count == 0)
            {
                return true;
            }
            if (File.Exists(FilePath))
            {
                File.Move(FilePath, 
                    FilePath + "_" + 
                    DateTime.Now.ToString("s").Replace(":", "").Replace("-", "").Replace("T", " ") + 
                    ".xlgq"
                    );
            }
            StringBuilder content = new StringBuilder();
            foreach (XlgQuickScript script in this)
            {
                content.AppendLine(script.ToFileFormat(script.Id == Default.Id));
            }
            File.WriteAllText(FilePath, content.ToString());
            string[] history = Directory.GetFiles(FilePath.TokensBeforeLast(@"\"), "*_* *.xlgq");
            if (history.Length > 5)
            {
                Array.Sort(history);
                for (int i = 5; i < history.Length; i++)
                {
                    File.SetAttributes(history[i], FileAttributes.Normal);
                    try
                    {
                        File.Delete(history[i]);
                    }
                    catch {  }
                }
            }
            return true;
        }

        public static XlgQuickScriptFile Load(string filePath)
        {
            XlgQuickScriptFile ret = new XlgQuickScriptFile(filePath);
            if (!File.Exists(ret.FilePath))
            {
                return ret;
            }
            string[] rawScripts = File
                .ReadAllText(ret.FilePath)
                .Split(new[] {"~~QuickScriptName:"}, StringSplitOptions
                .RemoveEmptyEntries);
            foreach (string rawScript in rawScripts)
            {
                XlgQuickScript script = new XlgQuickScript();
                bool isDefault = script.Parse(rawScript);
                ret.Add(script);
                if (isDefault) ret.Default = script;
            }
            if (ret.Default == null && ret.Count > 0)
            {
                ret.Default = ret[0];
            }
            return ret;
        }
    }
}