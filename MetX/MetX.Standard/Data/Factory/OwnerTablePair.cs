using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Data.Factory
{
    public class OwnerTablePair
    {
        public string Owner;
        public string TableName;
        public override string ToString()
        {
            return Owner.AsString("dbo") + "." + TableName.AsString();
        }
    }
}