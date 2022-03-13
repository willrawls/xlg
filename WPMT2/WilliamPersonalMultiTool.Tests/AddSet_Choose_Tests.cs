using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class AddSet_Choose_Tests
    {
        private const string SetText = @"
When CapsLock 1 2 3 type someone.at@hotmail.com
When CapsLock 1 2 4 choose
    abc
    def
    ghi
When CapsLock 1 2 P choose
jkl
  mno
When CapsLock 1 2 5 type someone.at@hotmail.com
";

        [TestMethod]
        public void AddSet_SetText_Basic()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(SetText);
            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        public void AddSet_Choose_124_3Choices()
        {
            var data = new CustomPhraseManager(null);
            var set = data.AddSet(SetText);
            var baseActor = (CustomKeySequence) set[1];
            var actual = (ChooseActor)baseActor.Actor;

            Assert.IsNotNull(actual.Choices);
            Assert.AreEqual(3, actual.Choices.Count);
            Assert.AreEqual("abc", actual.Choices[0]);
            Assert.AreEqual("def", actual.Choices[1]);
            Assert.AreEqual("ghi", actual.Choices[2]);
        }

        [TestMethod]
        public void AddSet_Choose_12P_2Choices()
        {
            var data = new CustomPhraseManager(null);
            var set = data.AddSet(SetText);
            var baseActor = (CustomKeySequence) set[2];
            var actual = (ChooseActor)baseActor.Actor;

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Choices.Count);
            Assert.AreEqual("jkl", actual.Choices[0]);
            Assert.AreEqual("mno", actual.Choices[1]);
        }
    }
}