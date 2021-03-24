using System.Xml;
using MetX.Aspects;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class AspectsCsProjGenerator : CsProjGenerator
    {
        public AspectsCsProjGenerator() : base()
        {
        }

        public AspectsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public AspectsCsProjGenerator(string filePath) : base(filePath)
        {
        }

        public AspectsCsProjGenerator(CsProjGeneratorOptions options) : base(options, "Namespace.AspectsName")
        {
            
        }
    }
}