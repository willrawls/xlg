using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MetX.Data
{
    [Serializable, XmlRoot(Namespace = "", IsNullable = false)]
    public class XlgSettings
    {
        private static XmlSerializer m_SettingsSerializer = new XmlSerializer(typeof (XlgSettings));

        [XmlArray("QuickScripts", Namespace = "", IsNullable = false),
         XmlArrayItem("QuickScript", Namespace = "", IsNullable = false)] public List<XlgQuickScript> QuickScripts = new List<XlgQuickScript>();

        [XmlAttribute] public string DefaultConnectionString;
        [XmlAttribute] public string DefaultProviderName;
        [XmlAttribute] public string Filename;
        [XmlIgnore] public Form Gui;

        [XmlArray("Sources", Namespace = "", IsNullable = false),
         XmlArrayItem("Source", Namespace = "", IsNullable = false)] public List<XlgSource> Sources = new List<XlgSource>();

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
            return (XlgSettings) m_SettingsSerializer.Deserialize(new StringReader(xmldoc));
        }

        public string OuterXml()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                m_SettingsSerializer.Serialize(sw, this);
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
            int genCount = 0;
            foreach (XlgSource currSource in Sources)
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
            int genCount = 0;
            foreach (XlgSource currSource in Sources)
            {
                if (currSource.Selected)
                {
                    int lastGen = currSource.Regenerate(gui);
                    if (lastGen == -1) return -genCount;
                    genCount++;
                }
            }
            return genCount;
        }
    }
}