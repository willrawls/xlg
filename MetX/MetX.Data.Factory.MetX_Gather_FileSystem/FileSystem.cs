using System.Text;

using MetX.Data;
using MetX.Data.Pipelines;
using MetX.Library;

namespace MetX.Gather
{
    public class FileSystem : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(args[0]);
            xlgFolder x = IO.FileSystem.DeepContents(source);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}