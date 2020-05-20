namespace MetX.Data
{
    /// <summary>
    /// Creates an ORDER BY statement for ANSI SQL
    /// </summary>
    public class OrderBy
    {
        public string OrderString;

        public OrderBy() { }
        public OrderBy(string orderString) { OrderString = orderString; }
        public static OrderBy Desc(DataProvider instance, string columnName) { return new OrderBy(" ORDER BY " + instance.ValidIdentifier(columnName) + " DESC"); }
        public static OrderBy Asc(DataProvider instance, string columnName) { return new OrderBy(" ORDER BY " + instance.ValidIdentifier(columnName)); }
        public static OrderBy Any(DataProvider instance, string orderString) { return new OrderBy(orderString); }
    }
}