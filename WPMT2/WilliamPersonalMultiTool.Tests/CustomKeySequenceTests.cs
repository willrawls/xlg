using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class CustomKeySequenceTests
    {
        [TestMethod]
        public void AddSet_WhenFollowedByOrEndRightBeforeNewLine()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(@"
When CapsLock G G type william.rawls@gmail.com
  Or W type william.rawls@otherplace.com
");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("william.rawls@gmail.com", actual[0].Name);
        }

        [TestMethod]
        public void AddSet_WhenFollowedByAnotherWhenEndsBeforeTheNextWhen()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(@"
When CapsLock G 1 type william.rawls@gmail.com
123
When CapsLock G 2 type
456
789!
When CapsLock G 3 type william.rawls@otherplace.com
");
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("william.rawls@gmail.com{ENTER}123", actual[0].Name);
            Assert.AreEqual("456{ENTER}789!", actual[1].Name);
            Assert.AreEqual("william.rawls@otherplace.com", actual[2].Name);
        }

        [TestMethod]
        public void AddSet_WhenKeySequenceAlreadyExists_Replace()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(@"
When CapsLock 1 2 3 type someone.at@gmail.com
When CapsLock 1 2 3 type someone.at@hotmail.com
");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("someone.at@hotmail.com", actual[0].Name);
            My.AssertAllAreEqual(TestPKeys.Caps123, actual[0].Sequence);
        }

        [TestMethod]
        public void ReplaceMatching_WhenKeySequenceAlreadyExists_Replace()
        {
            List<KeySequence> keySequences = new List<KeySequence>()
            {
                new CustomKeySequence("Fred", TestPKeys.Caps123, null),
            };
            var newSequence = new CustomKeySequence("George", TestPKeys.Caps123, null);

            // Act
            Extensions.ReplaceMatching(keySequences, newSequence);

            Assert.AreEqual(1, keySequences.Count);
            Assert.AreEqual("George", keySequences[0].Name);
        }

        [TestMethod]
        public void AddSet_Caps123_OnOneLine()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When CapsLock 1 2 3 type someone.at@gmail.com");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("someone.at@gmail.com", actual[0].Name);
            My.AssertAllAreEqual(TestPKeys.Caps123, actual[0].Sequence);
        }

        [TestMethod]
        public void AddSet_Caps123_NoSpaces_OnOneLine()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When CapsLock 123 type someone.at@gmail.com");
            Assert.AreEqual(1, actual.Count);
            My.AssertAllAreEqual(TestPKeys.Caps123, actual[0].Sequence);
        }

        [TestMethod]
        public void AddSet_Caps123_RunNotepad_Simple()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When CapsLock 1 2 3 run notepad.exe");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Run notepad.exe", actual[0].Name);
            Assert.AreEqual(1, data.Keyboard.KeySequences.Count);
            Assert.AreEqual("Run notepad.exe", data.Keyboard.KeySequences[0].Name);
        }

        [TestMethod]
        public void AddSet_Caps123_RunNotepad_FullPathWithArguments()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(@"

When CapsLock Shift W type William\tRawls
  Or 1 run ""notepad.exe"" arguments.txt ""Mike Fred George Mary""

");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("\nRun \"notepad.exe\" arguments.txt \"Mike Fred George Mary\"", "\n" + actual[1].Name);
            Assert.AreEqual(2, data.Keyboard.KeySequences.Count);
            var sequence = (CustomKeySequence) data.Keyboard.KeySequences[1];
            
            Assert.AreEqual(@"notepad.exe", sequence.ExecutablePath);
            Assert.AreEqual("\n" + @"""arguments.txt"" ""Mike Fred George Mary""", "\n" + sequence.Arguments);
        }

        [TestMethod]
        public void AddSet_ShiftX2WildDigits_OnOneLine()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When Shift X ## type x~~~~y");
            Assert.AreEqual("x~~~~y", actual[0].Name);
            My.AssertAllAreEqual(TestPKeys.ShiftX, actual[0].Sequence);
            Assert.AreEqual(2, actual[0].WildcardCount);
            Assert.AreEqual(WildcardMatchType.Digits, actual[0].WildcardMatchType);
        }

        [TestMethod]
        public void AddSet_ShiftX2WildAlphaNumeric_OnOneLine()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When Shift X ** type x~~~~y");
            Assert.AreEqual("x~~~~y", actual[0].Name);
            My.AssertAllAreEqual(TestPKeys.ShiftX, actual[0].Sequence);

            Assert.AreEqual(2, actual[0].WildcardCount);
            Assert.AreEqual(WildcardMatchType.AlphaNumeric, actual[0].WildcardMatchType);
        }

        [TestMethod]
        public void AddSet_BackspaceCountSetTo2()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet("When Shift X Y type fred");
            My.AssertAllAreEqual(TestPKeys.ShiftXY, actual[0].Sequence);

            Assert.AreEqual(2, ((CustomKeySequence)actual[0]).BackspaceCount);
        }

        [TestMethod]
        public void AddOrReplace_SingleLine_Caps123()
        {
            var data = new CustomPhraseManager(null);
            CustomKeySequence actual = data.AddOrReplace("CapsLock 1 2 3");
            My.AssertAllAreEqual(TestPKeys.Caps123, actual.Sequence);
            Assert.AreEqual(1, data.Keyboard.KeySequences.Count);
        }

        [TestMethod]
        public void AddSet_Caps123and4_OnTwoLines()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(@"
When CapsLock 1 2 3 type someone.at@gmail.com
Or 4 type someone.at@hotmail.com
");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("someone.at@gmail.com", actual[0].Name);
            My.AssertAllAreEqual(TestPKeys.Caps123, actual[0].Sequence);
            Assert.AreEqual("someone.at@hotmail.com", actual[1].Name);
            My.AssertAllAreEqual(TestPKeys.Caps124, actual[1].Sequence);
        }

        [TestMethod]
        public void ToPKeyList_Caps123()
        {
            var actual = Extensions.ToPKeyList("CapsLock 1 2 3", null, out var wildcardMatchType, out var wildcardCount);
            My.AssertAllAreEqual(TestPKeys.Caps123, actual);
            Assert.AreEqual(0, wildcardCount);
        }

        [TestMethod]
        public void ToPKeyList_1A3_WithPrependCaps()
        {
            var actual = Extensions.ToPKeyList("1 a 3", new List<PKey> { PKey.CapsLock }, out var wildcardMatchType, out var wildcardCount);
            My.AssertAllAreEqual(TestPKeys.Caps1A3, actual);
        }
    }
}
