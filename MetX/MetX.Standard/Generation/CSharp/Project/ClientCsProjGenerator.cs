using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using MetX.Aspects;
using MetX.Standard.Library;

namespace MetX.Standard.Generation.CSharp.Project
{
    public class ClientCsProjGenerator : CsProjGeneratorBase
    {
        public ClientCsProjGenerator() : base()
        {
        }

        public ClientCsProjGenerator(GenGenOptions options, XmlDocument document = null) : base(options, document)
        {
        }

        public ClientCsProjGenerator(string filePath) : base(filePath)
        {
        }

        public ClientCsProjGenerator(GenGenOptions options) : base(options, "Namespace.ClientName")
        {
            
        }
    }
}