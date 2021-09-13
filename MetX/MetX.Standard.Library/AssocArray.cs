using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Library
{
    [Serializable]
    [XmlRoot(ElementName = "AssocArray")]
    public class AssocArray : List<AssocItem>, IAssocItem
    {
        [XmlIgnore] public object SyncRoot { get; } = new();
        [XmlIgnore] public IAssocItem Parent { get; set; }

        [XmlAttribute] public string Key { get; set; }
        [XmlAttribute] public string Value { get; set; }
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public Guid ID { get; set; }
        [XmlAttribute] public string Category { get; set; }
        [XmlAttribute] public string Number { get; set; }
        
        [XmlIgnore]
        public string[] Values
        {
            get
            {
                if (Count == 0)
                    return new string[0];
                var answer = this.Select(i => i.Value).ToArray();
                return answer;
            }
        }

        [XmlIgnore]
        public int[] Numbers
        {
            get
            {
                if (Count == 0)
                    return new int[0];
                var answer = this.Select(i => i.Number).ToArray();
                return answer;
            }
        }

        [XmlIgnore]
        public string[] Names
        {
            get
            {
                if (Count == 0)
                    return new string[0];
                var answer = this.Select(i => i.Name).ToArray();
                return answer;
            }
        }

        [XmlIgnore]
        public string[] Keys
        {
            get
            {
                if (Count == 0)
                    return new string[0];
                var answer = this.Select(i => i.Key).ToArray();
                return answer;
            }
        }

        [XmlIgnore]
        public Guid[] Ids
        {
            get
            {
                if (Count == 0)
                    return new Guid[0];
                var answer = this.Select(i => i.ID).ToArray();
                return answer;
            }
        }

        [XmlIgnore]
        public AssocItem this[string key]
        {
            get
            {
                lock (SyncRoot)
                {
                    var assocItem = this.FirstOrDefault(item => string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) == 0);
                    if (assocItem != null) return assocItem;

                    assocItem = new AssocItem(key);
                    Add(assocItem);
                    return assocItem;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    for (var index = 0; index < this.Count; index++)
                    {
                        var item = this[index];
                        if (string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) != 0) continue;
                        this[index] = value;
                        break;
                    }
                    Add(value);
                }
            }
        }

        public AssocArray()
        {
        }

        public AssocArray(string key, string value = "", Guid? id = null, string name = "", IAssocItem parent = null)
        {
            Key = key;
            Value = value;
            ID = id ?? Guid.NewGuid();
            Name = name;
            Parent = parent;
        }
        
        public string ToXml()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<AssocArray{Key.ToXmlAttribute("Key")}{Value.ToXmlAttribute("Value")}{this.ID.ToString("N").ToXmlAttribute("ID")}{Name.ToXmlAttribute("Name")}{(Count != 0 ? Count.ToString().ToXmlAttribute("Number") : "")}{Category.ToXmlAttribute("Category")}>");
            foreach (var item in this) 
                sb.AppendLine(item.ToXml());
            sb.AppendLine("</AssocArray>");
            return sb.ToString();
        }

        public string ToXml2()
        {
            return Xml.ToXml(this);
        }

        public void SaveXmlToFile(string path, bool easyToRead)
        {
            if (easyToRead)
                File.WriteAllText(path, Xml.ToXml(this));
            else
                Xml.SaveFile(path, this);
        }
    }
}