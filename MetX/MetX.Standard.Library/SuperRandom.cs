using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MetX.Standard.Library
{
    public static class SuperRandom
    {
        private static readonly RNGCryptoServiceProvider Provider;
        private static readonly HashAlgorithm SHAProvider;

        public static int StartSaltingAtIndex;
        public static byte[] Salt;

        static SuperRandom()
        {
            Provider = new RNGCryptoServiceProvider();
            SHAProvider = SHA256.Create();
            FillSaltShaker();
        }

        public static long NextLong(long minValue, long maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException(nameof(minValue),
                    "minValue must be lower than maxExclusiveValue");

            var diff = maxExclusiveValue - minValue;
            var upperBound = long.MaxValue / diff * diff;

            long value;
            do
            {
                value = NextLong();
            } while (value >= upperBound);

            return minValue + value % diff;
        }

        public static int NextInteger(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException(nameof(minValue),
                    "minValue must be lower than maxExclusiveValue");

            var diff = maxExclusiveValue - minValue;
            var upperBound = int.MaxValue / diff * diff;

            int value;
            do
            {
                value = NextInteger();
            } while (value >= upperBound);

            return minValue + value % diff;
        }

        public static uint NextUnsignedInteger(uint minValue, uint maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException(nameof(minValue),
                    "minValue must be lower than maxExclusiveValue");

            var diff = maxExclusiveValue - minValue;
            var value = NextUnsignedInteger();
            if (value > diff)
                value %= diff;

            return minValue + value;
        }

        public static int BitsUsed(int n)
        {
            int count = 0, i;
            if(n==0) return 0;
            for(i=0; i < 32; i++)
            {
                var i1 = (1 << i) & n;
                if( i1 != 0)
                    count = i;
            }
            return ++count;
        }

        public static int BitsUsed(uint n)
        {
            int count = 0, i;
            if(n==0) return 0;
            for(i=0; i < 32; i++)
            {
                var i1 = (1 << i) & n;
                if( i1 != 0)
                    count = i;
            }
            return ++count;
        }

        public static string NextString(int length, bool includeLetters, bool includeNumbers, bool includeSymbols, bool includeSpace)
        {
            if (length < 1)
                return "";

            var bytes = NextBytes(sizeof(char) * length * 100);
            var result = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] >= 127)
                    bytes[i] -= 127;
                if (includeLetters && bytes[i] >= 'a' && bytes[i] <= 'z'
                    || bytes[i] >= 'A' && bytes[i] <= 'Z')
                    result += bytes[i];
                else if (includeNumbers && bytes[i] >= '0' && bytes[i] <= '9')
                    result += bytes[i];
                else if (includeSpace && bytes[i] == 32)
                    result += bytes[i];
                else if (includeSymbols
                         && (bytes[i] >= '!' && bytes[i] <= '/'
                             || bytes[i] >= ':' && bytes[i] <= '@'
                             || bytes[i] >= '[' && bytes[i] <= '_'))
                    result += bytes[i];
            }

            return result;
        }

        public static char NextChar()
        {
            var randomBytes = NextBytes(sizeof(char));
            return BitConverter.ToChar(randomBytes, 0);
        }

        public static int NextRoll(int dice, int sides)
        {
            if (dice < 1 || sides < 1)
                return 1;

            var result = 0;
            for (var i = 0; i < dice; i++) result += NextInteger(1, sides);

            return result;
        }

        public static long NextLong()
        {
            var randomBytes = NextBytes(sizeof(long));
            return BitConverter.ToInt64(randomBytes, 0);
        }

        public static string NextHash(IEnumerable<byte> bytes)
        {
            return SHAProvider.ComputeHash(bytes.ToArray()).AsString();
        }

        public static string NextSaltedHash(string data)
        {
            return NextHash(data, SaltShaker);
        }

        public static string NextHash(string data, Func<byte[], IEnumerable<byte>> shaker = null)
        {
            var bytes = SHAProvider
                .ComputeHash(data
                    .ToCharArray()
                    .Select(c => (byte) c)
                    .ToArray());

            if (shaker == null)
                return NextHash(bytes);

            var seasonedBytes = shaker(bytes);
            return NextHash(seasonedBytes);
        }

        public static void FillSaltShaker(byte[] salt)
        {
            if(salt == null)
            {
                var randomLength = NextInteger(128, 1024);
                Salt = NextBytes(randomLength);
            }

            StartSaltingAtIndex = 0;
            Array.Copy(salt, Salt, 0);
        }

        public static void FillSaltShaker(int startSaltingAtIndex = 0)
        {
            StartSaltingAtIndex = startSaltingAtIndex;
            var randomLength = NextInteger(1024, 2048);
            Salt = NextBytes(randomLength);
        }

        public static IEnumerable<byte> SaltShaker(IEnumerable<byte> bytes)
        {
            var byteArray = bytes.ToArray();
            for (var i = 0; i < StartSaltingAtIndex && i < byteArray.Length; i++) yield return byteArray[i];
            for (var i = StartSaltingAtIndex; i < byteArray.Length; i++)
                yield return byteArray[i] ^= Salt[i - StartSaltingAtIndex];
        }
        
        public static Func<byte, byte> SpiceBlender => DefaultSpiceBlender;
        private static byte DefaultSpiceBlender(byte byteIn)
        {
            return byteIn.RotateLeft(3);
        }
        public static IEnumerable<byte> SpiceBlendShaker(IEnumerable<byte> bytes)
        {
            foreach(var byteIn in bytes)
            {
                var byteOut = SpiceBlender(byteIn);
                yield return byteOut;
            }
        }

        public static byte RotateLeft(this byte value, int count)
        {
            return (byte) ((value << count) | (value >> (32 - count)));
        }

        public static byte RotateRight(this byte value, int count)
        {
            return (byte) ((byte) (value >> count) | (value << (32 - count)));
        }

        public static uint NextUnsignedInteger()
        {
            var randomBytes = NextBytes(sizeof(uint), false);
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        public static byte[] NextBytes(int bytesNumber, bool zeroTheLastByte = false)
        {
            if (bytesNumber < 1)
                return new byte[0];

            var buffer = new byte[bytesNumber];
            Provider.GetBytes(buffer);

            for (var i = 0; i < bytesNumber; i++)
            {
                if (buffer[i] == 0)
                    buffer[i] = (byte) NextChar();
            }

            if (zeroTheLastByte)
                buffer[bytesNumber-1] = 0;

            return buffer;
        }

        public static string AsString(this byte[] arrInput)
        {
            int i;
            var result = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++) result.Append(arrInput[i].ToString("X2"));
            return result.ToString();
        }
    }
}