using MetX.Standard.Library.Extensions;

namespace MetX.Five.Glove.Gatherers
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