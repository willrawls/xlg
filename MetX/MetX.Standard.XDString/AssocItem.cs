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
    private string _value;
    private int _number;
    private string _name;
    private string _category;

    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    
    [XmlAttribute]
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            Modified = DateTime.Now;
        }
    }

    [XmlAttribute]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Modified = DateTime.Now;
        }
    }

    [XmlAttribute]
    public int Number
    {
        get => _number;
        set
        {
            _number = value;
            Modified = DateTime.Now;
        }
    }

    [XmlAttribute]
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            Modified = DateTime.Now;
        }
    }

    [XmlAttribute] public DateTime At { get; set; }
    [XmlAttribute] public DateTime Modified { get; set; }

    public AssocItem()
    {
        ID = Guid.NewGuid();
        Key = ID.ToString("N");
        At = DateTime.Now;
    }
    public AssocItem(string key, string value = "", Guid? id = null, string name = null)
    {
        Value = value ?? "";
        Name = name ?? "";

        ID = id == Guid.Empty 
            ? Guid.NewGuid() 
            : id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
        At = DateTime.Now;
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
        sb.AppendLine(    $"{indentation}At:       {At:G}");
        if(Modified != DateTime.MinValue)
            sb.AppendLine($"{indentation}Modified: {Modified:G}");
        return sb.ToString();
    }
}