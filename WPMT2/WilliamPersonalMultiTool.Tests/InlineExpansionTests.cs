using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class InlineExpansionTests
    {
        [TestMethod]
        public void Pause5Milliseconds()
        {
            var manager = new CustomPhraseManager(null);
            var actual = new InlineExpansion(manager, "ab{pause 5}cd");

            Assert.AreEqual(3, actual.Count);
            
            Assert.AreEqual("ab", actual[0].Contents);
            Assert.IsNull(actual[0].Command);
            Assert.IsNull(actual[0].Arguments);

            Assert.AreEqual("pause 5", actual[1].Contents);
            Assert.AreEqual("pause", actual[1].Command);
            Assert.AreEqual("5", actual[1].Arguments);

            Assert.AreEqual("cd", actual[2].Contents);
            Assert.IsNull(actual[2].Command);
            Assert.IsNull(actual[2].Arguments);
        }

        [TestMethod]
        public void Backspace()
        {
            var manager = new CustomPhraseManager(null);
            var actual = new InlineExpansion(manager, "ab{BACKSPACE}cd");

            Assert.AreEqual(3, actual.Count);

            var piece = actual[1];
            Assert.AreEqual("BACKSPACE", piece.Contents);
            Assert.AreEqual("pkey", piece.Command);
            Assert.IsNull(piece.Arguments);
        }

        [TestMethod]
        public void TypeClipboard()
        {
            var manager = new CustomPhraseManager(null);
            var actual = new InlineExpansion(manager, "ab{Clipboard}cd");

            Assert.AreEqual(3, actual.Count);

            var piece = actual[1];
            Assert.AreEqual("clipboard", piece.Command);
            Assert.AreEqual("", piece.Arguments);
        }
    }
}