using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;
using MetX.Library;

namespace MetX.Gather
{
    public class FileSystem : GatherProvider
    {

        public override int GatherNow(StringBuilder sb, string[] args)
        {
            System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(args[0]);
            xlgFolder x = MetX.IO.FileSystem.DeepContents(source);
            sb.AppendLine(Xml.Declaration);
            sb.Append(x.OuterXml());
            return 0;
        }
    }
}
