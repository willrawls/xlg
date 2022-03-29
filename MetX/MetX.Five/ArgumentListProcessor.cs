using System.Collections.Generic;
using System.Threading.Tasks;
using MetX.Standard.Primary.Five;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five
{
    public class ArgumentListProcessor : IProcess 
    {
        public List<ArgumentProcessor> Arguments = new();

        public ArgumentListProcessor(List<string> arguments)
        {
            Arguments.Add(new ArgumentProcessor(ProcessorType.Xlg));
        }

        public ProcessingResult Go()
        {
            var result = new ProcessingResult();
            foreach (var processor in Arguments)
            {
                result.Results.Add(processor.Go());
            }

            return result;
        }

        public async Task<ProcessingResult> GoAsync()
        {
            return await Task.Run(Go);
        }
    }
}