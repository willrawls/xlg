using System.Xml;
using MetX.Aspects;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class GeneratorsCsProjGenerator : CsProjGenerator
    {
        public GeneratorsCsProjGenerator() : base()
        {
        }

        public GeneratorsCsProjGenerator(GenGenOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public GeneratorsCsProjGenerator(string filePath) : base(filePath)
        {
        }

        public GeneratorsCsProjGenerator(GenGenOptions options) : base(options, "Namespace.GeneratorsName")
        {
            
        }
    }
}