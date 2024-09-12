using System;
using System.Collections.Generic;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Fimm
{
    public class Area
    {
        public string Name { get; set; }
        public InstructionType InstructionType { get; set; }
        public bool IsAreaForProcessing { get; set; }
        public TemplateType TemplateType { get; set; }
        public string Target { get; set; }

        public List<string> Lines { get; set; } = new();
        public List<string> Arguments { get; set; } = new();

        public Area(bool isAreaForProcessing)
        {
            IsAreaForProcessing = isAreaForProcessing;
        }

        public Area(string instruction, string line, ref bool processingAlreadyBeganPreviously)
        {
            if (!Enum.TryParse(typeof(InstructionType), instruction, true, out var possibleInstructionType)) return;

            if (possibleInstructionType != null)
                InstructionType = (InstructionType) possibleInstructionType;

            Name = line
                .TokensAfterFirst(":")
                .Replace("\r", "")
                .FirstToken("\n")
                .Trim();

            Arguments = Name.WordList();
            TemplateType = TemplateType.NotATemplate;

            if (InstructionType == InstructionType.Template)
            {
                TemplateType = TemplateType.Unknown;
                if (!Enum.TryParse(typeof(TemplateType), Arguments[0], true, out var possibleTemplateType)) 
                    possibleTemplateType = TemplateType.Default;

                if (possibleTemplateType != null)
                    TemplateType = (TemplateType) possibleTemplateType;

                if (TemplateType != TemplateType.Unknown && Arguments.Count > 1) 
                    Target = Arguments[1];

                if (Name.IsEmpty())
                    Name = "Default";
            }
            else if (InstructionType == InstructionType.BeginProcessing)
            {
                IsAreaForProcessing = true;
                processingAlreadyBeganPreviously = true;
            }
            else
            {
                IsAreaForProcessing = processingAlreadyBeganPreviously;
            }
        }
    }
}