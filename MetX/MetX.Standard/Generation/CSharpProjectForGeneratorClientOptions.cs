using System.IO;
using MetX.Standard.Library;

namespace MetX.Aspects
{
    public class CSharpProjectForGeneratorClientOptions
    {
        public string BasePath;
        public string ClientName;
        public string Namespace;
        public string AspectsName;
        public string GeneratorsName;
        public GenFramework Framework;

        public static CSharpProjectForGeneratorClientOptions Defaults => new()
            {
                BasePath = @".\",
                ClientName = "Client",
                Namespace = "GenGen",
                AspectsName = "Aspects",
                GeneratorsName = "Generators",
                Framework = GenFramework.Windows,
            };

        public string Filename { get; set; }

        public string Resolve(string template)
        {
            var resolved = template
                    .Replace("~~BasePath~~", BasePath)
                    .Replace("~~ClientName~~", ClientName)
                    .Replace("~~Namespace~~", Namespace)
                    .Replace("~~AspectsName~~", AspectsName)
                    .Replace("~~GeneratorsName~~", GeneratorsName)
                    .Replace("~~Framework~~", Framework.ToString())
                    .Replace("\r", "")
                ;
            Filename = Path.Combine(BasePath, ClientName, ClientName + ".csproj");
            resolved = resolved.TokensAfterFirst("\n");
            return resolved;
        }
    }

    public enum GenFramework
    {
        Unknown,
        Library,
        Executable,
        StandardLibrary,
        StandardExecutable,
        Windows,
        Linux,
        Core
    }
}