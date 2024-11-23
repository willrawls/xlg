using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MetX.Standard.Library.Encryption;

public static class SuperRandom
{
    private static RNGCryptoServiceProvider _provider;

    public static byte[] Salt;

    /// <summary>
    ///     A large number used as the initial spice (a number that increments and is used to modify the generated number
    /// </summary>
    public static ulong Spice = (ulong) DateTime.UtcNow.Ticks % int.MaxValue;


    /// <summary>
    ///     Used during setting up the salt shaker
    /// </summary>
    public static int StartSaltingAtIndex;

    private static readonly object SyncRoot = new();

    private static byte[] toArray;


    /// <summary>
    ///     The function that spices up the next salted byte
    /// </summary>
    public static Func<byte, byte> SpiceBlender => DefaultSpiceBlender;

    /// <summary>
    ///     Calls ResetProvider
    /// </summary>
    static SuperRandom()
    {
        ResetProvider();
    }

    /// <summary>
    ///     Given an integer, returns the number of bits that are set to 1
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int BitsUsed(int n)
    {
        int count = 0, i;
        if (n == 0) return 0;
        for (i = 0; i < 32; i++)
        {
            var i1 = (1 << i) & n;
            if (i1 != 0)
                count = i;
        }

        return ++count;
    }

    /// <summary>
    ///     Given an unsigned integer, returns the number of bits that are set to 1
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int BitsUsed(uint n)
    {
        int count = 0, i;
        if (n == 0) return 0;
        for (i = 0; i < 32; i++)
        {
            var i1 = (1 << i) & n;
            if (i1 != 0)
                count = i;
        }

        return ++count;
    }

    /// <summary>
    ///     The default function used by SpiceBlender(byte). Combines the byte provided with the value of Spice, increments
    ///     Spice, and returns the modulo 254 of that value as a byte
    /// </summary>
    /// <param name="byteIn"></param>
    /// <returns></returns>
    private static byte DefaultSpiceBlender(byte byteIn)
    {
        return (byte) ((byte) (byteIn + Spice++) % 254);
    }

    /// <summary>
    ///     Given an enumerable of bytes, returns an altered enumerable of spiced bytes
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static IEnumerable<byte> EnumerableSpiceBlender(IEnumerable<byte> bytes)
    {
        foreach (var byteIn in bytes)
        {
            var byteOut = SpiceBlender(byteIn);
            yield return byteOut;
        }
    }

    /// <summary>
    ///     If salt is null, calls FillSaltShaker(). Otherwise, sets Salt to a random length of bytes from salt, repeating if
    ///     salt is not big enough to cover the random length assigned
    /// </summary>
    /// <param name="salt"></param>
    public static void FillSaltShaker(byte[] salt)
    {
        if (salt == null)
        {
            FillSaltShaker();
            return;
        }

        var randomLength = NextUnsignedInteger(1024, 2048);

        StartSaltingAtIndex = 0;
        Salt = Repeating(salt, (int) randomLength);
    }

    /// <summary>
    ///     Salt must already be set. Fills the salt shaker based on the current Salt starting at index startSaltingAtIndex of
    ///     Salt
    /// </summary>
    /// <param name="startSaltingAtIndex"></param>
    public static void FillSaltShaker(int startSaltingAtIndex = 0)
    {
        StartSaltingAtIndex = startSaltingAtIndex;
        var aSmallPrime = Primes.ASmallPrime();
        var randomLength = NextUnsignedInteger(aSmallPrime, aSmallPrime * 2);
        Salt = NextBytes((int) randomLength);
    }

    /// <summary>
    ///     Used at initialization, sets Salt to a random byte array of length of one of the SmallPrimes
    /// </summary>
    /// <returns></returns>
    public static byte[] GenerateRegularSalt()
    {
        return NextBytes(Primes.ASmallPrime());
    }

    /// <summary>
    ///     Returns the next spiced random byte
    /// </summary>
    /// <returns></returns>
    public static byte NextByte()
    {
        var buffer = new byte[1];
        _provider.GetBytes(buffer);
        return SpiceBlender(buffer[0]);
    }

    /// <summary>
    ///     Returns a spiced byte array numberOfBytes long.
    /// </summary>
    /// <param name="numberOfBytes"></param>
    /// <param name="zeroTheLastByte">True to insure the last byte is a 0</param>
    /// <returns></returns>
    public static byte[] NextBytes(int numberOfBytes, bool zeroTheLastByte = false)
    {
        if (numberOfBytes < 1)
            return Array.Empty<byte>();

        var buffer = new byte[numberOfBytes];
        _provider.GetBytes(buffer);

        for (var i = 0; i < numberOfBytes; i++)
        {
            buffer[i] = SpiceBlender(buffer[i]);
            if (buffer[i] == 0)
                buffer[i] = (byte) NextChar();
        }

        if (zeroTheLastByte)
            buffer[numberOfBytes - 1] = 0;

        return buffer;
    }

