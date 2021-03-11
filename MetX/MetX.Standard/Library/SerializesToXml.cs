using System.IO;
using System.Text;

namespace MetX.Standard.Library
{
    public abstract class SerializesToXml<T>
        where T : SerializesToXml<T>, new()
    {
        public static T FromXml(string xml)
        {
            return Xml.FromXml<T>(xml);
        }

        public static T LoadXmlFromFile(string path)
        {
            return Xml.LoadFile<T>(path);
        }

        public virtual void SaveXmlToFile(string path, bool easyToRead)
        {
            var t = (T)this;
            if (easyToRead)
                File.WriteAllText(path, Xml.ToXml(t));
            else
                Xml.SaveFile(path, t);
        }

        public virtual string ToXml()
        {
            var t = (T)this;
            return Xml.ToXml(t);
        }

        public virtual byte[] ToBytes()
        {
            var xml = ToXml();
            return xml.IsEmpty()
                ? new byte[0]
                : Encoding.ASCII.GetBytes(xml);
        }

        public static T FromBytes(byte[] bytes)
        {
            var xml = Encoding.ASCII.GetString(bytes);
            return xml.IsEmpty()
                ? default(T)
                : FromXml(xml);
        }
    }
}