using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Five.Setup;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Strings.Extensions;

namespace MetX.Five
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
            if (args.Length > 4) settings.AdditionalArguments = args.Skip(4).ToList();

            var actor = settings.Verb.GetActor(settings.Noun);
            Func<ArgumentSettings, ProcessorResult> processingFunction = null;
            if (actor != null)
            {
                processingFunction = actor[settings];
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

            Console.WriteLine("Ding");

        }

        private static void ShowSyntax()
        {
            Console.WriteLine(
                "Syntax: Fiver Verb Noun Name [Maybe Path] [Maybe connection string] [Maybe additional arguments]");
        }

        public static void ProcessArguments()
        {

        }
    }
}