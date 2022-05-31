using System;
using System.Collections.Generic;
using System.Configuration;
using MetX.Five.QuickScripts;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Metadata;
using MetX.Standard.Primary.Scripts;

namespace MetX.Five;

public enum ArgumentNoun
{
    Unknown,
    Script,
    Database,
    Xlg,
    Folder,
    Xml,
    Xsd,
    Json,
    Project,
    GenGen
}

public enum ArgumentVerb
{
    Unknown,
    Run,
    Build,
    Gen,
    Regen,
    Walk,
    Stage,
    Add,
    Clone,
    Delete,
    Test
}

public class ValidActions : List<IAct>
{
    public static ValidActions Supported =>
        new()
        {
            new ValidCombo<ScriptActor>(ArgumentVerb.Run, ArgumentNoun.Script)
        };
}

public interface IAct
{
    bool ReadyToAct(ArgumentSettings settings, out string reason);
    ProcessorResult Act(ArgumentSettings settings);
}

public class ScriptActor : IAct
{
    public bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = null;

        var validVerb = settings.Verb is
            ArgumentVerb.Run or ArgumentVerb.Build or
            ArgumentVerb.Add or ArgumentVerb.Clone or
            ArgumentVerb.Delete;
        if (!validVerb)
            return false;

        return true;
    }

    public ProcessorResult Act(ArgumentSettings settings)
    {
        switch (settings.Verb)
        {
            case ArgumentVerb.Run:
                return RunScript(settings);
            case ArgumentVerb.Build:
                break;
            case ArgumentVerb.Add:
                break;
            case ArgumentVerb.Clone:
                break;
            case ArgumentVerb.Delete:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    private ProcessorResult RunScript(ArgumentSettings settings)
    {
        ConsoleContext context = new ConsoleContext();
        IGenerationHost host = new ConsoleGenerationHost(context);
        var wallaby = new Wallaby(host);
        var scriptToRun = wallaby.FindScript(settings.Name);

        wallaby.FiverRunScript(scriptToRun);
        return null;
    }
}

public class ConsoleGenerationHost : IGenerationHost
{
    public ConsoleGenerationHost(ConsoleContext context)
    {
        Context = context;
    }

    public IMessageBox MessageBox { get; set; } = new ConsoleMessageBoxHost();

    public MessageBoxResult InputBox(string title, string description, ref string itemName)
    {
        return MessageBoxResult.Yes;
    }

    public Func<string> GetTextForProcessing { get; set; } = () => "";

    public ContextBase Context { get; set; }

    public void WaitFor(Action action)
    {
        throw new NotImplementedException();
    }
}

public class ConsoleMessageBoxHost : IMessageBox
{
    public IGenerationHost Host { get; }
    public MessageBoxResult Show(string message)
    {
        Console.WriteLine(message);
        return MessageBoxResult.OK;
    }

    public MessageBoxResult Show(string message, string title)
    {
        Console.WriteLine($"\n==[ {title} ]================\n{message}");
        return MessageBoxResult.OK;
    }

    public MessageBoxResult Show(string message, string title, MessageBoxChoices choices)
    {
        Console.WriteLine($"\n==[ {title} ]================\n{message}");
        return AskFromConsole(choices);
    }

    private MessageBoxResult AskFromConsole(MessageBoxChoices choices)
    {
        var result = MessageBoxResult.Unknown;
        switch(choices)
        {
            case MessageBoxChoices.Unknown: return MessageBoxResult.OK;
            case MessageBoxChoices.OK: return MessageBoxResult.OK;
            
            case MessageBoxChoices.OKCancel:
                Console.WriteLine("Please answer: [O]k or [C]ancel: ");
                var answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.O)
                    {
                        return MessageBoxResult.OK;
                    }

                    if (answer.Key == ConsoleKey.C)
                    {
                        return MessageBoxResult.Cancel;
                    }

                    if (answer.Key == ConsoleKey.Escape)
                    {
                        throw new Exception("Escape pressed during AskFromConsole");
                    }
                }

            case MessageBoxChoices.AbortRetryIgnore:
                break;
            case MessageBoxChoices.YesNoCancel:
                Console.WriteLine("Please answer: [Y]es, [N]o, [C]ancel: ");
                answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.Y)
                    {
                        return MessageBoxResult.Yes;
                    }

                    if (answer.Key == ConsoleKey.N)
                    {
                        return MessageBoxResult.No;
                    }

                    if (answer.Key == ConsoleKey.Escape)
                    {
                        throw new Exception("Escape pressed during AskFromConsole");
                    }
                }

            case MessageBoxChoices.YesNo:
                Console.WriteLine("Please answer: [Y]es or [N]o: ");
                answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.Y)
                    {
                        return MessageBoxResult.Yes;
                    }

                    if (answer.Key == ConsoleKey.N)
                    {
                        return MessageBoxResult.No;
                    }

                    if (answer.Key == ConsoleKey.Escape)
                    {
                        throw new Exception("Escape pressed during AskFromConsole");
                    }
                }


            case MessageBoxChoices.RetryCancel:

            default:
                throw new ArgumentOutOfRangeException(nameof(choices), choices, null);
        }

        return MessageBoxResult.OK;
    }

    public MessageBoxResult Show(string message, string title, MessageBoxChoices choices, MessageBoxStatus status, MessageBoxDefault @default)
    {
        throw new NotImplementedException();
    }
}

public class ValidCombo<T> : IAct
    where T : class, IAct, new()
{
    public ArgumentNoun Noun;
    public ArgumentVerb Verb;

    public ValidCombo(ArgumentVerb verb, ArgumentNoun noun)
    {
        Verb = verb;
        Noun = noun;
    }

    public bool ReadyToAct(ArgumentSettings settings, out string reason)
    {
        reason = null;

        return true;
    }

    public ProcessorResult Act(ArgumentSettings settings)
    {
        return null;
    }

    public T Factory()
    {
        var actor = new T();
        return actor;
    }
}

public class ProcessorResult
{
}