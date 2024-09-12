namespace MetX.Standard.Strings.Interfaces;

public interface IListLikeSerializeToXml<TParent, TChild> 
    where TParent : IListLikeSerializeToXml<TParent, TChild>, new()
    where TChild : new()
{
    void SaveXmlToFile<TConcreteType>(string path, bool easyToRead)
        where TConcreteType : class, new();

    byte[] ToBytes<TConcreteType>()
        where TConcreteType : class, new ();

    string ToXml<TConcreteType>(bool removeNamespaces, bool normalizeRootNodeName)
        where TConcreteType : class, new();
    
    string ToJson();

}

public interface ISerializeToXml<TItem> 
    where TItem : ISerializeToXml<TItem>, new()
{
    void SaveXmlToFile(string path, bool easyToRead);

    byte[] ToBytes();

    string ToXml(bool removeNamespaces, bool normalizeRootNodeName);

    string ToJson();

}