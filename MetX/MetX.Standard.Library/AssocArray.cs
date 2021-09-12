using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocArray : List<AssocItem>, IAssocItem
    {
        [XmlIgnore] public object SyncRoot { get; } = new();

        public string Key { get; set; }
        public IAssocItem Parent { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
        public string Category { get; set; }
        
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"<AssocArray{Key.ToXmlAttribute("Key")}{Value.ToXmlAttribute("Value")}{this.ID.ToString("N").ToXmlAttribute("ID")}{Name.ToXmlAttribute("Name")}{(Count != 0 ? Count.ToString().ToXmlAttribute("Count") : "")}{Category.ToXmlAttribute("Category")}>");
            foreach (var item in this)
            {
                sb.AppendLine(item.ToXml());
            }
            sb.AppendLine("</AssocArray>");
            return sb.ToString();
        }
    }
}