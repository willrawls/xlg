using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Gather
{
    public class FileSystem : GatherProvider
    {

        public override int GatherNow(StringBuilder sb, string[] args)
        {
            System.IO.DirectoryInfo Source = new System.IO.DirectoryInfo(args[0]);
            xlgFolder x = MetX.IO.FileSystem.DeepContents(Source);
            sb.AppendLine(xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}
