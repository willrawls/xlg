using System.Threading.Tasks;
using MetX.Standard.Primary.Fimm;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Fimm
{
    public class XlgDocDatabaseMetadataProcessor : IProcess
    {
        public XlgDocDatabaseMetadataProcessor(string templateFilePath)
        {
            Template = new FimmParser(templateFilePath);
        }

        public FimmParser Template { get; set; }

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