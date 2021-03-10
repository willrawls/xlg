using MetX.Library;
using System;
using System.Xml.Serialization;

#pragma warning disable 1591

namespace MetX.Techniques
{
    [Serializable]
    public class PatternWorks
    {
        [XmlArray(ElementName = "Connections")]
        [XmlArrayItem(typeof(Connection), ElementName = "Connection")]
        public
            ParticleList<Connection> Connections;

        [XmlArray(ElementName = "Providers")]
        [XmlArrayItem(typeof(Provider), ElementName = "Provider")]
        public
            ParticleList<Provider> Providers;

        [XmlArray(ElementName = "Techniques")]
        [XmlArrayItem(typeof(Technique), ElementName = "Technique")]
        public
            ParticleList<Technique> Techniques;

        [XmlArray(ElementName = "Variables")]
        [XmlArrayItem(typeof(Variable), ElementName = "Variable")]
        public
            ParticleList<Variable> Variables;

        /*
                [XmlArray(ElementName = "QuickScriptFiles")]
                [XmlArrayItem(typeof(Reference), ElementName = "QuickScriptFile")]
                public ParticleList QuickScriptFiles;

                [XmlArray(ElementName = "PipelineFiles")]
                [XmlArrayItem(typeof(Reference), ElementName = "PipelineFile")]
                public ParticleList PipelineFiles;
        */

        /*
                [XmlArray(ElementName = "ScriptTemplateFiles")]
                [XmlArrayItem(typeof(Reference), ElementName = "ScriptTemplateFile")]
                public ParticleList ScriptTemplateFiles;

                [XmlArray(ElementName = "XslTemplateFiles")]
                [XmlArrayItem(typeof(Reference), ElementName = "XslTemplateFile")]
                public ParticleList XslTemplateFiles;

                [XmlArray(ElementName = "StepSettingTemplateFiles")]
                [XmlArrayItem(typeof(Reference), ElementName = "StepSettingTemplateFile")]
                public ParticleList StepSettingTemplateFiles;
        */

        /*
                [XmlArray(ElementName = "Locations")]
                [XmlArrayItem(typeof(Reference), ElementName = "Location")]
                public ParticleList Locations;
        */

        public string ToXml()
        {
            return Xml.ToXml(this, false);
        }

        public static PatternWorks FromXml(string xmlDoc)
        {
            return xmlDoc.IsEmpty()
                ? null
                : Xml.FromXml<PatternWorks>(xmlDoc);
        }
    }
}