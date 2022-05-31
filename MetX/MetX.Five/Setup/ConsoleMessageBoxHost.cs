using System;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Five.Setup;

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

    public MessageBoxResult Show(string message, string title, MessageBoxChoices choices, MessageBoxStatus status,
        MessageBoxDefault @default)
    {
        throw new NotImplementedException();
    }

    private MessageBoxResult AskFromConsole(MessageBoxChoices choices)
    {
        var result = MessageBoxResult.Unknown;
        switch (choices)
        {
            case MessageBoxChoices.Unknown: return MessageBoxResult.OK;
            case MessageBoxChoices.OK: return MessageBoxResult.OK;

            case MessageBoxChoices.OKCancel:
                Console.WriteLine("Please answer: [O]k or [C]ancel: ");
                var answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.O) return MessageBoxResult.OK;

                    if (answer.Key == ConsoleKey.C) return MessageBoxResult.Cancel;

                    if (answer.Key == ConsoleKey.Escape) throw new Exception("Escape pressed during AskFromConsole");
                }

            case MessageBoxChoices.AbortRetryIgnore:
                break;
            case MessageBoxChoices.YesNoCancel:
                Console.WriteLine("Please answer: [Y]es, [N]o, [C]ancel: ");
                answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.Y) return MessageBoxResult.Yes;

                    if (answer.Key == ConsoleKey.N) return MessageBoxResult.No;

                    if (answer.Key == ConsoleKey.Escape) throw new Exception("Escape pressed during AskFromConsole");
                }

            case MessageBoxChoices.YesNo:
                Console.WriteLine("Please answer: [Y]es or [N]o: ");
                answer = Console.ReadKey();
                while (true)
                {
                    if (answer.Key is ConsoleKey.Y) return MessageBoxResult.Yes;

                    if (answer.Key == ConsoleKey.N) return MessageBoxResult.No;

                    if (answer.Key == ConsoleKey.Escape) throw new Exception("Escape pressed during AskFromConsole");
                }


            case MessageBoxChoices.RetryCancel:

            default:
                throw new ArgumentOutOfRangeException(nameof(choices), choices, null);
        }

        return MessageBoxResult.OK;
    }
}