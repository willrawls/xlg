using System.Text;

namespace MetX.Standard.Data
{
    public abstract class GatherProvider
    {
        public abstract int GatherNow(StringBuilder sb, string[] args);
    }
}
