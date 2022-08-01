using System;
using System.Xml.Serialization;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.ML;
using MetX.Standard.Strings.Extensions;

#pragma warning disable 1591

namespace MetX.Standard.Primary.Techniques;

[Serializable]
public class PatternWorks
{
    [XmlArray(ElementName = "Connections")] [XmlArrayItem(typeof(Connection), ElementName = "Connection")]
    public
        ParticleList<Connection> Connections;

    [XmlArray(ElementName = "Providers")] [XmlArrayItem(typeof(Provider), ElementName = "Provider")]
    public
        ParticleList<Provider> Providers;

    [XmlArray(ElementName = "Techniques")] [XmlArrayItem(typeof(Technique), ElementName = "Technique")]
    public
        ParticleList<Technique> Techniques;

    [XmlArray(ElementName = "Variables")] [XmlArrayItem(typeof(Variable), ElementName = "Variable")]
    public
        ParticleList<Variable> Variables;


    public string ToXml()
    {
        return Xml.ToXml(this, false);
    }

    public static PatternWorks FromXml(string xmlDoc)
    {
        return xmlDoc.IsEmpty()
            ? null
            : Xml.FromXml<PatternWorks>(xmlDoc);
    }
}