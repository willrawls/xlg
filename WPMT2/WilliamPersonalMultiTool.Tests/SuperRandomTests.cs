using System;
using System.IO;
using System.Linq;
using System.Threading;
using MetX.Standard.Library.Encryption;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Primary.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class SuperRandomTests
    {
        [TestMethod]
        [DataRow(127, 7)]
        [DataRow(128, 8)]
        [DataRow(-1, 32)]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 2)]
        [DataRow(4, 3)]
        [DataRow(int.MinValue, 32)]
        [DataRow(int.MaxValue, 31)]
        public void BitCount_Simple(int integer, int expected)
        {
            // NOTE TO SELF, Move to MetX.Tests
            Assert.AreEqual(expected, SuperRandom.BitsUsed(integer));

        }

        [TestMethod]
        public void NextUnsignedInteger_InRange()
        {
            // NOTE TO SELF, Move to MetX.Tests
            for (uint min = 0; min < 256; min++)
            {
                for (var max = min + 101; max < 101; max++)
                {
                    for (uint iteration = 0; iteration < 102; iteration++)
                    {
                        var number = SuperRandom.NextUnsignedInteger(min + iteration, max + iteration);
                        Assert.IsTrue(number >= min + iteration);
                        Assert.IsTrue(number <= max + iteration);
                    }
                }
            }
        }

        [TestMethod]
        public void NextInteger_InRange()
        {
            // NOTE TO SELF, Move to MetX.Tests
            for (var min = 0; min < 256; min++)
            {
                for (var max = min + 101; max < 101; max++)
                {
                    for (var iteration = 0; iteration < 102; iteration++)
                    {
                        var number = SuperRandom.NextInteger(min + iteration, max + iteration);
                        Assert.IsTrue(number >= min + iteration);
                        Assert.IsTrue(number <= max + iteration);
                    }
                }
            }
        }

        [TestMethod]
        public void NextString_LetterAndDigits()
        {
            for (var length = 1; length < 256; length++)
            {
                var result  = SuperRandom.NextString(length, true, true, false, false);
                Assert.AreEqual(length, result.Length);
                Assert.IsTrue(result.All(x => 
                    x is >= '0' and <= '9' or >= 'a' and <= 'z' or >= 'A' and <= 'Z'
                ), $"Length {length}: '{result}'");
            }
        }

        [TestMethod]
        public void NextString_LetterAndDigitsAndSpaceAndSymbols()
        {
            for (var length = 1; length < 256; length++)
            {
                var result  = SuperRandom.NextString(length, true, true, true, true);
                Assert.AreEqual(length, result.Length);
                var message = $"Length {length}: '{result}'";
                Assert.IsTrue(result.All(c => char.IsNumber(c) || char.IsLetter(c) || char.IsSymbol(c) || c == ' '), message);
                Console.WriteLine(message);
            }
        }

        [TestMethod]
        public void NextString_Letters()
        {
            for (var length = 1; length < 256; length++)
            {
                var result  = SuperRandom.NextString(length, true, false, false, false);
                Assert.AreEqual(length, result.Length);
                var message = $"Length {length}: '{result}'";
                Assert.IsTrue(result.All(x => x is >= 'a' and <= 'z' or >= 'A' and <= 'Z'), message);
                Console.WriteLine(message);
            }
        }

        [TestMethod]
        public void NextGuid()
        {
            for (var length = 1; length < 256; length++)
            {
                Guid actual  = SuperRandom.NextGuid();
                Assert.AreNotEqual(Guid.Empty, actual);
                var message = $"{actual:N}";
                Console.WriteLine(message);
            }
        }

        [TestMethod]
        public void NextString_Digits()
        {
            for (var length = 1; length < 256; length++)
            {
                var result  = SuperRandom.NextString(length, false, true, false, false);
                Assert.AreEqual(length, result.Length);
                Assert.IsTrue(result.All(x => x is >= '0' and <= '9'), $"Length {length}: '{result}'");
            }
        }

        [TestMethod]
        public void NextHexString()
        {
            for (var length = 1; length < 256; length++)
            {
                var result  = SuperRandom.NextHexString(length);
                Assert.AreEqual(length * 2, result.Length);
                Assert.IsTrue(result.All(
                        x => x is >= '0' and <= '9' or >= 'A' and <= 'E'), 
                    $"Length {length}: '{result}'");
            }
        }

        [TestMethod]
        public void NextSaltedHash_SameStringSameHashButNotTheSameAsNextHash()
        {
            var expected = SuperRandom.NextSaltedHash("Fred");
            var actual1 = SuperRandom.NextSaltedHash("Fred");
            var actual2 = SuperRandom.NextHash("Fred");

            Assert.IsTrue(actual1.IsNotEmpty());
            Assert.IsTrue(actual2.IsNotEmpty());
            Assert.AreEqual(expected, actual1);
            Assert.AreNotEqual(expected, actual2);
        }

        [TestMethod]
        public void NextSaltedHash_SameStringSameHash()
        {
            var expected = SuperRandom.NextSaltedHash("Fred");
            var actual = SuperRandom.NextSaltedHash("Fred");

            Assert.IsTrue(actual.IsNotEmpty());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProgressiveSaltShaker_NextSaltedHash_SameStringSameHash()
        {
            byte[] saltBytes = { 6, 7, 8, 9 };
            SuperRandom.FillSaltShaker(saltBytes);
            var expected = SuperRandom.NextSaltedHash("Fred");
            var actual = SuperRandom.NextSaltedHash("Fred");

            Assert.IsTrue(actual.IsNotEmpty());
            Assert.AreEqual(expected, actual);

            SuperRandom.FillSaltShaker(saltBytes);
            expected = SuperRandom.NextSaltedHash("Fred");
            Assert.AreEqual(expected, actual);

            actual = SuperRandom.NextSaltedHash("Fred");

            Assert.IsTrue(actual.IsNotEmpty());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NextSaltedHash_SameStringWithDifferentCaseSameHash()
        {
            var expected = SuperRandom.NextSaltedHash("Fred");
            var actual = SuperRandom.NextSaltedHash("fred");

            Assert.IsTrue(actual.IsNotEmpty());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NextSaltedHash_DifferentStringDifferentHash()
        {
            var expected = SuperRandom.NextSaltedHash("Fred");
            
            var actual = SuperRandom.NextSaltedHash("george");
            Assert.IsNotNull(actual);
            Assert.AreNotEqual("", actual);
            Assert.AreNotEqual(expected, actual);

            actual = SuperRandom.NextSaltedHash("Fred");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NextRoll_D20()
        {
            for(var i=0; i < 10000; i++)
            {
                var actual = SuperRandom.NextRoll(1, 20);
                Assert.IsTrue(actual is >= 1 and <= 20, actual.ToString());
            }
        }

        [TestMethod]
        public void GenerateNoiseBytesForTwoSeconds()
        {
            var endAfter = DateTime.Now.AddSeconds(2);
            using var noiseStream = new SuperRandom.NoiseStream(endAfter);
            var bytesWritten = 0;
            for (var i = 0; i < 10000000; i++)
            {
                byte[] buffer = new byte[100];
                int actual = noiseStream.Read(buffer, 0, 100);
                var milliseconds = endAfter.Subtract(DateTime.Now).TotalMilliseconds;
                if(actual != 0)
                {
                    bytesWritten += 100;
                    Assert.AreEqual(100, actual);
                    Assert.IsTrue(buffer.Any(b => b != 0), milliseconds.ToString());
                    Assert.IsTrue(milliseconds > 1, milliseconds.ToString());
                }
                else
                {
                    Assert.IsTrue(milliseconds < 10, milliseconds.ToString());
                    break;
                }
                Thread.Sleep(10);
            }

            var millisecondsLeft = DateTime.Now.Subtract(endAfter);
            Console.WriteLine($"bytesWritten = {bytesWritten}");
            Console.WriteLine($"Milliseconds 'left' = {millisecondsLeft}");
        }

        [TestMethod]
        public void Repeating_Simple()
        {
            var data = new byte[]{ 1, 2, 3, 4, 5 };
            var expected = new byte[]{ 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1 };
            var actual = SuperRandom.Repeating(data, 11);

            Assert.IsNotNull(actual);

            for (var i = 0; i < expected.Length; i++) 
                Assert.AreEqual(expected[i], actual[i]);
        }

    }
}
