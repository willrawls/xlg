using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Aspects
{
    public class GenGenOptions
    {
        public string GenerationSet { get; set; }
        public string TemplatesRootPath { get; set; }
        public string BaseOutputPath { get; set; }
        public string ClientName { get; set; }
        public string Namespace { get; set; }
        public string AspectsName { get; set; }
        public string GeneratorsName { get; set; }
        public string Filename { get; set; }
        public GenFramework Framework { get; set; }

        public static GenGenOptions Defaults => new()
            {
                TemplatesRootPath = @"Templates\",
                GenerationSet = "Default",
                ClientName = "Client",
                Namespace = "GenGen",
                AspectsName = "Aspects",
                GeneratorsName = "Generators",
                Framework = GenFramework.Net50Windows,
                BaseOutputPath = @".\",
            };

        public string Resolve(string template)
        {
            var resolved = template
                    .Replace("~~GenerationSet~~", GenerationSet)
                    .Replace("~~BasePath~~", BaseOutputPath)
                    .Replace("~~TemplatesRootPath~~", TemplatesRootPath)
                    .Replace("~~ClientName~~", ClientName)
                    .Replace("~~Namespace~~", Namespace)
                    .Replace("~~AspectsName~~", AspectsName)
                    .Replace("~~GeneratorsName~~", GeneratorsName)
                    .Replace("~~Framework~~", FrameworkValue())
                    .Replace("\r", "")
                ;
            Filename = Path.Combine(BaseOutputPath, ClientName, ClientName + ".csproj");
            resolved = resolved.TokensAfterFirst("\n");
            return resolved;
        }

        public string FrameworkValue()
        {
            switch(Framework)
            {
                case GenFramework.Net50:
                    return "net5.0";
                case GenFramework.Net50Windows:
                    return "net-5.0windows";
                case GenFramework.Core31:
                    return "netcoreapp3.1";
                case GenFramework.Standard20:
                    return "netstandard2.0";
                case GenFramework.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}