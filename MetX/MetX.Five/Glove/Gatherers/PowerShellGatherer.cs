using System.Text;
using MetX.Five.Glove.Providers;
using MetX.Standard.Primary.IO;

namespace MetX.Five.Glove.Gatherers
{
    public class PowerShellGatherer : GatherProvider
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
