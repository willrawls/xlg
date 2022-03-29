using CommandLine;
using MetX.Standard.Generators.GenGen;

namespace MetX.Standard.Generators
{
    // ReSharper disable once UnusedType.Global
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var worker = new GenGenWorker();
            Parser.Default.ParseArguments<GenGenOptions>(args)
                .WithParsed(worker.Go);
        }
    }
}