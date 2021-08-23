using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Standard.Library;

namespace MetX.Five
{
    public class FiverProcessor
    {
        public string Filename { get; set; }

        public Area MainTemplate { get; set; }

        public List<Area> Templates { get; set; } = new();
        public List<string> Contents { get; set; } = new();

        public Areas Areas { get; set; } = new();

        public FiverProcessor(string filename = null)
        {
            Filename = filename;
            if (filename.IsNotEmpty() && File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                Contents = new List<string>(lines);
            }
        }

        public void Parse(string fiver)
        {
            Filename = null;
            Contents = fiver.LineList(StringSplitOptions.RemoveEmptyEntries);
            Parse();
        }

        public bool Parse()
        {
            if (Contents.IsEmpty())
                return false;

            var processingAlreadyBeganPreviously = false;

            // ReSharper disable once TooWideLocalVariableScope
            Area currentArea = null;
            
            foreach (var line in Contents)
            {
                var instruction = line.TokenBetween("~~", ":");
                if (instruction.Length > 0)
                {
                    currentArea = new Area(instruction, line, ref processingAlreadyBeganPreviously);
                    Areas.Add(currentArea);
                }
                else if (line.Trim().Length == 0)
                {
                    // Skip
                }
                else if (currentArea == null)
                {
                    throw new Exception(
                        "Non blank lines in fiver contents before the first instruction are not allowed");
                }
                else
                {
                    currentArea.Lines.Add(line);
                }
            }

            Templates = Areas
                .Where(a => a.InstructionType is InstructionType.Template)
                .ToList();

            MainTemplate = Templates.FirstOrDefault(t => t
                .IsAreaForProcessing == false && t
                .InstructionType == InstructionType.Template && t
                .Arguments.Count == 0);
            
            return true;
        }
    }
}