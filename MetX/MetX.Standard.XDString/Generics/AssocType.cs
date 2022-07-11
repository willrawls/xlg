using System;

namespace MetX.Standard.XDString.Generics;

[Serializable]
public class AssocType<T> : AssocItem
{
    public T Target { get; set; }

    public AssocType()
    {
    }

    public AssocType(T target)
    {
        Target = target;
    }

    public static AssocType<T> Wrap(T target) => new AssocType<T>(target);

}