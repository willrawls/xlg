using System;
using System.Xml;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class ClientCsProjGenerator : CsProjGenerator
    {
        public ClientCsProjGenerator() 
        {
            WithDefaultTargetTemplate();
        }

        public ClientCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) 
            : base(options.WithFilename($"{options.Namespace}.{options.ClientName}"), document)
        {
            WithDefaultTargetTemplate();
        }

        public ClientCsProjGenerator(string filePath) : base(filePath)
        {
            WithDefaultTargetTemplate();
        }

        public ClientCsProjGenerator(CsProjGeneratorOptions options) 
            : base(options.WithFilename($"{options.Namespace}.{options.ClientName}"))
        {
            WithDefaultTargetTemplate();
            Options = options;
        }

        public sealed override IGenerateCsProj WithDefaultTargetTemplate()
        {
            DefaultTargetTemplate = "Namespace.ClientName";
            return this;
        }

        public override IGenerateCsProj Generate()
        {
            if (Document == null)
                throw new MissingFieldException("Document is required");
            
            if (Options.TryFullResolve(out var resolvedContents)) Document.LoadXml(resolvedContents);
            return this;
        }

        public override IGenerateCsProj Setup()
        {
            Options.TargetTemplate = "Namespace.ClientName";
            return base.Setup();
        }
    }
}