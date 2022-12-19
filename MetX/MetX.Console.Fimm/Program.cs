using System;
using System.Linq;
using MetX.Fimm.Setup;
using MetX.Standard.Strings;

namespace MetX.Fimm
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3 && Harness.ActOn(args, Console.Out)) 
                return;

            Console.WriteLine("Syntax: Fimm Verb Noun Name [Maybe Path] [Maybe connection string] [Maybe additional arguments]");
        }
    }
}