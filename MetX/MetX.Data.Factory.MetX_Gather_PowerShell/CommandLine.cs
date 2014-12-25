using System;
using System.Collections.Generic;
using System.Text;

using MetX;
using MetX.IO;
using MetX.Data;

namespace MetX.Gather
{
    public class PowerShell : GatherProvider
    {
        public override int GatherNow(StringBuilder sb, string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    sb.Append(FileSystem.GatherOutput(args[0], null, null, -1));
                    return 0;
                case 2:
                    sb.Append(FileSystem.GatherOutput(args[0], args[1], null, -1));
                    return 0;
                case 3:
                    sb.Append(FileSystem.GatherOutput(args[0], args[1], args[2], int.Parse(args[3])));
                    return 0;
            }
            return -1;
        }
    }
}
