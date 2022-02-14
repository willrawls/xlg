using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MetX.Standard.Library;
using MetX.Standard.Library.ML;

namespace MetX.Standard.Pipelines
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
        public List<XlgFolder> Folders = new();

        [XmlArray("Files", Namespace = "", IsNullable = false), XmlArrayItem("File", Namespace = "", IsNullable = false)]
        public List<XlgFile> Files = new();

        public XlgFolder() { /* XmlSerializer */ }

        public XlgFolder(string path, string name, DateTime created, DateTime modified)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            Path = path;
            Name = name;
            Created = created;
            Modified = modified;
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
            return Xml.ToXml(this);
        }

        public void Save(string filename)
        {
            Xml.SaveFile(filename, this);
        }

        public static XlgFolder Load(string filename)
        {
            return Xml.LoadFile<XlgFolder>(filename);
        }

        public bool ForEachFile(Func<XlgFile, bool> func)
        {
            foreach (XlgFile file in Files)
            {
                if(!func(file))
                    return false; // Don't continue
            }

            foreach (var folder in Folders)
            {
                if (!folder.ForEachFile(func))
                    return false;
            }

            return true;
        }
    }
}