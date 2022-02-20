using System.Text;
using MetX.Standard.Library.ML;
using XLG.Pipeliner.Providers;

namespace XLG.Pipeliner.Gatherers
{
    public class FileSystemGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var x = MetX.Standard.IO.FileSystem.DeepContents(args[0]);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}