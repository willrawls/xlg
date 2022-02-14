namespace MetX.Standard.Library;

public interface IListLikeSerializeToXml<TParent, TChild> 
    where TParent : IListLikeSerializeToXml<TParent, TChild>, new()
    where TChild : new()
{
    //string Name { get; set; }
    void SaveXmlToFile(string path, bool easyToRead);
    string ToXml();
    byte[] ToBytes();
    string ToJson();

}