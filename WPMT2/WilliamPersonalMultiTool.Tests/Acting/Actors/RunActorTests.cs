using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using WilliamPersonalMultiTool.Acting;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class RunActorTests
    {
        [TestMethod]
        public void RunActor_Simple()
        {
            var expected = TestPKeys.Caps123;

            // Act
            var customPhraseManager = new CustomPhraseManager(null);
            BaseActor actual = ActorHelper.Factory("CapsLock 123 run maximize notepad", customPhraseManager);

            Assert.IsNotNull(actual);
            RunActor runActor = (RunActor) actual;

            Assert.AreEqual(ActionableType.Run, actual.ActionableType);
            
            My.AssertAllAreEqual(expected, actual.KeySequence.Sequence);

            Assert.IsNotNull(actual.LegalVerbs);
            Assert.AreEqual(3, actual.LegalVerbs.Count);

            Assert.IsNotNull(actual.ExtractedVerbs);
            Assert.AreEqual(1, actual.ExtractedVerbs.Count);
            Assert.AreEqual("maximize", actual.ExtractedVerbs[0].Name);
            Assert.IsTrue(actual.ExtractedVerbs[0].Mentioned);
            Assert.IsTrue(runActor.Maximize.Mentioned);

            Assert.IsFalse(actual.Has(runActor.Minimize));
            Assert.IsTrue(actual.Has(runActor.Maximize));
            Assert.IsFalse(actual.Has(runActor.Normal));

            Assert.AreEqual("notepad", actual.Arguments);
        }

    }
}