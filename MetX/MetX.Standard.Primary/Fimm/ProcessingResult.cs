using System.Collections.Generic;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Standard.Primary.Fimm
{
    public class ProcessingResult
    {
        public List<OutputFile> OutputFiles { get; set; }
        public List<ProcessingResult> Results { get; set; }
        public IProcess Processor { get; set; }
    }
}