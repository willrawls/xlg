using System;
using System.Text;
using System.Xml.Serialization;

namespace MetX.Standard.Library
{
    [Serializable]
    public class AssocItem : IAssocItem
    {
        [XmlAttribute] public string Key { get; }
        [XmlAttribute] public string Value { get; set; }
        [XmlAttribute] public string Name { get; set; }
        [XmlAttribute] public Guid Id { get; set; }

        [XmlIgnore]
        public IAssocItem Parent { get; set; }

        public AssocItem() { }
        public AssocItem(string key, string value = "", Guid? id = null, string name = null, IAssocItem parent = null)
        {
            Parent = parent;
            Key = key.IsEmpty() ? AssocSupport.KeyWhenThereIsNoKey() : key;
            Value = value ?? "";
            Name = name ?? "";

            if(id.HasValue)
            {
                if (id.Value == Guid.Empty)
                    Id = Guid.NewGuid();
                Id = id.Value;
            }
            else
            {
                Id = Guid.NewGuid();
            }
        }

        public virtual string ToText(int indent = 0)
        {
            var indentation = indent == 0 ? "" : new string(' ', indent);
            var sb = new StringBuilder();

            sb.AppendLine($"{indentation}Key:   {Key}");
            sb.AppendLine($"{indentation}Value: {Value}");
            sb.AppendLine($"{indentation}Name:  {Name}");
            sb.AppendLine($"{indentation}ID:    {Id:N}");
            return sb.ToString();
        }

    }
}