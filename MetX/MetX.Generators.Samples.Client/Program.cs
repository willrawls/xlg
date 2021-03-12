using System;

namespace MetX.Generators.Samples.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Generated.GClass.GMethod();
            Generated.FredExampleOne.Go();
        }
    }

    [MetX.Aspects.GenerateAddingStaticCode]
    public class EmptyClass { }
    
    [Aspects.GenerateFromTemplate("exampleOne.template")]
    public class Fred
    {
        public string George;
        public long Frank;
        public DateTime Mary;
    }
}
