using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MetX.Standard.XDString.Interfaces;

namespace MetX.Standard.XDString.Generics.V1;

/// <summary>
/// AssocArray1D is like AssocArray but can be inherited and serialized to/from xml with top level attributes properly
/// </summary>
/// <typeparam name="TChild"></typeparam>
/// <typeparam name="TParent"></typeparam>
[Serializable]
[XmlRoot("AssocArray")]
public class AssocArray1D<TParent, TChild> 
    : ListLikeSerializesToXml<TParent, AssocArray1D<TParent, TChild>, AssocItem<TChild>, string, TChild> 
    where TParent : class
    where TChild : class, new() 
{
    [XmlAttribute] public string Key { get; set; }

    [XmlIgnore] public object SyncRoot { get; } = new();

    public AssocArray1D()
    {
    }

    public AssocArray1D(string key) 
    {
        Key = key;
    }

    [XmlIgnore]
    public string[] Values
    {
        get
        {
            if (Items.Count == 0)
                return Array.Empty<string>();
            var answer = Items.Select(i => i.Value).ToArray();
            return answer;
        }
    }

    [XmlIgnore]
    public int[] Numbers
    {
        get
        {
            if (Items.Count == 0)
                return Array.Empty<int>();
            var answer = Items.Select(i => i.Number).ToArray();
            return answer;
        }
    }

    [XmlIgnore]
    public string[] Names
    {
        get
        {
            if (Items.Count == 0)
                return Array.Empty<string>();
            var answer = Items.Select(i => i.Name).ToArray();
            return answer;
        }
    }

    [XmlIgnore]
    public string[] Keys
    {
        get
        {
            if (Items.Count == 0)
                return Array.Empty<string>();
            var answer = Items.Select(i => i.Key).ToArray();
            return answer;
        }
    }

    [XmlIgnore]
    public Guid[] Ids
    {
        get
        {
            if (Items.Count == 0)
                return Array.Empty<Guid>();
            var answer = Items.Select(i => i.ID).ToArray();
            return answer;
        }
    }

    public IAssocItem FirstKeyContaining(string toFind)
    {
        return Items.FirstOrDefault(i => i.Key.ToLower().Contains(toFind));
    }

    [XmlIgnore]
    public new AssocItem<TChild> this[string key]
    {
        get
        {
            lock (SyncRoot)
            {
                var assocItem = Items.FirstOrDefault(item => string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (assocItem != null) return assocItem;

                assocItem = new AssocItem<TChild>(key);
                Items.Add(assocItem);
                return assocItem;
            }
        }
        set
        {
            lock (SyncRoot)
            {
                for (var index = 0; index < Items.Count; index++)
                {
                    var item = Items[index];
                    if (string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) != 0) continue;
                    Items[index] = value;
                    break;
                }
                Items.Add(value);
            }
        }
    }

    public bool ContainsKey(string key)
    {
        lock (SyncRoot)
        {
            return Items.Any(i => string.Equals(i.Key, key, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var item in Items)
        {
            sb.AppendLine($"{item.Key}={item.Value}");
        }
        return sb.ToString();
    }

    public new static T FromTypedXml<T>(string xml) where T : class, new()
    {
        using var sr = new StringReader(xml);
        var xmlSerializer = GetSerializer(typeof(T), ExtraTypes());

        return xmlSerializer.Deserialize(sr) as T;
    }

}