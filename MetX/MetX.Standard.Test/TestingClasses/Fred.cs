using MetX.Standard.Library.ML;

namespace MetX.Standard.Test.TestingClasses;

public class Fred : ListSerializesToXml<Fred, FredItem>
{
    public Guid TestGuid { get; set; } = Guid.NewGuid();
}
