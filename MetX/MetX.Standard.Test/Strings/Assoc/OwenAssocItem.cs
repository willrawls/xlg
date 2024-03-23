using MetX.Standard.Strings.Interfaces;

namespace MetX.Standard.Test.Strings.Assoc;

public class OwenAssocItem : IAssocItem
{
    public string OwenItemName { get; set; }
    public Guid OwenItemTestGuid { get; set; } = Guid.NewGuid();

    public string Key { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public Guid ID { get; set; }
    public int Number { get; set; }
    public string Category { get; set; }
}