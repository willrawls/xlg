using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MetX.Standard.Strings;

namespace MetX.Standard.Library.Encryption
{
    public static class SuperRandom
    {
        private static RNGCryptoServiceProvider _provider;
        private static HashAlgorithm _shaProvider;

        public static int StartSaltingAtIndex;
        public static byte[] Salt;

        static SuperRandom()
        {
            ResetProvider(null);
        }

        public static void ResetProvider(byte[] saltBytes)
        {
            _provider = new RNGCryptoServiceProvider();
            _shaProvider = SHA256.Create();
            FillSaltShaker(saltBytes);
        }

        public static string NextDigits(int length)
        {
            if (length <= 0)
                return "";

            var value = "";

            for (var i = 0; i < length; length++)
            {
                value += (((byte) NextChar()) % 10).ToString();
            }

            return value;
        }

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

    public static int NextInteger()
        {
            var randomBytes = NextBytes(sizeof(int));
            return BitConverter.ToInt32(randomBytes, 0);
        }

        public static uint NextUnsignedInteger()
        {
            var randomBytes = NextBytes(sizeof(uint), false);
            return BitConverter.ToUInt32(randomBytes, 0);
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

        public static char NextChar()
        {
            byte randomByte;
            do
            {
                randomByte = NextByte();
            } while (randomByte is < 32 or > 126);

            return (char) randomByte;
        }
        
        // One hex is 2 chars long, so length * 2 is produced
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

                sb.Append((char)randomChar);
            }
            return sb.ToString();
        }

        public static uint NextRoll(int dice, int sides)
        {
            if (dice < 1 || sides < 1)
                return 1;

            uint result = 0;
            for (var i = 0; i < dice; i++) 
                result += NextUnsignedInteger((uint) 1, (uint) sides);

            return result;
        }

        public static long NextLong()
        {
            var randomBytes = NextBytes(sizeof(long));
            return BitConverter.ToInt64(randomBytes, 0);
        }

        public static string NextHash(byte[] bytes)
        {
            return _shaProvider.ComputeHash(bytes).AsString();
        }

        public static string NextHash(IEnumerable<byte> bytes)
        {
            return _shaProvider.ComputeHash(bytes.ToArray()).AsString();
        }

        public static string NextSaltedHash(string data)
        {
            return data.IsEmpty() ? "" : NextHash(data.ToLower(), SaltShaker);
        }

        public static string NextHash(string data, Func<byte[], IEnumerable<byte>> shaker = null)
        {
            if (data.IsEmpty())
                return "";

            var bytes = _shaProvider
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
                FillSaltShaker();
                return;
            }
            var randomLength = NextInteger(1024, 2048);

            StartSaltingAtIndex = 0;
            Salt = Repeating(salt, randomLength);
        }

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

        public static byte NextByte()
        {
            var buffer = new byte[1];
            _provider.GetBytes(buffer);
            return buffer[0];
        }

        public static byte[] NextBytes(int bytesNumber, bool zeroTheLastByte = false)
        {
            if (bytesNumber < 1)
                return Array.Empty<byte>();

            var buffer = new byte[bytesNumber];
            _provider.GetBytes(buffer);

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

        public static Guid NextGuid()
        {
            return new Guid(NextBytes(16, true));
        }

        public class NoiseStream : Stream
        {
            public DateTime Until = DateTime.MinValue;
            private long _length = long.MaxValue;
            private long _lengthLeft = long.MaxValue;

            public NoiseStream(DateTime until)
            {
                Until = until;
            }

            public NoiseStream(long length)
            {
                _length = length;
                _lengthLeft = _length;
            }

            public override void Flush()
            {
                ResetProvider(null);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (Until == DateTime.MinValue)
                {
                    if (_lengthLeft > 0)
                    {
                        _lengthLeft -= count;
                    }
                }
                else if (Until.Subtract(DateTime.Now).TotalMilliseconds < 1)
                    return 0;

                var randomBytes = SuperRandom.NextBytes(count);
                Array.Copy(randomBytes, 0, buffer, offset, count - offset);

                if (_lengthLeft < 0)
                {
                    var bytesToClear = (int) Math.Abs(_lengthLeft);
                    Array.Clear(buffer, buffer.Length - bytesToClear, bytesToClear);
                    _lengthLeft = 0;
                    return count - bytesToClear;
                }

                return count;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;
            }

            public override void SetLength(long value)
            {
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                
            }

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => _length;
            public override long Position { get; set; } = 0;
        }
    }
}