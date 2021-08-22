using System.Collections.Generic;
using System.Threading.Tasks;
using MetX.Standard.Five;
using MetX.Standard.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetX.Five
{
    public class XlgDocDatabaseMetadataProcessor : IProcess
    {
        public XlgDocDatabaseMetadataProcessor(string templateFilePath)
        {
            Template = new ProcessorTemplate(templateFilePath);
        }

        public ProcessorTemplate Template { get; set; }

        public ProcessingResult Go()
        {
            return new()
            {
                OutputFiles = new(),
                Results = new(),
                Processor = this,
            };
        }

        public async Task<ProcessingResult> GoAsync()
        {
            return await Task.Run(Go);
        }
    }
}