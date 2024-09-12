using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings;

public abstract class ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey> 
    : IListLikeSerializeToXml<TSecondAxis, TItem>, 
      IEnumerable<TItem>,
      IAssocItem
    where TFirstAxis : class, new()
    where TSecondAxis : ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey>, new()
    where TItem : IAssocItem, new()
{

    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    [XmlArray(ElementName = "Items"), XmlArrayItem(ElementName = "Item")]
    public virtual List<TItem> Items { get; set; } = new();

    [XmlIgnore] public int Count => Items.Count;

    protected ListLikeSerializesToXml(string key = null, string value = null, string name = null, Guid? id = null)
    {
        ID = id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
        Value = value;
        Name = name;
    }


    public IEnumerator<TItem> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TItem item)
    {
        if (item == null)
            return;
        Items.Add(item);
    }

    public virtual TItem this[TKey key]
    {
        get
        {
            if (key == null)
                return default;

            var keyString = 
                key is string s
                    ? s
                    : key.ToString();

            foreach (var item in Items)
                if (string.Equals(keyString, item.Key, StringComparison.InvariantCultureIgnoreCase))
                    return item;

            return default;
        }
        set
        {
            if (key == null)
                return;

            var keyString = 
                key is string s
                    ? s
                    : key.ToString();

            for (var index = 0; index < Items.Count; index++)
            {
                var item = Items[index];
                if (!string.Equals(keyString, item.Key, StringComparison.InvariantCultureIgnoreCase)) continue;

                Items.Insert(index, value);
                Items.RemoveAt(index + 1);
            }
        }
    }


    public virtual void SaveXmlToFile<TConcreteType>(string path, bool easyToRead = false)
        where TConcreteType : class, new()
    {
        if (easyToRead)
        {
            File.WriteAllText(path, ToXml<TConcreteType>());
        }
        else
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }

            using var xtw = new XmlTextWriter(path, Encoding.UTF8);
            GetSerializer(typeof(TConcreteType), ExtraTypes<TConcreteType>())
                .Serialize(xtw, this);
        }
    }

    public virtual byte[] ToBytes<TConcreteType>()
        where TConcreteType : class, new()
    {
        var xml = ToXml<TConcreteType>();
        if(xml.IsEmpty()) return Array.Empty<byte>();
        return Encoding.UTF8.GetBytes(xml);
    }

    public string ToJson()
    {
        /*var xml = ToXml(true, false);
        return xml.IsNotEmpty()
            ? MetX.Standard.Strings.ConvertToJson.Xml(xml)
            : "";*/
        return "";
    }

    /// <summary>
    ///     Turns an object into an xml string
    /// </summary>
    /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
    /// <typeparam name="TConcreteType"></typeparam>
    /// <param name="toSerialize">The object to serialize</param>
    /// <param name="removeNamespaces"></param>
    /// <param name="normalizeRootNodeName"></param>
    /// <returns></returns>
    public virtual string ToXml<TConcreteType>(bool removeNamespaces, bool normalizeRootNodeName) where TConcreteType : class, new()
    {
        TConcreteType toSerialize = this as TConcreteType;
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
        var extraTypes = ExtraTypes<TConcreteType>();

        using (var xw = XmlWriter.Create(sb, settings))
        {
            GetSerializer(typeof(TConcreteType), extraTypes).Serialize(xw, toSerialize);
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

    public virtual string ToXml(bool removeNamespaces, bool normalizeRootNodeName)
    {
        var topLevelType = TopLevelType();
        var toSerialize = GetType();
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
        var extraTypes = ExtraTypes(toSerialize);

        using (var xw = XmlWriter.Create(sb, settings))
        {
            GetSerializer(toSerialize, extraTypes).Serialize(xw, toSerialize);
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

    public Type TopLevelType()
    {
        return GetType();
    }

    public static Type[] ExtraTypes<TConcreteType>(Type[] extraExtraTypes = null)
    {
        var extraTypes = new[]
        {
            typeof(TFirstAxis),
            typeof(TSecondAxis),
            typeof(TItem),
            typeof(TKey),
            typeof(TConcreteType),
            typeof(ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey>)
        }.ToList();

        extraTypes.AddRange(typeof(TSecondAxis).GenericTypeArguments);
        extraTypes.AddRange(typeof(TItem).GenericTypeArguments);
        extraTypes.AddRange(typeof(TKey).GenericTypeArguments);
        extraTypes.AddRange(typeof(TConcreteType).GenericTypeArguments);

        if (extraExtraTypes is {Length: > 0})
        {
            extraTypes.AddRange(extraExtraTypes);
        }

        return extraTypes.Distinct().ToArray();
    }

    public static Type[] ExtraTypes(Type concreteType, Type[] extraExtraTypes = null)
    {
        var extraTypes = new[]
        {
            typeof(TFirstAxis),
            typeof(TSecondAxis),
            typeof(TItem),
            typeof(TKey),
            concreteType,
            typeof(ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey>)
        }.ToList();

        extraTypes.AddRange(typeof(TSecondAxis).GenericTypeArguments);
        extraTypes.AddRange(typeof(TItem).GenericTypeArguments);
        extraTypes.AddRange(typeof(TKey).GenericTypeArguments);
        extraTypes.AddRange(concreteType.GenericTypeArguments);

        if (extraExtraTypes is {Length: > 0})
        {
            extraTypes.AddRange(extraExtraTypes);
        }

        return extraTypes.Distinct().ToArray();
    }

    public static TConcreteType FromXml<TConcreteType>(string xml)
    {
        if (!xml.Contains("<AssocArray"))
            xml = RewriteRootNodeName_TParentToAssocArray(xml);
        using var sr = new StringReader(xml);
        var parent = (TConcreteType) GetSerializer(typeof(TConcreteType), null).Deserialize(sr);
        return parent;
    }

    public static TConcreteClass LoadXmlFromFile<TConcreteClass>(string path)
        where TConcreteClass: ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey>
    {
        if (!File.Exists(path))
        {
            return null;
        }

        using var xtw = new XmlTextReader(path);

        var deserializedObject = 
            GetSerializer(
                    typeof(TConcreteClass), 
                    ExtraTypes<TConcreteClass>())
                .Deserialize(xtw) as TConcreteClass;

        return deserializedObject;
    }


    public static string RewriteRootNodeName_AssocArrayToTParent(string xml)
    {
        return xml;
        var targetNameOfRootElement = typeof(TFirstAxis).Name;
        if (targetNameOfRootElement != "AssocArray")
            xml = xml
                .Replace("<AssocArray", $"<{targetNameOfRootElement}")
                .Replace("</AssocArray", $"</{targetNameOfRootElement}");
        return xml;
    }

    public static string RewriteRootNodeName_TParentToAssocArray(string xml)
    {
        return xml;
        var targetNameOfRootElement = typeof(TFirstAxis).Name;
        if (targetNameOfRootElement != "AssocArray")
            xml = xml
                .Replace($"<{targetNameOfRootElement}", "<AssocArray")
                .Replace($"</{targetNameOfRootElement}", "</AssocArray");
        return xml;
    }

    public static TConcreteType FromBytes<TConcreteType>(byte[] bytes)
        where TConcreteType : ListLikeSerializesToXml<TFirstAxis, TSecondAxis, TItem, TKey>
    {
        var xml = Encoding.UTF8.GetString(bytes);
        var item = xml.IsNotEmpty()
            ? FromXml<TConcreteType>(xml)
            : default;
        return item;
    }

    public string ToXml<TConcreteType>()
        where TConcreteType : class, new()
    {
        return ToXml<TConcreteType>(true, true);
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

