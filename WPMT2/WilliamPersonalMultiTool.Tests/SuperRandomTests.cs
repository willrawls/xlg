using System.Reflection.Metadata.Ecma335;
using MetX.Standard.Library;
using MetX.Standard.Library.Encryption;
using MetX.Standard.Library.Extensions;
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
