using XLG.Pipeliner.Providers;

namespace XLG.Pipeliner.Data
{
    /// <summary>
    /// Creates an ORDER BY statement for ANSI SQL
    /// </summary>
    public class OrderBy
    {
        public string OrderString;

        public OrderBy() { }
        public OrderBy(string orderString) { OrderString = orderString; }
        public static OrderBy Desc(DataProvider instance, string columnName) { return new(" ORDER BY " + instance.ValidIdentifier(columnName) + " DESC"); }
        public static OrderBy Asc(DataProvider instance, string columnName) { return new(" ORDER BY " + instance.ValidIdentifier(columnName)); }
        public static OrderBy Any(DataProvider instance, string orderString) { return new(orderString); }
    }
}