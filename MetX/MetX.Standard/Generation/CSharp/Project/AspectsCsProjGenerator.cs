using System.Xml;
using MetX.Aspects;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class AspectsCsProjGenerator : CsProjGenerator
    {
        public const string DefaultTargetTemplate = "Namespace.AspectsName";

        public AspectsCsProjGenerator() 
        {
        }

        public AspectsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public AspectsCsProjGenerator(string filePath) : base(filePath)
        {
        }
    }
}