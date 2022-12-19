using System;
using System.IO;
using System.Linq;
using MetX.Fimm.Setup;
using MetX.Standard.Strings;

namespace MetX.Fimm;

public static class Harness
{
    public static bool ActOn(string[] args, TextWriter textWriter)
    {
        if (!ProcessingFunctionFactory(args, textWriter, out var settings, out var processingFunction) || processingFunction == null)
        {
            return false;
        }

        textWriter.WriteLine("Processing...");
        var result = processingFunction(settings);

        if (result.ActualizationResult.ActualizationSuccessful != true)
        {
            textWriter.WriteLine($"Error: {result.ActualizationResult.ActualizeErrorText}");
            return false;
        }

        if (result.ActualizationResult.Errors.IsNotEmpty())
        {
            textWriter.WriteLine($"Errors: {result.ActualizationResult.Errors}");
        }

        textWriter.WriteLine("Done");
        return true;
    }

    public static bool ProcessingFunctionFactory(string[] args, TextWriter textWriter, out ArgumentSettings settings, out Func<ArgumentSettings, ProcessorResult> processingFunction)
    {
        settings = InitializeSettings(args, textWriter);
        processingFunction = null;

        if (settings.IsValid == false)
            return false;

        var actor = settings.Verb.GetActor(settings.Noun);
        processingFunction = null;
        if (actor != null)
        {
            processingFunction = actor.GetProcessingFunction(settings);
        }

        return processingFunction != null;
    }

    public static ArgumentSettings InitializeSettings(string[] args, TextWriter textWriter)
    {
        if (args.IsEmpty())
        {
            textWriter.WriteLine("Nothing to do");
            return null;
        }

        if (args.Length < 3)
        {
            return null;
        }

        var settings = new ArgumentSettings
        {
            Verb = Ron.DetermineVerb(args[0]),
            Noun = Ron.DetermineNoun(args[1]),
            Name = args[2],
        };

        if (args.Length > 3) settings.Path = args[3];
        if (args.Length > 4) settings.AdditionalArguments = args.Skip(3).ToList();

        textWriter.WriteLine(settings.ToString());
        return settings;
    }


}