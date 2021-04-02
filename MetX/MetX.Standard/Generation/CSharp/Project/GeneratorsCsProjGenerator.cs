using System;
using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class GeneratorsCsProjGenerator : CsProjGenerator
    {
        public GeneratorsCsProjGenerator() : base()
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(string filePath) : base(filePath)
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options) : base(options)
        {
            WithDefaultTargetTemplate();
        }

        public sealed override IGenerateCsProj WithDefaultTargetTemplate()
        {
            DefaultTargetTemplate = "Namespace.GeneratorsName";
            return this;
        }
        
        public override IGenerateCsProj Generate()
        {
            if (Document == null)
                throw new MissingFieldException("Document is required");
            
            if (Options.TryFullResolve(out var resolvedContents)) Document.LoadXml(resolvedContents);
            return this;
        }
    }
}