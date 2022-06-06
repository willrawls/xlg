using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetX.Five.Setup;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Interfaces;

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

            var actor = Ron.SupportedFiverActions
                .FirstOrDefault(a => 
                a.Verb == settings.Verb && a.Noun == settings.Noun);

            if(actor == null)
            {
                ShowSyntax();
                return;
            }

            if(!actor.ReadyToAct(settings, out var reason))
            {
                Console.WriteLine($"Error: {reason}");
                ShowSyntax();
                return;
            }

            ProcessorResult results = actor[settings](settings);

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