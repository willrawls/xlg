using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamPersonalMultiTool.Acting;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests.Acting.Actors
{
    [TestClass]
    public class RandomActorTests
    {
        [TestMethod]
        public void Roll1d20_Simple()
        {
            var randomActor = SetupRandomActor("dice 1 20");
            Assert.IsTrue(randomActor.Dice.Mentioned);
            Assert.IsFalse(randomActor.Digits.Mentioned);

            Assert.AreEqual(1, randomActor.Count);
            Assert.AreEqual(20, randomActor.Sides);
        }

        [TestMethod]
        public void NineDigits_Simple()
        {
            var randomActor = SetupRandomActor("digits 9");
            Assert.IsTrue(randomActor.Digits.Mentioned);
            Assert.IsFalse(randomActor.Dice.Mentioned);

            Assert.AreEqual(9, randomActor.Count);
            Assert.AreEqual(-1, randomActor.Sides);
        }

        [TestMethod]
        public void FiveLetters_Simple()
        {
            var randomActor = SetupRandomActor("letters 5");
            Assert.IsTrue(randomActor.Letters.Mentioned);

            Assert.AreEqual(5, randomActor.Count);
            Assert.AreEqual(-1, randomActor.Sides);
        }

        [TestMethod]
        public void NumberBeforeAndAfter_Simple()
        {
            var randomActor = SetupRandomActor("5 49 \"before\" \"after\" this should be ignored");
            Assert.IsTrue(randomActor.Number.Mentioned);

            Assert.AreEqual("before", randomActor.Before);
            Assert.AreEqual("after", randomActor.After);
            Assert.AreEqual(5, randomActor.Count);
            Assert.AreEqual(49, randomActor.Sides);
            Assert.IsFalse(randomActor.Arguments.Contains("ignored"));
        }

        private static RandomActor SetupRandomActor(string text)
        {
            var customPhraseManager = new CustomPhraseManager(null);
            BaseActor actual = ActorHelper.Factory($"CapsLock 123 random {text}", customPhraseManager);

            Assert.IsNotNull(actual);
            RandomActor randomActor = (RandomActor)actual;

            Assert.AreEqual(ActionableType.Random, actual.ActionableType);

            My.AssertAllAreEqual(TestPKeys.Caps123, actual.KeySequence.Sequence);

            Assert.IsNotNull(actual.LegalVerbs);
            Assert.AreEqual(4, actual.LegalVerbs.Count);

            Assert.IsNotNull(actual.ExtractedVerbs);
            return randomActor;
        }
    }
}