using MetX.Standard.Strings;

namespace MetX.Standard.Test.TestingClasses;

public class FredItem : BasicAssocItem
{
    public string FredItemName { get; set; }
    public Guid FredItemTestGuid { get; set; } = Guid.NewGuid();
}

