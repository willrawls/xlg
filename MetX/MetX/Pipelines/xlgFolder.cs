using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Pipelines
{
    /// <summary>
    /// Represents a library to generate
    /// </summary>
    [Serializable, XmlType(Namespace = "", AnonymousType = true)]
    public class XlgFolder
    {
        [XmlAttribute]
        public string Path;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public DateTime Created;
        [XmlAttribute]
        public DateTime Modified;

        [XmlArray("Folders", Namespace = "", IsNullable = false), XmlArrayItem("Folder", Namespace = "", IsNullable = false)]
        public List<XlgFolder> Folders = new List<XlgFolder>();

        [XmlArray("Files", Namespace = "", IsNullable = false), XmlArrayItem("File", Namespace = "", IsNullable = false)]
        public List<XlgFile> Files = new List<XlgFile>();

        public XlgFolder() { /* XmlSerializer */ }

        public XlgFolder(string path, string name, DateTime created, DateTime modified)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            this.Path = path;
            this.Name = name;
            this.Created = created;
            this.Modified = modified;
        }

        public override string ToString()
        {
            return Name;
        }

        public static XlgFolder FromXml(string xmldoc)
        {
            return Xml.FromXml<XlgFolder>(xmldoc);
        }

        public string OuterXml()
        {
            return Xml.ToXml(this, true);
        }

        public void Save(string filename)
        {
            Xml.SaveFile(filename, this);
        }

        public static XlgFolder Load(string filename)
        {
            return Xml.LoadFile<XlgFolder>(filename);
        }
    }
}