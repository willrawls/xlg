using System.Xml;
using MetX.Aspects;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class GeneratorsCsProjGenerator : CsProjGenerator
    {
        public const string DefaultTargetTemplate = "Namespace.GeneratorsName";
        
        public GeneratorsCsProjGenerator() : base()
        {
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public GeneratorsCsProjGenerator(string filePath) : base(filePath)
        {
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options) : base(options)
        {
            
        }
    }
}