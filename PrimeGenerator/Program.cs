using System.Numerics;
using MetX.Standard.Library.Encryption;

namespace PrimeGenerator;

class Program
{
    static void Main()
    {
        Primes.LargePrimes = GenerateSomeLargePrimes().ToArray();

        // Convert the primes into a comma-delimited string
        var primesCommaDelimited = string.Join(", ", Primes.LargePrimes);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(primesCommaDelimited);
    }

    public static IEnumerable<ulong> GenerateSomeLargePrimes(ulong startAt = 10000000000, uint numberToFind = 35)
    {
        var primesNonSequential = new List<ulong>();
        var current = startAt;
        int previousFirstDigit = 0;
        var x = 0;
        while (primesNonSequential.Count < numberToFind)
        {
            var primes = NextPrimes(current, 10);
            foreach(var bigInteger in primes)
                primesNonSequential.Add((ulong) bigInteger);
            current = primesNonSequential[^1] + 1; // Jump by a significant amount to speed up generation
            if (x > Primes.SmallPrimes.Length - 1) x = 0;
        }

        return primesNonSequential;
    }

    public static List<BigInteger> NextPrimes(BigInteger start, int primesToFind)
    {
        var primesFound = new List<BigInteger>();
        if (start < 2)
        {
            primesFound.Add(2);
            return primesFound;
        }

        var candidate = start.IsEven ? start + 1 : start + 2;

        // Check candidates in parallel for faster results
        var numberFound = 0;
        while (primesFound.Count < primesToFind)
        {
            var tasks = new List<Task<bool>>();
            for (var i = 0; i < Environment.ProcessorCount; i++)
            {
                var currentCandidate = candidate + i * 2; // Skip even numbers
                tasks.Add(Task.Run(() => IsPrime(currentCandidate)));
            }

            Task.WaitAll(tasks.ToArray());

            for (var i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Result)
                {
                    numberFound++;
                    primesFound.Add(candidate + i * 2); // prime found
                    Console.Write($"{candidate + i * 2},");
                }
            }

            var x = primesFound.Count > 0 ? primesFound[0] : 1000000;
            candidate += primesToFind * 2 + x; // Jump forward by the number of tasks
            if(numberFound % 5 == 0)
            {
                numberFound = 0;
            }
        }

        return primesFound;
    }

    static bool IsPrime(BigInteger number)
    {
        if (number < 2) return false;
        if (number == 2 || number == 3) return true;
        if (number.IsEven) return false;

        var limit = Sqrt(number);
        for (BigInteger i = 3; i <= limit; i += 2)
        {
            if (number % i == 0)
                return false;
        }
        return true;
    }

    static BigInteger Sqrt(BigInteger n)
    {
        if (n == 0) return 0;
        if (n > 0)
        {
            var bitLength = (BigInteger)Math.Ceiling(BigInteger.Log10(n) / BigInteger.Log10(2));
            var root = BigInteger.One << (int)(bitLength / 2);

            while (!IsPerfectSquare(root, n))
            {
                root += n / root;
                root /= 2;
            }
            return root;
        }
        throw new ArgumentException("NaN");
    }

    static bool IsPerfectSquare(BigInteger root, BigInteger n)
    {
        return root * root <= n && (root + 1) * (root + 1) > n;
    }
}