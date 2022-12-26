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
            if (args.Length < 3) 
                return;

            if(!Harness.ActOn(args, Console.Out))
                Console.WriteLine("Syntax: Fimm Verb Noun Name [Maybe Path] [Maybe connection string] [Maybe additional arguments]");
        }
    }
}