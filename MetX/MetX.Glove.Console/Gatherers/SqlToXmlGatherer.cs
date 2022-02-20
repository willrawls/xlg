using System.Text;
using MetX.Standard.Library.ML;
using XLG.Pipeliner.Providers;

namespace XLG.Pipeliner.Gatherers
{
    public class SqlToXmlGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var sdp = new SqlDataProvider(args[1]);
            sb.AppendLine(Xml.Declaration);
            sdp.OuterXml(sb, "xlgSqlToXml", string.Empty, args[2]);
            return 0;
        }
    }
}
