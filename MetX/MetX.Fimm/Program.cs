using System;
using System.Linq;
using MetX.Fimm.Setup;
using MetX.Standard.Strings;

namespace MetX.Fimm
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.IsEmpty())
            {
                Console.WriteLine("Nothing to do");
                return;
            }

            if (args.Length < 3)
            {
                ShowSyntax();
                return;
            }

            var settings = new ArgumentSettings
            {
                Verb = Ron.DetermineVerb(args[0]),
                Noun = Ron.DetermineNoun(args[1]),
                Name = args[2],
            };

            if (args.Length > 3) settings.Path = args[3];
            if (args.Length > 4) settings.AdditionalArguments = args.Skip(3).ToList();

            FimmActorBase actor = settings.Verb.GetActor(settings.Noun);
            Func<ArgumentSettings, ProcessorResult> processingFunction = null;
            if (actor != null)
            {
                processingFunction = actor.GetProcessingFunction(settings);
            }

            if(processingFunction == null)
            {
                ShowSyntax();
                return;
            }

            var result = processingFunction(settings);
            if(result.ActualizationResult.ActualizationSuccessful != true)
            {
                Console.WriteLine($"Error: {result.ActualizationResult.ActualizeErrorText}");
                ShowSyntax();
                return;
            }

            System.Console.WriteLine("Ding");
        }

        private static void ShowSyntax()
        {
            Console.WriteLine(
                "Syntax: Fimm Verb Noun Name [Maybe Path] [Maybe connection string] [Maybe additional arguments]");
        }

        public static void ProcessArguments()
        {

        }
    }
}