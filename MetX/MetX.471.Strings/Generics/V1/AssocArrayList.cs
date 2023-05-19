using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace MetX._471.Strings.Generics.V1;

[Serializable]
public class AssocArrayList<T> : AssocItemOfT<T>
    where T : class, new()
{
    [XmlIgnore] public object SyncRoot = new();

    public SortedDictionary<string, AssocArrayOfT<T>> Items {get; set; }= new();

    public Func<string> KeyWhenThereIsNoKey { get; set; }
    public Func<string, string> ToAssocKey { get; set; }

    public AssocArrayOfT<T> this[string key]
    {
        get
        {
            lock (SyncRoot)
            {
                AssocArrayOfT<T> assocArray;
                var assocKey = ToAssocKey(key);
                if (!Items.ContainsKey(assocKey))
                {
                    assocArray = new AssocArrayOfT<T>(key);
                    Items.Add(assocKey, assocArray);
                }
                else
                {
                    assocArray = Items[assocKey];
                }

                return assocArray;
            }
        }
        set
        {
            lock (SyncRoot)
            {
                var assocKey = ToAssocKey(key);
                if (!Items.ContainsKey(assocKey))
                {
                    var assocArray = new AssocArrayOfT<T>(key);
                    Items.Add(assocKey, assocArray);
                }
                else
                {
                    Items[assocKey] = value;
                }
            }
        }
    }

    public AssocArrayList(string key, 
        Func<string, string> toAssocKey = null,
        Func<string> keyWhenThereIsNoKey = null) 
        : base(key)
    {
        ToAssocKey = toAssocKey ?? DefaultToAssocKey;
        KeyWhenThereIsNoKey = keyWhenThereIsNoKey ?? (() => "CetasList");
    }

    private string DefaultToAssocKey(string target)
    {
        if (!string.IsNullOrEmpty(target)) return target;

        var defaultKey = KeyWhenThereIsNoKey();
        return string.IsNullOrEmpty(defaultKey)
            ? $"{target.ToLowerInvariant()}_aak" 
            : defaultKey;
    }
}