using System;
using System.Collections.Generic;
using System.Linq;
using MetX.Standard.Library;

namespace MetX.Five
{
    public class Area
    {
        public Area(bool isAreaForProcessing)
        {
            IsAreaForProcessing = isAreaForProcessing;
        }

        public Area(string instruction, string line, ref bool processingAlreadyBeganPreviously)
        {
            if (!Enum.TryParse(typeof(InstructionType), instruction, true, out object possibleInstructionType)) return;

            if (possibleInstructionType != null) 
                Instruction = (InstructionType) possibleInstructionType;
            
            InstructionArguments = line.TokensAfterFirst(":").Trim().Split(' ').Where(a => a.IsNotEmpty()).ToList();

            if (Instruction == InstructionType.BeginProcessing)
            {
                IsAreaForProcessing = true;
                processingAlreadyBeganPreviously = true;
            }
            else
            {
                IsAreaForProcessing = processingAlreadyBeganPreviously;
            }

        }

        public List<string> InstructionArguments { get; set; }
        public InstructionType Instruction { get; set; }
        public List<string> Lines { get; set; }
        public bool IsAreaForProcessing { get; set; }

    }
}