using System;
using MetX.Library;

namespace MetX.Data
{
    /// <summary>
    /// Creates an aggregate function call for ANSI SQL
    /// </summary>
    public class Aggregate
    {
        public string AggregateString = string.Empty;
        public Aggregate() {  }
        public Aggregate(DataProvider Instance, AggregateFunction agg, string columnName, string alias)
        {
            AggregateString = Enum.GetName(typeof(AggregateFunction), agg).AsString().ToUpper() + "(" + Instance.ValidIdentifier(columnName) + ") as '" + alias + "'";
        }
        public static Aggregate New(DataProvider Instance, AggregateFunction agg, string columnName, string alias) { return new Aggregate(Instance, agg, columnName, alias); }
    }
}