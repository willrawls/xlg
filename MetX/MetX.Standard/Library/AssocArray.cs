using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocArray : AssocItem
    {
        [XmlIgnore] public object SyncRoot { get; }= new();
        public SortedDictionary<string, AssocItem> Pairs = new();

        public AssocArray() { }
        public AssocArray(string key, string value = "", Guid? id = null, string name = "", IAssocItem parent = null) 
            : base(key, value, id, name, parent) { }

        public AssocItem this[string key]
        {
            get
            {
                lock(SyncRoot)
                {
                    AssocItem assocItem;
                    var assocKey = key.ToAssocKey();
                    if (!Pairs.ContainsKey(assocKey))
                    {
                        assocItem = new AssocItem(key, "");
                        Pairs.Add(assocKey, assocItem);
                    }
                    else
                        assocItem = Pairs[assocKey];
                    return assocItem;
                }
            }
            set
            {
                lock(SyncRoot)
                {
                    var assocKey = key.ToAssocKey();
                    if (!Pairs.ContainsKey(assocKey))
                        Pairs.Add(assocKey, value);
                    else
                        Pairs[assocKey] = value;
                }
            }
        }

        public new string ToText(int indent = 0)
        {
            var sb = new StringBuilder();
            var indentation = indent == 0 ? "" : new string(' ', indent);
            lock (SyncRoot)
            {
                sb.AppendLine(base.ToText(indent));
                foreach (var pair in Pairs)
                {
                    sb.AppendLine(pair.Value.ToText(indent + 1));
                }
            }
            return sb.ToString();
        }
    }
}