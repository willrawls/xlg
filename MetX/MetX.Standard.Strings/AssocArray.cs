using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;
using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Strings;

[Serializable]
[XmlRoot("AssocArray")]
public class AssocArray : ListLikeSerializesToXml<AssocArray, AssocArray, BasicAssocItem, string>
{
    [XmlIgnore] public object SyncRoot { get; } = new();
    [XmlIgnore] public bool AutoPersist { get; set; }
    [XmlIgnore] public string FilePath { get; set; }

    public AssocArray() : base()
    {
    }

    public AssocArray(string key = null, string value = null, string name = null, Guid? id = null) : base(key, value, name, id)
    {
    }

    public static bool DefaultKeyComparer(string keyOrName, TimeTrackingAssocItem item)
    {
        if (string.IsNullOrEmpty(keyOrName) || item == null) return false;

        return item.Key.Equals(keyOrName) || item.Name.Equals(keyOrName);
                
    }

    public AssocArray(string key) : base(key)
    {
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
    public override BasicAssocItem this[string key]
    {
        get
        {
            lock (SyncRoot)
            {
                var assocItem = Items.FirstOrDefault(item => string.Compare(item.Key, key, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (assocItem != null) return assocItem;

                assocItem = new BasicAssocItem(key);
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
                    HandleAutoPersist();
                    break;
                }
                Items.Add(value);
            }
        }
    }

    public void HandleAutoPersist()
    {
        if (!AutoPersist || FilePath.IsEmpty())
            return;

        Save();
    }

    public  void Save()
    {
        SaveXmlToFile<AssocArray>(FilePath, true);
    }

    public void SaveXmlToFile(string path, bool easyToRead = false)
    {
        base.SaveXmlToFile<AssocArray>(path, easyToRead);
    }

    public static AssocArray Load(string filePath = "", bool autoPersist = false)
    {
        if (filePath.IsEmpty() || !File.Exists(filePath))
        {
            return new AssocArray
            {
                FilePath = ""
            };
        }

        AssocArray ret;
        if (!File.Exists(filePath)) ret = default(AssocArray);
        else
        {
            var xml = File
                .ReadAllText(filePath)
                .Replace("<ListLikeSerializesToXmlOfAssocArrayAssocItemStringString", "<AssocArray")
                .Replace("</ListLikeSerializesToXmlOfAssocArrayAssocItemStringString", "</AssocArray")
                ;

            using var stringReader = new StringReader(xml);
            using var xmlTextReader = new XmlTextReader(stringReader);
            ret = (AssocArray) GetSerializer(typeof(AssocArray), ExtraTypes<AssocArray>()).Deserialize(xmlTextReader);
        }

        var aa = ret;
        aa.FilePath = filePath;
        aa.AutoPersist = autoPersist;

        return aa;
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

    public string Resolve(string target)
    {
        if (target.IsEmpty()) return "";
        if (!target.Contains("%")) return target;

        var result = Items
            .Aggregate(target, 
                (current, item) => current
                    .Replace($"%{item.Key}%", item.Value));
        return result;
    }
}