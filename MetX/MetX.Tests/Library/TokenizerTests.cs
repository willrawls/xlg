using MetX.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Library
{
    [TestClass]
    public class TokenizerTests
    {
        string _sample = "Fred goes home";

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