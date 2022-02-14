using System.Linq;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable StringLiteralTypo

namespace MetX.Tests.Standard.Library
{
    [TestClass]
    public class TokenizerTests
    {
        private readonly string _sample = "Fred goes home";

        [TestMethod]
        public void TokensBeforeLast_Simple_path()
        {
            var data = @"a\b\c\d.txt";
            var actual = data.TokensBeforeLast(@"\");
            Assert.AreEqual(@"a\b\c", actual);
        }

        [TestMethod]
        public void TokensBefore_Simple_path()
        {
            var data = @"a\b\c\d.txt";
            var actual = data.TokensBefore(4, @"\");
            Assert.AreEqual(@"a\b\c", actual);
        }

        [TestMethod]
        public void AllTokensIgnoreCase_Simple_b()
        {
            var data = "aBcbd";
            var actual = data.AllTokensIgnoreCase("b");
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("a", actual[0]);
            Assert.AreEqual("c", actual[1]);
            Assert.AreEqual("d", actual[2]);
        }

        [TestMethod]
        public void TokenIndexes_Simple_b()
        {
            // start of delimiter at position 1 and 3
            var data = "aBcbd";
            var actual = data.TokenIndexes("b").ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(3, actual[1]);
        }

        [TestMethod]
        public void TokenIndexes_Simple_B()
        {
            // start of delimiter at position 1 and 3
            var data = "aBcbd";
            var actual = data.TokenIndexes("B").ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(3, actual[1]);
        }

        [TestMethod]
        public void TokenIndexes_Simple_NoTokens()
        {
            var data = "aeiou";
            var actual = data.TokenIndexes("b").ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void TokenIndexes_Simple_NoDelimiter()
        {
            var data = "aeiou";
            var actual = data.TokenIndexes("");
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.ToArray().Length);
        }

        [TestMethod]
        public void AllTokensIgnoreCase_Simple_B()
        {
            var data = "aBcbd";
            var actual = data.AllTokensIgnoreCase("B");
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("a", actual[0]);
            Assert.AreEqual("c", actual[1]);
            Assert.AreEqual("d", actual[2]);
        }

        [TestMethod]
        public void TokenIndexes_Simple()
        {
            var data = "a,b,c";
            var actual = data.TokenIndexes(",").ToArray();
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(3, actual[1]);
        }

        [TestMethod]
        public void Carve_Simple_ZeroLengthDelimiter()
        {
            var data = "01234";
            var indexes = new[]
            {
                1, 4
            };
            var actual = data.Carve(indexes, 0);
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("0", actual[0]);
            Assert.AreEqual("123", actual[1]);
            Assert.AreEqual("4", actual[2]);
        }

        [TestMethod]
        public void Carve_Simple_DelimiterLength3()
        {
            var data = "0---123---4";
            var indexes = new[]
            {
                1, 7
            };
            var actual = data.Carve(indexes, 3);
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("0", actual[0]);
            Assert.AreEqual("123", actual[1]);
            Assert.AreEqual("4", actual[2]);
        }

        [TestMethod]
        public void Carve_IndexBeyondLengthOfInput()
        {
            var data = "01234";
            var indexes = new[]
            {
                1, 3, 10
                // 0 for 1 then 1 for 2 then 3 on
                
            };
            var actual = data.Carve(indexes, 0);
            Assert.IsNotNull(actual);
            Assert.AreEqual("0", actual[0]);
            Assert.AreEqual("12", actual[1]);
            Assert.AreEqual("34", actual[2]);
        }

        [TestMethod]
        public void Carve_LastIndexShortOfTheLengthOfInput()
        {
            var data = "0123456789";
            var indexes = new[]
            {
                3
            };
            var actual = data.Carve(indexes, 0);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("012", actual[0]);
            Assert.AreEqual("3456789", actual[1]);
        }

        /*
        [DataTestMethod]
        [DataRow("[", "]", "a[b]c[d]e", new[] { "a", "b", "c", "d", "e"})]
        [DataRow("[","]","a[b]c", new[] { "a", "b", "c" })]
        [DataRow("//~{", "}~//", "abc//~{def}~//ghi", new[] { "abc", "def", "ghi" })]
        public void Tear_Basic(string left, string right, string data, string[] expected)
        {
            var actual = data.Tear(left, right).ToArray();
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsEmpty());
            
            Assert.AreEqual(expected.Length, actual.Length, actual.AsString());

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], actual.AsString());
            }
        }
        */

        [DataTestMethod]
        [DataRow("{", "}", "q{speed 50}{TAB}r", new[] { "q", "speed 50", "TAB", "r"})]
        [DataRow("//~{", "}~//", "abc//~{def}~//ghi", new[] { "abc", "def", "ghi" })]
        [DataRow("[", "]", "a[b]c[d]e", new[] { "a", "b", "c", "d", "e" })]
        [DataRow("[", "]", "a[b]c", new[] { "a", "b", "c" })]
        [DataRow("[", "]", "[", new[] { "" })]
        [DataRow("[", "]", "]", new[] { "" })]
        [DataRow("[", "]", "[]", new[] { "" })]
        [DataRow("[", "]", "h[i]j", new[] { "h", "i", "j" })]
        public void Splice_Basic(string left, string right, string data, string[] expected)
        {
            var actual = data.Splice(left, right).ToArray();
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsEmpty());

            Assert.AreEqual(expected.Length, actual.Length, actual.AsString());

            for (var i = 0; i < expected.Length; i++) Assert.AreEqual(expected[i], actual[i]);
        }

        [DataTestMethod]
        [DataRow("a\nb\nc\n", "~~:a\n~~:b\n~~:c\n~~:\n")]
        [DataRow("123", "~~:123\n")]
        [DataRow(null, "")]
        [DataRow("", "")]
        [DataRow("\nd\ne\nf", "~~:\n~~:d\n~~:e\n~~:f\n")]
        public void QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine_Basic1(string data, string expected)
        {
            var actual = Helpers.QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine(data);
            Assert.AreEqual(
                "\n[" + expected + "]\n",
                "\n[" + actual + "]\n");
        }

        [DataTestMethod]
        [DataRow("--==", "", true)]
        [DataRow("--==", "--==", false)]
        [DataRow("--a==", "~~:a\n", true)]
        [DataRow("--b==", "--~~:b\n==", false)]
        public void UpdateTokensBetween_Basic3(string data, string expected, bool eliminateDelimiters)
        {
            var actual = data.UpdateBetweenTokens("--", "==", eliminateDelimiters,
                Helpers.QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine);
            Assert.AreEqual(
                "\n[" + expected + "]\n",
                "\n[" + actual + "]\n");
        }

        [DataTestMethod]
        [DataRow("//~{}~//", "", true)]
        [DataRow("//~{}~//", "//~{}~//", false)]
        [DataRow("//~{a}~//", "~~:a\n", true)]
        [DataRow("//~{b}~//", "//~{~~:b\n}~//", false)]
        public void UpdateTokensBetween_Basic2(string data, string expected, bool eliminateDelimiters)
        {
            Assert.AreEqual("\n[" + expected + "]\n",
                "\n[" + data.UpdateBetweenTokens("//~{", "}~//", eliminateDelimiters,
                    Helpers.QuickScriptTokenProcessor_AddTildeTildeColonOnEachLine) + "]\n");
        }

        [TestMethod]
        public void TokenAt_Basic()
        {
            var sample1 = _sample + ".";
            Assert.AreEqual("Fred", sample1.TokenAt(1));
            Assert.AreEqual("goes", sample1.TokenAt(2));
            Assert.AreEqual("home.", sample1.TokenAt(3));
            Assert.AreEqual("", sample1.TokenAt(4));
            Assert.AreEqual("", sample1.TokenAt(10));
            Assert.AreEqual("", sample1.TokenAt(-2));

            Assert.AreEqual("", ((string)null).TokenAt(1));
            Assert.AreEqual("", ((string)null).TokenAt(1, null));

            Assert.AreEqual(sample1, sample1.TokenAt(1, ""));
            Assert.AreEqual(sample1, sample1.TokenAt(1, null));

            var sample2 = sample1 + " Fred reads his mail.";
            Assert.AreEqual(_sample, sample2.TokenAt(1, "."));
            Assert.AreEqual(" Fred reads his mail", sample2.TokenAt(2, "."));
            Assert.AreEqual("", sample2.TokenAt(3, "."));
        }

        [TestMethod]
        public void TokenCount_Basic()
        {
            Assert.AreEqual(3, _sample.TokenCount());

            Assert.AreEqual(0, ((string)null).TokenCount());
            Assert.AreEqual(0, ((string)null).TokenCount(null));

            Assert.AreEqual(0, "".TokenCount());
            Assert.AreEqual(0, "".TokenCount(""));
            Assert.AreEqual(0, "".TokenCount("goes"));

            Assert.AreEqual(1, _sample.TokenCount(null));

            Assert.AreEqual(1, _sample.TokenCount(""));
            Assert.AreEqual(1, _sample.TokenCount("boggle"));
            Assert.AreEqual(1, _sample.TokenCount("boggle"));
        }

        [TestMethod]
        public void TokenIndex_Basic()
        {
            Assert.AreEqual(0, _sample.TokenIndex(-1));
            Assert.AreEqual(0, _sample.TokenIndex(0));
            Assert.AreEqual(0, _sample.TokenIndex(1));
            Assert.AreEqual(5, _sample.TokenIndex(2));
            Assert.AreEqual(10, _sample.TokenIndex(3));
            Assert.AreEqual(14, _sample.TokenIndex(4));
            Assert.AreEqual(14, _sample.TokenIndex(400));
        }

        [TestMethod]
        public void TokensBefore_Basic()
        {
            Assert.AreEqual("", _sample.TokensBefore(1));
            Assert.AreEqual("Fred", _sample.TokensBefore(2));
            Assert.AreEqual("Fred goes", _sample.TokensBefore(3));
            Assert.AreEqual("Fred goes home", _sample.TokensBefore(4));
            Assert.AreEqual("Fred goes home", _sample.TokensBefore(10));
            Assert.AreEqual("", _sample.TokensBefore(-2));

            Assert.AreEqual("", ((string)null).TokensBefore(1));
            Assert.AreEqual("", ((string)null).TokensBefore(1, null));

            Assert.AreEqual("", _sample.TokensBefore(1, ""));
            Assert.AreEqual("", _sample.TokensBefore(1, null));
        }
    }
}