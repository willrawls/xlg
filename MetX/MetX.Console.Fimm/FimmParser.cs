﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Standard.Strings;

namespace MetX.Fimm;

public class FimmParser
{
    public string Filename { get; set; }
    public bool InMemory { get; set; }

    public Area MainTemplate { get; set; }
    public Areas Areas { get; set; } = new();
    public Areas Activities { get; set; } = new();

    public List<Area> Templates { get; set; } = new();

    public List<string> Contents { get; set; } = new();

    public FimmParser(string filename = null)
    {
        Filename = filename;
        if (filename.IsNotEmpty() && File.Exists(filename))
        {
            var lines = File.ReadAllLines(filename);
            Contents = new List<string>(lines);
        }
    }

    public bool Parse(string text)
    {
        InMemory = true;
        Filename = null;
        Contents = text.LineList(StringSplitOptions.RemoveEmptyEntries);
        return Parse();
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
            if (instruction.IsNotEmpty())
            {
                currentArea = new Area(instruction, line, ref processingAlreadyBeganPreviously);
                if (currentArea.TemplateType != TemplateType.NotATemplate)
                    Templates.Add(currentArea);
                else if (processingAlreadyBeganPreviously)
                    Activities.Add(currentArea);
                else if (currentArea.InstructionType == InstructionType.Area) Areas.Add(currentArea);
            }
            else if (line.Trim().Length == 0)
            {
                currentArea?.Lines.Add(line);
            }
            else if (currentArea == null)
            {
                throw new Exception(
                    "Non blank lines in script contents before the first instruction are not allowed");
            }
            else
            {
                currentArea.Lines.Add(line);
            }
        }

        MainTemplate = Templates.Count switch
        {
            1 => Templates[0],
            > 1 => Templates.FirstOrDefault(t =>
                t.IsAreaForProcessing == false 
                && t.InstructionType == InstructionType.Template 
                && t.Arguments.Count == 0),
            _ => MainTemplate
        };
        return true;
    }
}