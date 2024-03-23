using MetX.Standard.Strings;

namespace MetX.Standard.Test.TestingClasses;

public class FredAssocItem : TimeTrackingAssocItem
{
    public string FredAssocItemName { get; set; }
    public Guid FredAssocItemTestGuid { get; set; } = Guid.NewGuid();

    public FredAssocItem()
    {
        Key = "Fred";
    }
}