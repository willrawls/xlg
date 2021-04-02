using System;
using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class AspectsCsProjGenerator : CsProjGenerator
    {
        private CsProjGenerator _csProjGeneratorImplementation;

        public AspectsCsProjGenerator() 
        {
            WithDefaultTargetTemplate();
        }

        public override IGenerateCsProj Generate()
        {
            if (Document == null)
                throw new MissingFieldException("Document is required");
            
            if (Options.TryFullResolve(out var resolvedContents)) Document.LoadXml(resolvedContents);
            return this;
        }

        public sealed override IGenerateCsProj WithDefaultTargetTemplate()
        {
            DefaultTargetTemplate = "Namespace.AspectsName";
            return this;
        }

        public AspectsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
            WithDefaultTargetTemplate();
        }

        public AspectsCsProjGenerator(string filePath) : base(filePath)
        {
            WithDefaultTargetTemplate();
        }
    }
}