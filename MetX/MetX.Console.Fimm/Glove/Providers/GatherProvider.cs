using System.Text;

namespace MetX.Fimm.Glove.Providers
{
    public abstract class GatherProvider
    {
        public abstract int GatherNow(StringBuilder sb, string[] args);
    }
}
