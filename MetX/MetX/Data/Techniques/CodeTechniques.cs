using System;
using System.Xml.Serialization;

namespace MetX.Data.Techniques
{
    [Serializable]
    public class CodeTechniques : ParticleList
    {
        [XmlArray(ElementName = "Connections")]
        [XmlArrayItem(typeof(Reference), ElementName = "Connection")]
        public ParticleList Connections;

        [XmlArray(ElementName = "Variables")]
        [XmlArrayItem(typeof(Reference), ElementName = "Variable")]
        public ParticleList Variables;

        [XmlArray(ElementName = "Providers")]
        [XmlArrayItem(typeof(Reference), ElementName = "Provider")]
        public ParticleList Providers;

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
    }
}