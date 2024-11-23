using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MetX.Standard.Library.Encryption;

public static class Primes
{
    /// <summary>
    ///     Some (overrideable) large prime numbers used by GenerateStandardSalt
    /// </summary>
    public static ulong[] LargePrimes =
    {
        7079336023289, 1000000007, 7079336023303, 1000000021, 7079336023307,
        7079336023349, 7079336023493, 7079336023531, 7079336023541, 7079336023607,
        7079336023667, 7079336023681, 7079340000049, 7079350000067, 7079360000143,
        7079370000149, 7079380000151, 7079390000219, 7079400000221, 7079410000249,
        7079420000267, 7079430000283, 10000000019, 20000021983, 30000044287,
        40000066459, 50000088877, 60000111377, 70000134157, 80000157649,
        90000182747, 100000205741, 10000000019, 30000000089, 30000000091,
        70000000237, 70000000243, 90000000313, 90000000331, 150000000551,
        170000000633, 190000000721, 190013000783, 570039002389, 950065003999,
        1710117007223, 2090143008823, 2090143008827, 2470169010437, 2850195012043,
        3230221013641, 3230221013647, 3230235013739, 3230235013741, 9690705041261,
        29072115123811, 35532585151319, 35532585151331, 48453525206369, 54913995233891,
        93676815398987, 100137285426509, 100137295426513, 700961067985709, 1502059431397991,
        1902608613104123, 2102883203957183, 2503432385663311, 2503432385663317, 2903981567369447,
        3705079930781707, 3705079930781711, 3705079942781759, 18525399713908873, 48166039256163107,
        55576199141726663, 70396518912853789, 77806678798417333, 77806678798417351, 85216838683980907,
        100037158455108023, 107447318340671579
    };

    /// <summary>
    ///     Some (overrideable) small prime numbers (1031 to 2477) used by GenerateStandardSalt
    /// </summary>
    public static int[] SmallPrimes =
    {
        1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097, 1103, 1109, 1117, 1123, 1129, 1151,
        1153, 1163, 1171, 1181, 1187, 1193, 1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279, 1283,
        1289, 1291, 1297, 1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367, 1373, 1381, 1399, 1409, 1423, 1427, 1429,
        1433, 1439, 1447, 1451, 1453, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499, 1511, 1523, 1531, 1543, 1549,
        1553, 1559, 1567, 1571, 1579, 1583, 1597, 1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637, 1657, 1663, 1667,
        1669, 1693, 1697, 1699, 1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759, 1777, 1783, 1787, 1789, 1801, 1811,
        1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877, 1879, 1889, 1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973,
        1979, 1987, 1993, 1997, 1999, 2003, 2011, 2017, 2027, 2029, 2039, 2053, 2063, 2069, 2081, 2083, 2087, 2089,
        2099, 2111, 2113, 2129, 2131, 2137, 2141, 2143, 2153, 2161, 2179, 2203, 2207, 2213, 2221, 2237, 2239, 2243,
        2251, 2267, 2269, 2273, 2281, 2287, 2293, 2297, 2309, 2311, 2333, 2339, 2341, 2347, 2351, 2357, 2371, 2377,
        2381, 2383, 2389, 2393, 2399, 2411, 2417, 2423, 2437, 2441, 2447, 2459, 2467, 2473, 2477
    };

    /// <summary>
    ///     Returns a random large prime number from the LargePrimes array
    /// </summary>
    /// <returns></returns>
    public static ulong ALargePrime()
    {
        var index = SuperRandom.NextInteger(0, LargePrimes.Length);
        return LargePrimes[index];
    }

    /// <summary>
    ///     Returns a random small prime number from the SmallPrimes array
    /// </summary>
    /// <returns></returns>
    public static int ASmallPrime()
    {
        var index = SuperRandom.NextUnsignedInteger(0, (uint) SmallPrimes.Length);
        return SmallPrimes[index];
    }

    public static void SetupLargePrimes(ulong startAt = 10000000000, uint numberToFind = 35)
    {
        LargePrimes = GenerateSomeLargePrimes(startAt, numberToFind).ToArray();
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
            current = primesNonSequential[primesNonSequential.Count - 1] + 1; // Jump by a significant amount to speed up generation
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