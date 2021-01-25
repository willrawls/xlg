using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using MetX.Scripts;

namespace MetX.Pipelines
{
    [Serializable, XmlRoot(Namespace = "", IsNullable = false)]
    public class XlgSettings
    {
        private static XmlSerializer _mSettingsSerializer = new(typeof(XlgSettings));

        [XmlArray("QuickScripts", Namespace = "", IsNullable = false),
         XmlArrayItem("QuickScript", Namespace = "", IsNullable = false)]
        public IList QuickScripts = new List<XlgQuickScript>();

        [XmlAttribute]
        public string DefaultConnectionString;
        [XmlAttribute]
        public string DefaultProviderName;
        [XmlAttribute]
        public string Filename;
        [XmlIgnore]
        public Form Gui;

        [XmlArray("Sources", Namespace = "", IsNullable = false),
         XmlArrayItem("Source", Namespace = "", IsNullable = false)]
        public List<XlgSource> Sources = new();

        public XlgSettings()
        {
            /* XmlSerilizer */
        }

        public XlgSettings(Form gui)
        {
            Gui = gui;
        }

        public static XlgSettings FromXml(string xmldoc)
        {
            return (XlgSettings)_mSettingsSerializer.Deserialize(new StringReader(xmldoc));
        }

        public string OuterXml()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
                _mSettingsSerializer.Serialize(sw, this);
            return sb.ToString();
        }

        public void Save()
        {
            if (QuickScripts == null)
                QuickScripts = new List<XlgQuickScript>();
            File.WriteAllText(Filename, OuterXml());
        }

        public static XlgSettings Load(string filename)
        {
            return FromXml(File.ReadAllText(filename));
        }

        public int Generate(Form gui)
        {
            var genCount = 0;
            foreach (var currSource in Sources)
            {
                if (currSource.Selected)
                {
                    int lastGen;
                    lastGen = currSource.RegenerateOnly ? currSource.Regenerate(gui) : currSource.Generate(gui);
                    if (lastGen == -1) return -genCount;
                    genCount++;
                }
            }
            return genCount;
        }

        public int Regenerate(Form gui)
        {
            var genCount = 0;
            foreach (var currSource in Sources)
            {
                if (currSource.Selected)
                {
                    var lastGen = currSource.Regenerate(gui);
                    if (lastGen == -1) return -genCount;
                    genCount++;
                }
            }
            return genCount;
        }
    }
}