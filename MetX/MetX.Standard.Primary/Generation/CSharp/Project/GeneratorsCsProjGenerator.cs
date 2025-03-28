﻿using System;
using System.Xml;

namespace MetX.Standard.Primary.Generation.CSharp.Project
{
    public class GeneratorsCsProjGenerator : CsProjGenerator
    {
        public GeneratorsCsProjGenerator()
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) 
            : base(options.WithFilename($"{options.Namespace}.{options.ClientName}"), document)
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(string filePath) : base(filePath)
        {
            WithDefaultTargetTemplate();
        }

        public GeneratorsCsProjGenerator(CsProjGeneratorOptions options) 
            : base(options.WithFilename($"{options.Namespace}.{options.ClientName}"))
        {
            WithDefaultTargetTemplate();
            Options = options;
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

        public override IGenerateCsProj Setup()
        {
            Options.TargetTemplate = "Namespace.GeneratorsName";
            return base.Setup();
        }
    }
}