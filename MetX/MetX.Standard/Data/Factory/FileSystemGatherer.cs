using System.Text;
using MetX.Standard.Library;

namespace MetX.Standard.Data.Factory
{
    public class FileSystemGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var x = IO.FileSystem.DeepContents(args[0]);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}