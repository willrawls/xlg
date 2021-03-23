using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public interface IRefer
    {
        ClientCsProjGenerator Parent { get; set; }
        XmlElement Remove();
        XmlElement InsertOrUpdate();
    }
}