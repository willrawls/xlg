using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using MetX.Standard.Primary.Five;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five
{
    public class ArgumentSettings
    {
        public ArgumentSettings()
        {
        }

        public ArgumentNoun Noun;
        public ArgumentVerb Verb;

        public string Name{ get; set; }
        public string Path{ get; set; }
        public string ConnectionString{ get; set; }
        public List<string> AdditionalArguments { get; set; }
    }

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
}