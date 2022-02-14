using System.IO;
using MetX.Standard.Library;
using MetX.Standard.Library.ML;

namespace MetX.Standard.Techniques
{
    public abstract class SerializeToXml<T>
        where T : SerializeToXml<T>, new()
    {
        public static T FromXml(string xml) { return Xml.FromXml<T>(xml); }

        public static T LoadXmlFromFile(string path) { return Xml.LoadFile<T>(path); }

        public virtual void SaveXmlToFile(string path, bool easyToRead)
        {
            var t = (T)this;
            if (easyToRead)
            {
                File.WriteAllText(path, Xml.ToXml(t));
            }
            else
            {
                Xml.SaveFile(path, t);
            }
        }

        public virtual string ToXml()
        {
            var t = (T)this;
            return Xml.ToXml(t);
        }

    }
}