using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public interface IGenerateCsProj
    {
        XmlNode ProjectNode { get; }
        PropertyGroups PropertyGroups { get; set; }
        Targets Targets { get; set; }
        string FilePath { get; set; }
        XmlDocument Document { get; set; }
        ItemGroup ItemGroup { get; set; }
        
        bool Save();
        void SetElementInnerText(string xpath, bool value);
        void SetElementInnerText(string xpath, string innerText);
        string InnerTextAt(string xpath, bool blankMeansNull = true);
        XmlNode MakeXPath(string xpath);
        XmlNode MakeXPath(XmlNode parent, string xpath);
        bool IsElementMissing(string xpath);
    }
}