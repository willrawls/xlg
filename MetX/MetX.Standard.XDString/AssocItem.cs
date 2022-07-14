using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString;


[Serializable]
public class AssocItem : IAssocItem
{
    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }

    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public AssocItem()
    {
        ID = Guid.NewGuid();
        Key = ID.ToString("N");
    }
    public AssocItem(string key, string value = "", Guid? id = null, string name = null)
    {
        Value = value ?? "";
        Name = name ?? "";

        ID = id == Guid.Empty 
            ? Guid.NewGuid() 
            : id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
    }

    public virtual string ToText(int indent = 0)
    {
        var indentation = indent == 0 ? "" : new string(' ', indent);
        var sb = new StringBuilder();

        sb.AppendLine($"{indentation}Key:   {Key}");
        if(ID != Guid.Empty) 
            sb.AppendLine($"{indentation}ID:       {ID:N}");
        if(Category.IsNotEmpty())
            sb.AppendLine($"{indentation}Category: {Category}");
        if(Name.IsNotEmpty())
            sb.AppendLine($"{indentation}Name:     {Name}");
        if(Value.IsNotEmpty()) 
            sb.AppendLine($"{indentation}Value:    {Value}");
        if(this.Number != 0)
            sb.AppendLine($"{indentation}Number:   {Number}");
        return sb.ToString();
    }
}