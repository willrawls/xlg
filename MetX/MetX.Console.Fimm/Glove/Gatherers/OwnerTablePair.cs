using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm.Glove.Gatherers
{
    public class OwnerTablePair
    {
        public string Owner;
        public string TableName;
        public override string ToString()
        {
            return Owner.AsStringFromObject("dbo") + "." + TableName.AsStringFromObject();
        }
    }
}