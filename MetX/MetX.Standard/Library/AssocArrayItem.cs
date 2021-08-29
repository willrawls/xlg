using System;

namespace MetX.Standard.Library
{
    public class AssocArrayItem
    {
        public AssocArray Parent;
        public readonly string Key;
        public string Item;
        public string Name;
        public Guid Id;

        public AssocArrayItem(AssocArray parent, string key, string item, Guid? id = null, string name = null)
        {
            Parent = parent;
            Key = key;
            Item = item;
            Name = name;
            if (id.HasValue)
                Id = id.Value;
            else
                Id = Guid.NewGuid();
        }

        public AssocArrayItem(string key)
        {
            Key = key;
            Item = string.Empty;
        }
    }

    public class AssocArrayItem<T> where T : class, new()
    {
        public readonly string Key;
        public T Item;

        public AssocArrayItem(string key, T item)
        {
            Key = key;
            Item = item;
        }

        public AssocArrayItem(string key)
        {
            Key = key;
            Item = new T();
        }
    }
}