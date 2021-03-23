using System;
using System.Diagnostics;
using System.IO;
using MetX.Standard.Library;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Aspects
{
    public class GenGenOptions
    {
        public string BasePath;
        public string ClientName;
        public string Namespace;
        public string AspectsName;
        public string GeneratorsName;
        public GenFramework Framework;

        public static GenGenOptions Defaults => new()
            {
                BasePath = @".\",
                ClientName = "Client",
                Namespace = "GenGen",
                AspectsName = "Aspects",
                GeneratorsName = "Generators",
                Framework = GenFramework.Net50Windows,
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
                    .Replace("~~Framework~~", FrameworkValue())
                    .Replace("\r", "")
                ;
            Filename = Path.Combine(BasePath, ClientName, ClientName + ".csproj");
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