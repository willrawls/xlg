using MetX.Standard.Strings.ML;

namespace MetX.Standard.Test.TestingClasses;

public class Mary : ListSerializesToXml<Mary, MaryItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}