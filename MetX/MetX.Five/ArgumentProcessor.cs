using System.Threading.Tasks;
using MetX.Standard.Primary.Five;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five
{
    public class ArgumentProcessor : IProcess
    {
        private readonly IProcess _processor;

        public ArgumentProcessor(ArgumentNoun argumentNoun)
        {
            _processor = new XlgDocDatabaseMetadataProcessor("Five.xlgt");
        }

        public ProcessingResult Go()
        {
            return _processor.Go();
        }

        public async Task<ProcessingResult> GoAsync()
        {
            return await _processor.GoAsync();
        }
    }
}