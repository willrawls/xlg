using System;

namespace MetX.Generators.Samples.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MetX.Generated.GClass.GMethod();
        }
    }

    [MetX.Aspects.GenerateAddingStaticCode]
    public class Fred
    {
        public string George;
        public long Frank;
        public DateTime Mary;
    }
}
