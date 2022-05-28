using System;

namespace MetX.Standard.XDString.Interfaces
{
    public interface IAssocItem
    {
        public string Key { get; }

        public IAssocItem Parent { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }

    public interface IAssocItem<T> : IAssocItem
    {
        T Item { get; set; }
    }
}