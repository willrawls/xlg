using System.Text;

namespace MetX.Data
{
    public abstract class GatherProvider
    {
        public abstract int GatherNow(StringBuilder sb, string[] args);
    }
}
