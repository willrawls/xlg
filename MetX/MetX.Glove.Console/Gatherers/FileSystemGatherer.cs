using System.Text;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.IO;
using XLG.Pipeliner.Providers;

namespace XLG.Pipeliner.Gatherers
{
    public class FileSystemGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var x = FileSystem.DeepContents(args[0]);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}