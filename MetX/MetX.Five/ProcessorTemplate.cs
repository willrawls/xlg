using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Standard.Library;

namespace MetX.Five
{
    public class ProcessorTemplate
    {
        public ProcessorTemplate(string templateFilePath)
        {
            TemplateFilePath = templateFilePath;
            Contents = new List<string>(File.ReadAllLines(templateFilePath));

            ParseContents();
        }

        public void ParseContents()
        {
            var currentArea = new Area(false);
            var processingAlreadyBeganPreviously = false;

            foreach (var line in Contents)
            {
                var instruction = line.TokenBetween("~~", ":");
                if (instruction.Length == 0)
                {
                    currentArea.Lines.Add(line);
                }
                else
                {
                    currentArea = new Area(instruction, line, ref processingAlreadyBeganPreviously);
                }
            }

            Templates = Areas
                .Where(a => a.Instruction is InstructionType.Template)
                .ToList();

            MainTemplate = Templates.FirstOrDefault(t => t
                .IsAreaForProcessing == false && t
                .Instruction == InstructionType.Template && t
                .InstructionArguments.Count == 0);
        }

        public Area MainTemplate { get; set; }
        public List<Area> Templates { get; set; }
        public string TemplateFilePath { get; set; }
        public List<string> Contents { get; set; }
        public Areas Areas { get; set; }
    }
}