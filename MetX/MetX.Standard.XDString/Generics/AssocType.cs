using System;

namespace MetX.Standard.XDString.Generics
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