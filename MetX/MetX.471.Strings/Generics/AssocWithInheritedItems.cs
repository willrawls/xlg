﻿using MetX._471.Strings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MetX._471.Strings.Generics;

[Serializable]
public class AssocWithInheritedItems<T> : List<T>, IAssocItem 
    where T : IAssocItem, new()
{
    [XmlAttribute] public string Key { get; set; }
    [XmlAttribute] public string Value { get; set; }
    [XmlAttribute] public string Name { get; set; }
    [XmlAttribute] public Guid ID { get; set; }
    [XmlAttribute] public int Number { get; set; }
    [XmlAttribute] public string Category { get; set; }

    public AssocWithInheritedItems()
    {
        ID = Guid.NewGuid();
        Key = ID.ToString("N");
    }

    public AssocWithInheritedItems(string key, string value = null, string name = null, Guid? id = null)
    {
        Value = value;
        Name = name;
        ID = id ?? Guid.NewGuid();
        Key = key ?? ID.ToString("N");
    }

    public string[] Values()
    {
        if (Count == 0)
            return Array.Empty<string>();
        var answer = this.Select(i => i.Value).ToArray();
        return answer;
    }

    public string[] Names()
    {
        if (Count == 0)
            return Array.Empty<string>();
        var answer = this.Select(i => i.Name).ToArray();
        return answer;
    }

    public string[] Keys()
    {
        if (Count == 0)
            return Array.Empty<string>();
        var answer = this.Select(i => i.Key).ToArray();
        return answer;
    }

    public Guid[] IDs()
    {
        if (Count == 0)
            return Array.Empty<Guid>();
        var answer = this.Select(i => i.ID).ToArray();
        return answer;
    }

    [XmlIgnore] public object SyncRoot = new();

    public T this[string key]
    {
        get
        {
            lock (SyncRoot)
            {
                var assocItem = this.FirstOrDefault(i => string.Equals(i.Key, key, StringComparison.InvariantCultureIgnoreCase));
                if (assocItem != null) return assocItem;

                assocItem = new T
                {
                    Key = key
                };
                Add(assocItem);
                return assocItem;
            }
        }
        set
        {
            lock (SyncRoot)
            {
                for (var i = 0; i < Count; i++)
                {
                    var assocItem = this[i];
                    if (assocItem.Key == key)
                    {
                        this[i] = value;
                        return;
                    }
                }
                Add(value);
            }
        }
    }

    public T this[Guid id]
    {
        get
        {
            lock (SyncRoot)
            {
                var assocItem = this.FirstOrDefault(i => i.ID == id);
                if (assocItem != null) return assocItem;

                assocItem = new T{ ID = id };
                Add(assocItem);
                return assocItem;
            }
        }
        set
        {
            lock (SyncRoot)
            {
                for (var i = 0; i < Count; i++)
                {
                    var assocItem = this[i];
                    if (assocItem.ID != id) continue;

                    this[i] = value;
                    return;
                }
                Add(value);
            }
        }
    }

    public bool ContainsKey(string key)
    {
        lock (SyncRoot)
        {
            return this.Any(i => string.Equals(i.Key, key, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"  {Key}={Value ?? "nil"}, {ID:N}, {Name ?? "nil"}");
        foreach (var item in this)
        {
            sb.AppendLine(item.ToString());
        }
        return sb.ToString();
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((AssocWithInheritedItems<T>)obj);
    }

    protected bool Equals(AssocWithInheritedItems<T> other)
    {
        return string.Equals(Key, other.Key, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return (Key != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Key) : 0);
    }
}