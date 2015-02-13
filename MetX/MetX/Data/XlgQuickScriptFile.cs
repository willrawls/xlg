using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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
                File.Move(FilePath, FilePath + "_" + DateTime.Now.ToString("s"));
            }
            StringBuilder content = new StringBuilder();
            foreach (XlgQuickScript script in this)
            {
                content.AppendLine(script.ToString(script.Id == Default.Id));
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
            XlgQuickScript currScript = new XlgQuickScript();
            foreach (string line in File.ReadAllLines(ret.FilePath))
            {
                XlgQuickScript.LoadLineFromFile(ret, ref currScript, line);
            }

            if (!string.IsNullOrEmpty(currScript.Name) && currScript.Script.Length > 0)
            {
                ret.Add(currScript);
            }

            if (ret.Default == null && ret.Count > 0)
            {
                ret.Default = ret[0];
            }
            return ret;
        }
    }
}