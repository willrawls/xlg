using System;
using MetX.Fimm.Glove.Providers;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm.Glove.Data
{
    /// <summary>
    /// Creates an aggregate function call for ANSI SQL
    /// </summary>
    public class Aggregate
    {
        public string AggregateString = string.Empty;
        public Aggregate() {  }
        public Aggregate(DataProvider instance, AggregateFunction agg, string columnName, string alias)
        {
            AggregateString = Enum.GetName(typeof(AggregateFunction), agg).AsString().ToUpper() + "(" + instance.ValidIdentifier(columnName) + ") as '" + alias + "'";
        }
        public static Aggregate New(DataProvider instance, AggregateFunction agg, string columnName, string alias) { return new(instance, agg, columnName, alias); }
    }
}