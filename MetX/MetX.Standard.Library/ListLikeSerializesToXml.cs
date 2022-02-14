using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Library;

public abstract class ListLikeSerializesToXml<TParent, TChild, TKey, TActual> : 
    IListLikeSerializeToXml<TParent, TChild>
    where TParent : ListLikeSerializesToXml<TParent, TChild, TKey, TActual>, new()
    where TChild : new()
{
    [XmlArray(ElementName = "Items")]
    [XmlArrayItem(ElementName = "Item")]
    public virtual List<TChild> Items { get; set; } = new();

    [XmlIgnore]
    public virtual int Count => Items.Count;

    [XmlIgnore]
    public Func<TKey, TChild, bool> KeyComparer { get; set; }

    protected ListLikeSerializesToXml(Func<TKey, TChild, bool> keyComparer)
    {
        KeyComparer = keyComparer;
    }


    public virtual TChild this[TKey key]
    {
        get
        {
            foreach (var item in Items)
            {
                if (KeyComparer(key, item))
                    return item;
            }

            return default;
        }
        set
        {
            for (var index = 0; index < Items.Count; index++)
            {
                var item = Items[index];
                if (!KeyComparer(key, item)) continue;

                Items.Insert(index, value);
                Items.RemoveAt(index + 1);
            }

        }
    }

    private static readonly Type[] ExtraTypes = new Type[]
    {
        typeof(TParent),
        typeof(TChild),
        typeof(TKey),
        typeof(TActual),
    }.ToList().Distinct().ToArray();
    
    public static TActual FromXml(string xml)
    {
        return Xml.FromXml<TActual>(xml, ExtraTypes);
    }

    public static TActual LoadXmlFromFile(string path)
    {
        return Xml.LoadFile<TActual>(path, ExtraTypes);
    }

    public virtual void SaveXmlToFile(string path, bool easyToRead)
    {
        var t = this;
        if (easyToRead)
            File.WriteAllText(path, Xml.ToXml(t, true, ExtraTypes));
        else
            Xml.SaveFile(path, t, ExtraTypes);
    }

    public virtual string ToXml()
    {
        var t = (TParent) this;
        var xml = Xml.ToXml(t, true, ExtraTypes);
        var targetNameOfRootElement = typeof(TActual).Name;
        if(targetNameOfRootElement != "AssocArray")
            xml = xml.Replace("<AssocArray", $"<{targetNameOfRootElement}")
                     .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }

    public virtual byte[] ToBytes()
    {
        var xml = ToXml();
        return xml.IsEmpty()
            ? Array.Empty<byte>()
            : Encoding.ASCII.GetBytes(xml);
    }

    public static TActual FromBytes(byte[] bytes)
    {
        var xml = Encoding.ASCII.GetString(bytes);
        return xml.IsNotEmpty()
            ? FromXml(xml)
            : default;
    }
}