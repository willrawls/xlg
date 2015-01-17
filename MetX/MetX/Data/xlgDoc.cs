using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
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

        public xlgFile(string Path, string Name, string Extension, long Size, DateTime Created, DateTime Modified)
        {
            if (!Path.EndsWith(@"\")) Path += @"\";
            this.Path = Path;
            this.Name = Name;
            this.Extension = Extension;
            this.Size = Size;
            this.Created = Created;
            this.Modified = Modified;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
