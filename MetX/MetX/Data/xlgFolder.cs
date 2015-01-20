using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Library;

namespace MetX.Data
{
    /// <summary>
    /// Represents a library to generate
    /// </summary>
    [Serializable, XmlType(Namespace="",AnonymousType=true)]
    public class xlgFolder
    {
        [XmlAttribute] public string Path;
        [XmlAttribute] public string Name;
        [XmlAttribute] public DateTime Created;
        [XmlAttribute] public DateTime Modified;

        [XmlArray("Folders", Namespace = "", IsNullable = false), XmlArrayItem("Folder", Namespace = "", IsNullable = false)]
        public List<xlgFolder> Folders = new List<xlgFolder>();

        [XmlArray("Files", Namespace = "", IsNullable = false), XmlArrayItem("File", Namespace = "", IsNullable = false)]
        public List<xlgFile> Files = new List<xlgFile>();

        public xlgFolder() { /* XmlSerializer */ }

        public xlgFolder(string Path, string Name, DateTime Created, DateTime Modified)
        {
            if (!Path.EndsWith(@"\")) Path += @"\";
            this.Path = Path;
            this.Name = Name;
            this.Created = Created;
            this.Modified = Modified;
        }

        public override string ToString()
        {
            return Name;
        }

        public static xlgFolder FromXml(string xmldoc)
        {
            return Xml.FromXml<xlgFolder>(xmldoc);
        }

        public string OuterXml()
        {
            return Xml.ToXml<xlgFolder>(this, true);
        }

        public void Save(string Filename)
        {
            Xml.SaveFile<xlgFolder>(Filename, this);
        }

        public static xlgFolder Load(string Filename)
        {
            return Xml.LoadFile<xlgFolder>(Filename);
        }
    }
}