using System.Text;
using MetX.Library;

namespace MetX.Data.Factory
{
    public class FileSystemGatherer : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            var source = new System.IO.DirectoryInfo(args[0]);
            var x = IO.FileSystem.DeepContents(source);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}