using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Data.Techniques
{
    [Serializable]
    public class CodeTechniques
    {
        [XmlAttribute]
        public Guid Id;

        [XmlArray(ElementName = "QuickScriptFiles")]
        [XmlArrayItem(typeof(Reference), ElementName = "QuickScriptFile")]
        public List<Reference> QuickScriptFiles;

        [XmlArray(ElementName = "PipelineFiles")]
        [XmlArrayItem(typeof(Reference), ElementName = "PipelineFile")]
        public List<Reference> PipelineFiles;

        [XmlArray(ElementName = "Connections")]
        [XmlArrayItem(typeof(Reference), ElementName = "Connection")]
        public List<Reference> Connections;

        [XmlArray(ElementName = "Locations")]
        [XmlArrayItem(typeof(Reference), ElementName = "Location")]
        public List<Reference> Locations;

        [XmlArray(ElementName = "Variables")]
        [XmlArrayItem(typeof(Reference), ElementName = "Variable")]
        public List<Reference> Variables;

        [XmlArray(ElementName = "ScriptTemplates")]
        [XmlArrayItem(typeof(Reference), ElementName = "ScriptTemplate")]
        public List<Reference> ScriptTemplates;

        [XmlArray(ElementName = "XslTemplates")]
        [XmlArrayItem(typeof(Reference), ElementName = "XslTemplate")]
        public List<Reference> XslTemplates;

        [XmlArray(ElementName = "StepSettingTemplates")]
        [XmlArrayItem(typeof(Reference), ElementName = "StepSettingTemplate")]
        public List<Reference> StepSettingTemplates;

        [XmlArray(ElementName = "Providers")]
        [XmlArrayItem(typeof(Reference), ElementName = "Provider")]
        public List<Reference> Providers;
    }

    [Serializable]
    public class Reference
    {
        [XmlAttribute]
        public int Index;
        [XmlAttribute]
        public string Name;
        //        [XmlAttribute]
        //        public string Value;
        [XmlAttribute]
        public ReferenceType RefType;
        [XmlAttribute]
        public string Value;
        [XmlAttribute]
        public string Content;
    }

    [Serializable]
    public enum ReferenceType
    {
        Unknown,
        File,
        Folder,
        Url,
        Database,
        Variable,
    }
}