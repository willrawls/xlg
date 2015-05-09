namespace MetX.Data
{
    /// <summary>
    /// Creates an ORDER BY statement for ANSI SQL
    /// </summary>
    public class OrderBy
    {
        public readonly string OrderString;

        public OrderBy() { }
        public OrderBy(string orderString) { this.OrderString = orderString; }
        public static OrderBy Desc(DataProvider Instance, string columnName) { return new OrderBy(" ORDER BY " + Instance.ValidIdentifier(columnName) + " DESC"); }
        public static OrderBy Asc(DataProvider Instance, string columnName) { return new OrderBy(" ORDER BY " + Instance.ValidIdentifier(columnName)); }
        public static OrderBy Any(DataProvider Instance, string orderString) { return new OrderBy(orderString); }
    }
}