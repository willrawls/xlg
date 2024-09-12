namespace MetX.Standard.Library.ML
{
    public interface ISerializeToXml<T> where T : SerializesToXml<T>, new()
    {
        void SaveXmlToFile(string path, bool easyToRead);
        string ToXml();
        byte[] ToBytes();
    }
}