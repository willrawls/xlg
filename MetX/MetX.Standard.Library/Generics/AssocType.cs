using System;
using System.Xml.Serialization;

namespace MetX.Standard.Library.Generics
{
    [Serializable]
    public class AssocType<T>
    {
        public T Target { get; set; }

        public AssocType()
        {
        }

        public AssocType(T target)
        {
            Target = target;
        }
    }
}