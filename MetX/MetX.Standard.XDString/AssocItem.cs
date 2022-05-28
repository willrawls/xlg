using System;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString;

[Serializable]
public class AssocItem : IAssocItem
{
    [XmlIgnore]
    public IAssocItem Parent { get; set; }

    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }

    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public AssocItem() { }
    public AssocItem(string key, string value = "", Guid? id = null, string name = null, IAssocItem parent = null)
    {
        Parent = parent;
        Key = string.IsNullOrEmpty(key) ? "CetasItem" : key;
        Value = value ?? "";
        Name = name ?? "";

        if(id.HasValue)
        {
            if (id.Value == Guid.Empty)
                ID = Guid.NewGuid();
            ID = id.Value;
        }
        else
        {
            ID = Guid.NewGuid();
        }
    }

    public virtual string ToText(int indent = 0)
    {
        var indentation = indent == 0 ? "" : new string(' ', indent);
        var sb = new StringBuilder();

        sb.AppendLine($"{indentation}Key:   {Key}");
        sb.AppendLine($"{indentation}Value: {Value}");
        sb.AppendLine($"{indentation}Name:  {Name}");
        sb.AppendLine($"{indentation}ID:    {ID:N}");
        return sb.ToString();
    }

    public string ToXml()
    {
        return $"    <AssocItem{Key.ToXmlAttribute("Key")}{Value.ToXmlAttribute("Value")}{this.ID.ToString("N").ToXmlAttribute("ID")}{(Number != 0 ? Number.ToString().ToXmlAttribute("Number") : "")}{Name.ToXmlAttribute("Name")}{Category.ToXmlAttribute("Category")} />";
    }
}