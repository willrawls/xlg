namespace MetX.Standard.Library.ML;

public interface IListLikeSerializeToXml<TParent, TChild> 
    where TParent : IListLikeSerializeToXml<TParent, TChild>, new()
    where TChild : new()
{
    void SaveXmlToFile(string path, bool easyToRead);
    byte[] ToBytes();
    string ToXml();
    string ToJson();

}