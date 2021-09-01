namespace MetX.Standard.Library.Generics
{
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