using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Standard.XDString.Generics
{
    [Serializable]
    public class AssocArray<T> : List<AssocItem<T>>, IAssocItem where T : class, new()
    {
        public string Key { get; }
        public IAssocItem Parent { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }

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

        public Guid[] IDs
        {
            get
            {
                if (Count == 0)
                    return new Guid[0];
                var answer = this.Select(i => i.ID).ToArray();
                return answer;
            }
        }

        public T[] Items
        {
            get
            {
                if (Count == 0)
                    return new T[0];

                var answer = this.Select(i => i.Item).ToArray();
                return answer;
            }
        }

        [XmlIgnore] public object SyncRoot = new();

        public AssocArray() { }

        public AssocArray(string key, string value = null, Guid? id = null, string name = null, IAssocItem parent = null)
        {
            Key = key;
            Value = value;
            ID = id ?? Guid.NewGuid();
            Name = name;
            Parent = parent;
        }

        public AssocItem<T> this[string key]
        {
            get
            {
                lock (SyncRoot)
                {
                    var assocItem = this.FirstOrDefault(i => string.Equals(i.Key, key, StringComparison.InvariantCultureIgnoreCase));
                    if (assocItem == null)
                    {
                        assocItem = new AssocItem<T>(key);
                        Add(assocItem);
                    }
                    return assocItem;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    for (var i = 0; i < Count; i++)
                    {
                        var assocItem = this[i];
                        if (assocItem.Key == key)
                        {
                            this[i] = value;
                            return;
                        }
                    }
                    Add(value);
                }
            }
        }

        public bool ContainsKey(string key)
        {
            lock (SyncRoot)
            {
                return this.Any(i => string.Equals(i.Key, key, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"  {Key}={Value ?? "nil"}, {ID:N}, {Name ?? "nil"}");
            foreach (var item in this)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((AssocArray<T>)obj);
        }

        protected bool Equals(AssocArray<T> other)
        {
            return string.Equals(Key, other.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Key != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Key) : 0);
        }
    }
}