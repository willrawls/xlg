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
        public Guid Id { get; set; }

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
            Id = id ?? Guid.NewGuid();
            Name = name;
            Parent = parent;
        }

        /*
        public string ToText(int indent = 0)
        {
            var sb = new StringBuilder();
            var indentation = indent == 0 ? "" : new string(' ', indent);
            lock (SyncRoot)
            {
                sb.AppendLine(base.ToText(indent));
                foreach (var pair in Pairs) sb.AppendLine(pair.Value.ToText(indent + 1));
            }

            return sb.ToString();
        }
    */
    }
}