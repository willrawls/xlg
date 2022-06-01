/*using System.Collections.Generic;
using System.Threading.Tasks;
using MetX.Five.Scripts;
using MetX.Standard.Primary.Five;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Setup
{
    public class ArgumentListProcessor : IProcess 
    {
        public List<ArgumentProcessor> Processors = new();
        private readonly ArgumentSettings _argumentSettings;


        public ArgumentListProcessor(List<string> arguments)
        {
            
            Processors.Add(new ArgumentProcessor(ArgumentNoun.Xlg));
            _argumentSettings = new ArgumentSettings();
        }

        public ProcessingResult Go()
        {
            var result = new ProcessingResult();
            foreach (var processor in Processors)
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
}*/