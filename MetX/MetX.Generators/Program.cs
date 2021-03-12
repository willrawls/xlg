using System;

namespace MetX.Generators
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Generates generator in current folder
            //  MetX.Generators.exe -generate Namespace.GeneratorName 
            //  When sub folders are missing or are empty
            //      Creates 2 sub folders
            //          Namespace
            //              Namespace.csproj
            //              GeneratorName.cs
            //          Namespace.Aspects
            //              Namespace.Aspects.csproj
            //              GeneratorNameAttribute.cs
            //  When sub folder already exist
            //      Add or overwrite
            //          Namespace
            //              Namespace.csproj
            //                  Create NextGeneratorName.cs
            //                  Add NextGeneratorName.cs to .csproj
            //              Namespace.NextGeneratorName.cs
            //          Namespace.Aspects
            //              Namespace.Name.Aspects.csproj
            //              NextGeneratorNameAttribute.cs
            //  MetX.Generators.exe -addto "PathTo.csproj the generator should be added as an analyzer"
            //      
            //  MetX.Generators.exe -just Namespace.GeneratorName -addto "PathTo.csproj the generator should be added as an analyzer"
            //      Find and modify/update the csproj of the client
            
            Console.WriteLine("Ding 3");
        }
    }
}
