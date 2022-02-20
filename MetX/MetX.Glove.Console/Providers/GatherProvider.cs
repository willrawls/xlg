using System.Text;

namespace XLG.Pipeliner.Providers
{
    public abstract class GatherProvider
    {
        public abstract int GatherNow(StringBuilder sb, string[] args);
    }
}
