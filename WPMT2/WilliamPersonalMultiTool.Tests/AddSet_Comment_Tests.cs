using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class AddSet_CommentTests
    {
        private const string SetText = @"
When CapsLock 1 2 3 type someone.at@a.com
//When CapsLock 1 2 3 type someone.at@b.com
Or 5 type someone.at@c.com
";

        [TestMethod]
        public void AddSet_SetText_Basic()
        {
            var data = new CustomPhraseManager(null);
            var actual = data.AddSet(SetText);
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void AddSet_Comment_124_3Choices()
        {
            var data = new CustomPhraseManager(null);
            var set = data.AddSet(SetText);
            My.AssertAllAreEqual(TestPKeys.Caps123, set[0].Sequence);
            My.AssertAllAreEqual(TestPKeys.Caps125, set[1].Sequence);
        }
    }
}