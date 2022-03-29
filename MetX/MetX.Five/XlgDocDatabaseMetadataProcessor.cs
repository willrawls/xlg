using System.Threading.Tasks;
using MetX.Standard.Primary.Five;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five
{
    public class XlgDocDatabaseMetadataProcessor : IProcess
    {
        public XlgDocDatabaseMetadataProcessor(string templateFilePath)
        {
            Template = new FiverParser(templateFilePath);
        }

        public FiverParser Template { get; set; }

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