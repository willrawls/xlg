using System;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocItem : IAssocItem
    {
        public string Key { get; }

        public IAssocItem Parent { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }

        public AssocItem() { }
        public AssocItem(string key, string value = "", Guid? id = null, string name = null, IAssocItem parent = null)
        {
            Parent = parent;
            Key = key.IsEmpty() ? AssocSupport.KeyWhenThereIsNoKey() : key;
            Value = value ?? "";
            Name = name ?? "";
            Id = id ?? Guid.NewGuid();
        }
    }
}