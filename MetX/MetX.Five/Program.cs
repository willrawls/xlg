using System;
using System.Collections.Generic;
using System.IO;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>(args);

            if (args.Length > 0 && File.Exists(args[0]) && args[0].EndsWith(".xlgq"))
            {
                Dirs.LastScriptFilePath = args[0];
            }

            IProcess processorList = new ArgumentListProcessor(arguments);

            processorList.Go();

            Console.WriteLine("Ding");

        }

        public static void ProcessArguments()
        {

        }
    }
}