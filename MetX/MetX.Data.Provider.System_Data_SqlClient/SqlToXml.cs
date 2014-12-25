using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.Data;

namespace MetX.Gather
{
    public class SqlToXml : GatherProvider
    {

        public override int GatherNow(StringBuilder sb, string[] args)
        {
            SqlDataProvider sdp = new SqlDataProvider(args[1]);
            sb.AppendLine(xml.Declaration);
            sdp.OuterXml(sb, "xlgSqlToXml", string.Empty, args[2]);
            return 0;
        }
    }
}
