namespace MetX.Standard.Strings.Interfaces;

public interface IListLikeSerializeToXml<TParent, TChild> 
    where TParent : IListLikeSerializeToXml<TParent, TChild>, new()
    where TChild : new()
{
    void SaveXmlToFile(string path, bool easyToRead);
    byte[] ToBytes();
    string ToXml(bool removeNamespaces, bool normalizeRootNodeName);
    string ToJson();

}