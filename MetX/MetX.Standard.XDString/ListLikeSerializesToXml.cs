using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;
using MetX.Standard.XDString.Support;

namespace MetX.Standard.XDString;

public abstract class ListLikeSerializesToXml<TTop, TParent, TChild, TKey, TActual> :
    IListLikeSerializeToXml<TParent, TChild>
    where TParent : ListLikeSerializesToXml<TTop, TParent, TChild, TKey, TActual>, new()
    where TChild : new()
    where TTop : class
{
    protected ListLikeSerializesToXml(Func<TKey, TChild, bool> keyComparer)
    {
        KeyComparer = keyComparer;
    }

    [XmlArray(ElementName = "Items")]
    [XmlArrayItem(ElementName = "Item")]
    public virtual List<TChild> Items { get; set; } = new();

    [XmlIgnore] public int Count => Items.Count;

    [XmlIgnore] public Func<TKey, TChild, bool> KeyComparer { get; set; }


    public virtual TChild this[TKey key]
    {
        get
        {
            foreach (var item in Items)
                if (KeyComparer(key, item))
                    return item;

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

    public virtual void SaveXmlToFile(string path, bool easyToRead)
    {
        if (easyToRead)
        {
            File.WriteAllText(path, ToXml());
        }
        else
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }

            using var xtw = new XmlTextWriter(path, Encoding.UTF8);
            GetSerializer(typeof(TTop), ExtraTypes())
                .Serialize(xtw, this);
        }
    }

    public virtual byte[] ToBytes()
    {
        var xml = ToXml();
        return xml.IsEmpty()
            ? Array.Empty<byte>()
            : Encoding.UTF8.GetBytes(xml);
    }

    public string ToJson()
    {
        var xml = ToXml(true, false);
        return xml.IsNotEmpty()
            ? ConvertToJson.Xml(xml)
            : "";
    }

    /// <summary>
    ///     Turns an object into an xml string
    /// </summary>
    /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
    /// <param name="toSerialize">The object to serialize</param>
    /// <param name="removeNamespaces"></param>
    /// <param name="normalizeRootNodeName"></param>
    /// <returns></returns>
    public virtual string ToXml(bool removeNamespaces, bool normalizeRootNodeName)
    {
        TTop toSerialize = this as TTop;
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
        var extraTypes = ExtraTypes();

        using (var xw = XmlWriter.Create(sb, settings))
        {
            GetSerializer(typeof(TTop), extraTypes).Serialize(xw, toSerialize);
        }

        if (!removeNamespaces) return sb.ToString();

        sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);
        sb.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
        sb.Replace(" xsi:nil=\"true\"", string.Empty);

        if (extraTypes == null || extraTypes.Length == 0) 
            return sb.ToString();

        foreach (var extraType in extraTypes!)
        {
            sb.Replace($" xsi:type=\"{extraType.Name}\"", string.Empty);
            sb.Replace($" xsi:type=\"{extraType.Name.FirstToken("`")}\"", string.Empty);
        }

        if (normalizeRootNodeName) return RewriteRootNodeName_AssocArrayToTParent(sb.ToString());

        return sb.ToString();
    }

    public static Type[] ExtraTypes(Type[] extraExtraTypes = null)
    {
        
        var extraTypes = new[]
        {
            typeof(TTop),
            typeof(TParent),
            typeof(TChild),
            typeof(TKey),
            typeof(TActual),
            typeof(ListLikeSerializesToXml<TTop, TParent, TChild, TKey, TActual>)
        }.ToList();

        extraTypes.AddRange(typeof(TParent).GenericTypeArguments);
        extraTypes.AddRange(typeof(TChild).GenericTypeArguments);
        extraTypes.AddRange(typeof(TKey).GenericTypeArguments);
        extraTypes.AddRange(typeof(TActual).GenericTypeArguments);

        if (extraExtraTypes is {Length: > 0})
        {
            extraTypes.AddRange(extraExtraTypes);
        }

        return extraTypes.Distinct().ToArray();
    }

    public static TTop FromXml(string xml)
    {
        if (!xml.Contains("<AssocArray"))
            xml = RewriteRootNodeName_TParentToAssocArray(xml);
        using var sr = new StringReader(xml);
        var parent = (TTop) GetSerializer(typeof(TTop), null).Deserialize(sr);
        return parent;
    }

    public static string RewriteRootNodeName_AssocArrayToTParent(string xml)
    {
        return xml;
        var targetNameOfRootElement = typeof(TTop).Name;
        if (targetNameOfRootElement != "AssocArray")
            xml = xml
                .Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }

    public static string RewriteRootNodeName_TParentToAssocArray(string xml)
    {
        return xml;
        var targetNameOfRootElement = typeof(TTop).Name;
        if (targetNameOfRootElement != "AssocArray")
            xml = xml
                .Replace($"<{targetNameOfRootElement}", "<AssocArray")
                .Replace($"</{targetNameOfRootElement}", "</AssocArray");
        return xml;
    }

    public static TTop FromBytes(byte[] bytes)
    {
        var xml = Encoding.UTF8.GetString(bytes);
        var aa = !string.IsNullOrEmpty(xml)
            ? FromXml(xml)
            : default;
        return aa as TTop;
    }

    public string ToXml()
    {
        return ToXml(true, true);
    }

    /// <summary>
    ///     Returns a XmlSerializer for the given type. Repeated calls pull the serializer previously used. Serializers are
    ///     stored internally in a sorted list for quick retrieval.
    /// </summary>
    /// <param name="type">The type to return a XmlSerializer for</param>
    /// <param name="extraTypes"></param>
    /// <returns>The XmlSerializer for the type</returns>
    public static XmlSerializer GetSerializer(Type type, Type[] extraTypes)
    {
        if (type.FullName == null) return null;

        var xmlSerializer = extraTypes is {Length: > 0}
            ? new XmlSerializer(type, extraTypes)
            : new XmlSerializer(type);

        return xmlSerializer;
    }

}