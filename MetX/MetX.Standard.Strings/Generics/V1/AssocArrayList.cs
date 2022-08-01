using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MetX.Standard.XDString.Generics.V1;

[Serializable]
public class AssocArrayList<T> : AssocItem<T>
    where T : class, new()
{
    [XmlIgnore] public object SyncRoot = new();

    public SortedDictionary<string, AssocArray<T>> Items {get; set; }= new();

    public Func<string> KeyWhenThereIsNoKey { get; set; }
    public Func<string, string> ToAssocKey { get; set; }

    public AssocArray<T> this[string key]
    {
        get
        {
            lock (SyncRoot)
            {
                AssocArray<T> assocArray;
                var assocKey = ToAssocKey(key);
                if (!Items.ContainsKey(assocKey))
                {
                    assocArray = new AssocArray<T>(key);
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
                    var assocArray = new AssocArray<T>(key);
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