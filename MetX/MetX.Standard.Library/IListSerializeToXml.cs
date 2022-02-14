namespace MetX.Standard.Library;

public interface IListSerializeToXml<TParent, TChild> 
    where TParent : ListSerializesToXml<TParent, TChild>, new()
    where TChild : new()
{
    //string Name { get; set; }
    void SaveXmlToFile(string path, bool easyToRead);
    string ToXml();
    byte[] ToBytes();
}