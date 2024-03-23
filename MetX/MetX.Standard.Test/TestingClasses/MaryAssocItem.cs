using MetX.Standard.Strings;

namespace MetX.Standard.Test.TestingClasses;

public class MaryAssocItem : TimeTrackingAssocItem
{
    public string MaryAssocItemName { get; set; }
    public Guid MaryAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public MaryAssocItem()
    {
        Key = "Mary";
    }
}