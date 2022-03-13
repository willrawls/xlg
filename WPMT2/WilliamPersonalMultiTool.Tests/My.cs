using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace WilliamPersonalMultiTool.Tests
{
    public static class My
    {
        public static void AssertAllAreEqual(List<InlinePiece> expected, List<InlinePiece> actual)
        {
            Assert.IsNotNull(actual);
            
            var debug = "\nExpected: ";
            foreach (InlinePiece item in expected)
                debug += item.Contents + ",";

            debug += "\nActual:   ";
            foreach (InlinePiece item in actual)
                debug += item.Contents + ",";

            Assert.AreEqual(expected.Count, actual.Count, debug);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(actual[i].Contents, expected[i].Contents, $"{debug}: Index {i}");
                Assert.AreEqual(actual[i].Command, expected[i].Command, $"{debug}: Index {i}");
                Assert.AreEqual(actual[i].Arguments, expected[i].Arguments, $"{debug}: Index {i}");
            }
        }

        public static void AssertAllAreEqual(List<PKey> expected, List<PKey> actual)
        {
            Assert.IsNotNull(actual);
            
            var debug = "\nExpected: ";
            foreach (var key in expected)
                debug += key.ToString() + " ";

            debug += "\nActual:   ";
            foreach (var key in actual)
                debug += key.ToString() + " ";

            Assert.AreEqual(expected.Count, actual.Count, debug);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(actual[i], expected[i], $"{debug}: Index {i}");
            }
        }
    }
}