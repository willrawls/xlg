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

            if(actor.ReadyToAct(settings, out var reason))
            {
                Console.WriteLine($"Error: {reason}");
                ShowSyntax();
                return;
            }

            ProcessorResult result = null;

            switch (settings.Verb)
            {
                case ArgumentVerb.Unknown:
                    Console.WriteLine($"Error: Unknown verb");
                    ShowSyntax();
                    break;

                case ArgumentVerb.Run:
                    result = actor.Run(settings);
                    break;

                case ArgumentVerb.Build:
                    result = actor.Build(settings);
                    break;
                case ArgumentVerb.Gen:
                    result = actor.Gen(settings);
                    break;
                case ArgumentVerb.Regen:
                    result = actor.Regen(settings);
                    break;
                case ArgumentVerb.Walk:
                    result = actor.Walk(settings);
                    break;
                case ArgumentVerb.Stage:
                    result = actor.Stage(settings);
                    break;
                case ArgumentVerb.Add:
                    result = actor.Add(settings);
                    break;
                case ArgumentVerb.Clone:
                    result = actor.Clone(settings);
                    break;
                case ArgumentVerb.Delete:
                    result = actor.Delete(settings);
                    break;
                case ArgumentVerb.Test:
                    result = actor.Test(settings);
                    break;
                case ArgumentVerb.Remove:
                    result = actor.Remove(settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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