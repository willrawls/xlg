using System;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings;

[Serializable]
public class AssocItemWithChildren : BasicAssocItem
{
    public AssocArray Children = new();

    public AssocItemWithChildren(string key, string value = null, Guid? id = null, string name = null, string category = null) : base(key, value, id, name, category)
    {
    }

    public AssocItemWithChildren()
    {
    }
}

[Serializable]
public class BasicAssocItem : IAssocItem
{
    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public BasicAssocItem(string key, string value = null, Guid? id = null, string name = null, string category = null)
    {
        Value = value;
        Name = name;
        Category = category;
        ID = id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
    }

    public BasicAssocItem()
    {
        ID = Guid.NewGuid();
        Key = ID.ToString("N");
        
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"      {Key}={Value ?? "nil"}, {ID:N}, {Name ?? "nil"}");
        return sb.ToString();
    }
}

