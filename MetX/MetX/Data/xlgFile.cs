using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;

namespace MetX.Data
{
    /// <summary>
    /// Represents a file
    /// </summary>
    [Serializable, XmlType(Namespace = "", AnonymousType = true)]
    public class xlgFile
    {
        [XmlAttribute]
        public string Path;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Extension;
        [XmlAttribute]
        public DateTime Created;
        [XmlAttribute]
        public DateTime Modified;
        [XmlAttribute]
        public long Size;

        public xlgFile() { /* XmlSerializer */ }

        public xlgFile(string path, string name, string extension, long size, DateTime created, DateTime modified)
        {
            if (!path.EndsWith(@"\")) path += @"\";
            Path = path;
            Name = name;
            Extension = extension;
            Size = size;
            Created = created;
            Modified = modified;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