    /// <summary>
    ///     Returns the next spiced character
    /// </summary>
    /// <returns></returns>
    public static char NextChar()
    {
        byte randomByte;
        do
        {
            randomByte = NextByte();
        } while (randomByte is < 32 or > 126);

        return (char) randomByte;
    }

    /// <summary>
    ///     Generates a string of numbers length long
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string NextDigits(int length)
    {
        if (length <= 0)
            return "";

        var value = "";

        for (var i = 0; i < length; length++) value += ((byte) NextChar() % 10).ToString();

        return value;
    }

    /// <summary>
    ///     Generates a spiced Guid
    /// </summary>
    /// <returns></returns>
    public static Guid NextGuid()
    {
        return new Guid(NextBytes(16, true));
    }


    /// <summary>
    ///     One hex value is 2 chars long (0x00 to 0xFF), so a string of numbers length * 2 long is produced.
    ///     Includes only 0-9 and A-F
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string NextHexString(int length)
    {
        length *= 2;
        var sb = new StringBuilder(length);

        for (var i = 0; i < length; i++)
        {
            char randomChar;
            do
            {
                randomChar = NextChar();
            } while (!char.IsNumber(randomChar) && randomChar is < 'A' or > 'E');

            sb.Append(randomChar);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Returns an integer number in the range of minValue to maxExclusiveValue    (So 1-10 generates a long from 1-9)
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static int NextInteger(int minValue, int maxExclusiveValue)
    {
        if (minValue >= maxExclusiveValue)
            throw new ArgumentOutOfRangeException(nameof(minValue),
                "minValue must be lower than maxExclusiveValue");

        var diff = maxExclusiveValue - minValue;

        int value;
        do
        {
            value = (NextInteger() + minValue) % maxExclusiveValue;
        } while (value < minValue && value > maxExclusiveValue);

        return minValue + value % diff;
    }

    /// <summary>
    ///     Returns the next spiced integer
    /// </summary>
    /// <returns></returns>
    public static int NextInteger()
    {
        var randomBytes = NextBytes(sizeof(int));
        return BitConverter.ToInt32(randomBytes, 0);
    }

    /// <summary>
    ///     Returns a long number in the range of minValue to maxExclusiveValue    (So 1-10 generates a long from 1-9)
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static long NextLong(long minValue, long maxExclusiveValue)
    {
        if (minValue >= maxExclusiveValue)
            throw new ArgumentOutOfRangeException(nameof(minValue),
                "minValue must be lower than maxExclusiveValue");

        long value;
        do
        {
            value = Math.Abs(NextLong()) % maxExclusiveValue;
            if (value < minValue)
                value += minValue - value;
        } while (value < minValue || value > maxExclusiveValue);

        return value;
    }

    /// <summary>
    ///     Returns the next spiced long
    /// </summary>
    /// <returns></returns>
    public static long NextLong()
    {
        var randomBytes = NextBytes(sizeof(long));
        return BitConverter.ToInt64(randomBytes, 0);
    }

    /// <summary>
    ///     Rolls 'dice' number of dice with a number of 'sides' (say 1, 20 for 1 20 sided die) and returns the result
    /// </summary>
    /// <param name="dice"></param>
    /// <param name="sides"></param>
    /// <returns></returns>
    public static int NextRoll(int dice, int sides)
    {
        var result = 0;
        if (dice < 1 || sides < 1)
            return 1;

        for (var i = 0; i < dice; i++)
            result += (int) NextUnsignedInteger(1, sides + 1);

        return result;
    }

    /// <summary>
    ///     Produces a spiced string 'length' long.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="includeLetters">Output string may contain letters</param>
    /// <param name="includeNumbers">Output string may contain numbers</param>
    /// <param name="includeSymbols">Output string may contain symbols like $</param>
    /// <param name="includeSpace">Output string may contain one or more spaces</param>
    /// <returns></returns>
    public static string NextString(int length, 
        bool includeLetters = true, 
        bool includeNumbers = true, 
        bool includeSymbols = true,
        bool includeSpace = true)
    {
        if (length < 1)
            return "";

        var sb = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            bool valid;
            char c;
            do
            {
                c = NextChar();
                valid = (includeLetters && char.IsLetter(c))
                        || (includeNumbers && char.IsNumber(c))
                        || (includeSpace && c == ' ')
                        || (includeSymbols && char.IsSymbol(c));
            } while (!valid);

            sb.Append(c);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Returns the next spiced unsigned integer
    /// </summary>
    /// <returns></returns>
    public static uint NextUnsignedInteger()
    {
        var randomBytes = NextBytes(sizeof(uint));
        return BitConverter.ToUInt32(randomBytes, 0);
    }

    /// <summary>
    ///     Returns the next spiced unsigned integer in the range of minValue to maxExclusiveValue - 1. So min=1,max=10 would
    ///     generate a number from 1 to 9
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static uint NextUnsignedInteger(uint minValue, uint maxExclusiveValue)
    {
        if (minValue >= maxExclusiveValue)
            throw new ArgumentOutOfRangeException(nameof(minValue),
                "minValue must be lower than maxExclusiveValue");

        var diff = maxExclusiveValue - minValue;
        uint value = 0;
        do
        {
            value = NextUnsignedInteger() + minValue;
            if (value > diff)
                value = value % diff + minValue;
        } while (value < minValue && value >= maxExclusiveValue);

        return value;
    }

    /// <summary>
    ///     Returns the next spiced unsigned integer in the range of minValue to maxExclusiveValue - 1. So min=1,max=10 would
    ///     generate a number from 1 to 9
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static uint NextUnsignedInteger(int minValue, int maxExclusiveValue)
    {
        return NextUnsignedInteger((uint) minValue, (uint) maxExclusiveValue);
    }

    /// <summary>
    ///     Returns an unsigned long number in the range of minValue to maxExclusiveValue    (So 1-10 generates a long from
    ///     1-9)
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ulong NextUnsignedLong(ulong minValue, ulong maxExclusiveValue)
    {
        if (minValue >= maxExclusiveValue)
            throw new ArgumentOutOfRangeException(nameof(minValue),
                "minValue must be lower than maxExclusiveValue");

        ulong value;
        do
        {
            value = NextUnsignedLong() % maxExclusiveValue;
            if (value < minValue)
                value += minValue - value;
        } while (value < minValue || value > maxExclusiveValue);

        return value;
    }

    /// <summary>
    ///     Returns the next spiced unsigned long
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxExclusiveValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ulong NextUnsignedLong()
    {
        var randomBytes = NextBytes(sizeof(ulong));
        return BitConverter.ToUInt64(randomBytes, 0);
    }

    /// <summary>
    ///     Given a byte array, it produces a byte array targetLength long. If padding is needed the bytes are repeated as many
    ///     times as it takes to fill the output array
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="targetLength"></param>
    /// <returns></returns>
    public static byte[] Repeating(byte[] bytes, int targetLength)
    {
        if (bytes == null) return null;
        if (bytes.Length == targetLength) return bytes;
        if (bytes.Length > targetLength) return bytes.Take(targetLength).ToArray();

        var repeated = new byte[targetLength];
        for (int i = 0, j = 0; i < targetLength; i++)
        {
            if (j > bytes.Length - 1)
                j = 0;
            repeated[i] = bytes[j++];
        }

        return repeated;
    }

    /// <summary>
    ///     Resets the random providers and fills the salt shaker
    /// </summary>
    /// <param name="saltBytes"></param>
    public static void ResetProvider(byte[] saltBytes = null)
    {
        lock (SyncRoot)
        {
            _provider = new RNGCryptoServiceProvider();
            FillSaltShaker(saltBytes);
        }
    }

    /// <summary>
    ///     Bit shift the byte count times to the left
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static byte RotateLeft(this byte value, int count)
    {
        return (byte) ((value << count) | (value >> (32 - count)));
    }

    /// <summary>
    ///     Bit shift the byte count times to the right
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static byte RotateRight(this byte value, int count)
    {
        return (byte) ((byte) (value >> count) | (value << (32 - count)));
    }

    /// <summary>
    ///     Given an enumerable byte array, generates a salted enumerable byte array based on Salt
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static IEnumerable<byte> SaltShaker(IEnumerable<byte> bytes)
    {
        var byteArray = bytes.ToArray();
        for (var i = 0; i < StartSaltingAtIndex && i < byteArray.Length; i++) yield return byteArray[i];
        for (var i = StartSaltingAtIndex; i < byteArray.Length; i++)
            yield return byteArray[i] ^= Salt[i - StartSaltingAtIndex];
    }
}