using System.Text;
using MetX.Five.Glove.Providers;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.IO;

namespace MetX.Five.Glove.Gatherers
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