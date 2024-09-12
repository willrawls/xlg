using System.Xml.Serialization;

namespace MetX.Standard.Strings.Generics;

public class LongAssocType : AssocType<long>
{
    [XmlAttribute]
    public long Long { get; set; }
}

public class StringAssocType : AssocType<string>
{
    [XmlAttribute]
    public string String { get; set; }
}

/*
public class AssocMultiverse : AssocSheet<LongAssocType, AssocReality, VectorAssocType>
{
    
}
*/
