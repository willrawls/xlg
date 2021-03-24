using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using MetX.Aspects;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class ClientCsProjGenerator : CsProjGenerator
    {
        public ClientCsProjGenerator() : base()
        {
        }

        public ClientCsProjGenerator(CsProjGeneratorOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public ClientCsProjGenerator(string filePath) : base(filePath)
        {
        }

        public ClientCsProjGenerator(CsProjGeneratorOptions options) : base(options, "Namespace.ClientName")
        {
            
        }
    }
}