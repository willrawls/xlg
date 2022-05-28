using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MetX.Standard.XDString;

[Serializable]
public class AssocArrayList : List<AssocArray>
{
    [XmlIgnore]
    public object SyncRoot { get; }= new();

    public AssocArray this[string key]
    {
        get
        {
            lock(SyncRoot)
            {
                var assocArray = this.FirstOrDefault(item =>
                    string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (assocArray != null) return assocArray;

                assocArray = new AssocArray(key, this);
                Add(assocArray);
                return assocArray;
            }
        }
        set
        {
            lock (SyncRoot)
            {
                for (var index = 0; index < Count; index++)
                {
                    var item = this[index];
                    if (string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) != 0) continue;
                    this[index] = value;
                    return;
                }
                Add(value);
            }
        }
    }

}