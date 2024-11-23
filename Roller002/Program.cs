using MetX.Standard.Library.Encryption;

public class Program
{
// Displays a text chart and statistics for the dice rolls
    private static void DisplayStatistics(Dictionary<int, int> results)
    {
        var totalRolls = results.Values.Sum();
        var average = results.Select(kv => kv.Key * kv.Value).Sum() / (double) totalRolls;
        var min = results.Keys.First(k => results[k] == results.Values.Min());
        var max = results.Keys.First(k => results[k] == results.Values.Max());

        Console.WriteLine();
        Console.WriteLine("Statistics:");
        Console.WriteLine("-----------");
        Console.WriteLine($"Total Rolls: {totalRolls}");
        Console.WriteLine($"Average Roll: {average:F2}");
        Console.WriteLine($"Minimum Rolls: {results[min]} (Number {min})");
        Console.WriteLine($"Maximum Rolls: {results[max]} (Number {max})");
        Console.WriteLine($"Range (Max-Min): {results[max] - results[min]}");
    }

    private static void Main(string[] args)
    {
        // Roll a d20 10000 times and analyze the results
        var results = RollD20Statistics(10000);

        // Display the results
        DisplayStatistics(results);

        Console.WriteLine("-----------");
        for (var i = 0; i < 10; i++) Console.WriteLine(SuperRandom.NextInteger());

        Console.WriteLine("-----------");
        Console.WriteLine(SuperRandom.NextString(80, true, true, false, false));

        Console.WriteLine("-----------");
        using var ns = NoiseStream.StreamForFixedLength(100);
        using var sr = new StreamReader(ns);
        var noise = sr.ReadToEnd();
        Console.WriteLine(noise);
        Console.WriteLine(noise.Length);

        Console.WriteLine("-----------");
    }

// Rolls a 1d20 multiple times and keeps track of the results
    private static Dictionary<int, int> RollD20Statistics(int numberOfRolls)
    {
        var rollCounts = new Dictionary<int, int>();

        try
        {
            // Initialize the dictionary with keys from 1 to 20
            for (var i = 1; i <= 20; i++) rollCounts[i] = 0;

            for (var i = 0; i < numberOfRolls; i++)
            {
                var roll = SuperRandom.NextRoll(1, 20); // Roll a d20
                rollCounts[roll]++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return rollCounts;
    }
}