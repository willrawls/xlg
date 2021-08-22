using System.Collections.Generic;
using MetX.Standard.Interfaces;

namespace MetX.Standard.Five
{
    public class ProcessingResult
    {
        public List<OutputFile> OutputFiles { get; set; }
        public List<ProcessingResult> Results { get; set; }
        public IProcess Processor { get; set; }
    }
}