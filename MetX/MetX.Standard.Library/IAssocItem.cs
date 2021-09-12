using System;

namespace MetX.Standard.Library
{
    public interface IAssocItem
    {
        public string Key { get; }

        public IAssocItem Parent { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
    }

    public interface IAssocItem<T> : IAssocItem
    {
        T Item { get; set; }
    }
}