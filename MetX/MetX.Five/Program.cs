using System;
using System.Collections.Generic;
using MetX.Standard.Interfaces;

namespace MetX.Five
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>(args);
            IProcess processorList = new ArgumentListProcessor(arguments);

            processorList.Go();

            Console.WriteLine("Ding");

        }

        public static void ProcessArguments()
        {

        }
    }
}