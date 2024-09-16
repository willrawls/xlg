using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings.ML;

public abstract class ListSerializesToXml<TParent, TChild> : List<TChild>, IListSerializeToXml<TParent, TChild>,
    IAssocItem
    where TParent : ListSerializesToXml<TParent, TChild>, new()
    where TChild : new()
{
    private static readonly Type[] ExtraTypes =
    {
        typeof(TParent),
        typeof(TChild)
    };

    public static TParent FromBytes(byte[] bytes)
    {
        var xml = Encoding.ASCII.GetString(bytes);
        return xml.IsEmpty()
            ? default
            : FromXml(xml);
    }

    public static TParent FromXml(string xml)
    {
        return Xml.FromXml<TParent>(xml, ExtraTypes);
    }

    public static TParent LoadXmlFromFile(string path)
    {
        return Xml.LoadFile<TParent>(path);
    }

    public string Key { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public Guid ID { get; set; }
    public int Number { get; set; }
    public string Category { get; set; }

    public virtual void SaveXmlToFile(string path, bool easyToRead)
    {
        var t = this;
        if (easyToRead)
            File.WriteAllText(path, Xml.ToXml(t, true, ExtraTypes));
        else
            Xml.SaveFile(path, t);
    }

    public virtual string ToXml()
    {
        var t = (TParent) this;
        return Xml.ToXml(t, true, ExtraTypes);
    }

    public virtual byte[] ToBytes()
    {
        var xml = ToXml();
        return xml.IsEmpty()
            ? Array.Empty<byte>()
            : Encoding.ASCII.GetBytes(xml);
    }
}