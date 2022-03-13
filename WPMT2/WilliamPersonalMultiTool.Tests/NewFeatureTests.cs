
using System.DirectoryServices.ActiveDirectory;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;
using Accessibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamPersonalMultiTool.Acting;
using WilliamPersonalMultiTool.Acting.Actors;
using WilliamPersonalMultiTool.Custom;

namespace WilliamPersonalMultiTool.Tests
{
    [TestClass]
    public class NewFeatureTests
    {
        /*
        [TestMethod]
        public void TextBetweenSlashStarAreComments()
        {
            var actual = Build("A B /* C #1#D type 123");
            My.AssertAllAreEqual(TestPKeys.ABD, actual.Sequence);
        }
        */

        [TestMethod]
        public void KeysNotSeparatedBySpacesGetInterpretedAsIfThereWereSpaces()
        {
            var actual = Build("CapsLock ABD type 123");
            My.AssertAllAreEqual(TestPKeys.CapsABD, actual.Sequence);
        }

        [TestMethod]
        public void ActorsCanHaveVerbs_MoveTo()
        {
            var actual = Build("When CapsLock A move to relative 1 2 3 4");
            Assert.AreEqual("1 2 3 4", actual.Arguments);

            Assert.IsNotNull(actual.Actor);
            Assert.AreEqual(ActionableType.Move, actual.Actor.ActionableType);
            var moveActor = (MoveActor) actual.Actor;

            Assert.IsNotNull(moveActor.ExtractedVerbs);
            Assert.AreEqual(2, moveActor.ExtractedVerbs.Count);
            Assert.AreEqual(ActionableType.Move, moveActor.ActionableType);
            Assert.IsTrue(moveActor.Relative.Mentioned);

            Assert.AreEqual(1, moveActor.Left);
            Assert.AreEqual(2, moveActor.Top);
            Assert.AreEqual(3, moveActor.Width);
            Assert.AreEqual(4, moveActor.Height);
        }

        [TestMethod]
        public void ActionsCanBeTwoWords_MovePercent()
        {
            var actual = Build("A B D move percent relative 10 10 75 50");
            Assert.IsInstanceOfType(actual.Actor, typeof(MoveActor));
            My.AssertAllAreEqual(TestPKeys.ABD, actual.Sequence);
            Assert.AreEqual("10 10 75 50", actual.Arguments);

            Assert.IsNotNull(actual.Actor);
            Assert.AreEqual(ActionableType.Move, actual.Actor.ActionableType);
            var actor = (MoveActor) actual.Actor;

            Assert.AreEqual(2, actor.ExtractedVerbs.Count);
            Assert.AreEqual("percent", actor.ExtractedVerbs[0].Name);

            Assert.AreEqual(10, actor.Left);
            Assert.AreEqual(10, actor.Top);
            Assert.AreEqual(75, actor.Width);
            Assert.AreEqual(50, actor.Height);
        }

        [TestMethod]
        public void VerbHasADefault()
        {
            var actual = Build("CapsLock A size 500 400");
            Assert.AreEqual(ActionableType.Size, actual.Actor.ActionableType);
            var actor = (SizeActor) actual.Actor;
            Assert.AreEqual(actor.To.Name, actor.DefaultVerb.Name);
        }

        [TestMethod]
        public void SizeActor_SizePercent()
        {
            var actual = Build("CapsLock A size percent -10 +10");
            Assert.AreEqual(ActionableType.Size, actual.Actor.ActionableType);
            var actor = (SizeActor) actual.Actor;

            Assert.AreEqual(actor.Percent.Name, actor.ExtractedVerbs[0].Name);
        }

        [TestMethod]
        public void RandomActor_RandomNumber_10To20_Inclusive()
        {
            var actual = Build("CapsLock A random number 10 20");
            Assert.AreEqual(ActionableType.Random, actual.Actor.ActionableType);
            var actor = (RandomActor) actual.Actor;
            Assert.IsTrue(actor.Has(actor.Number));
        }

        [TestMethod]
        public void RandomActor_TenRandomLetters()
        {
            var actual = Build("CapsLock A random letters 10");
            Assert.AreEqual(ActionableType.Random, actual.Actor.ActionableType);
            var actor = (RandomActor) actual.Actor;
            Assert.IsTrue(actor.Has(actor.Letters));
            Assert.IsTrue(actor.Letters.Mentioned);
            Assert.IsFalse(actor.Digits.Mentioned);
            Assert.AreEqual(10, actor.Count);
        }

        [TestMethod]
        public void TypeActor_Slowest()
        {
            var actual = Build("CapsLock A type slowest fred");
            Assert.AreEqual(ActionableType.Type, actual.Actor.ActionableType);
            var actor = (TypeActor) actual.Actor;
            Assert.AreEqual(50, actor.DelayInMilliseconds);
        }

        /*
        [TestMethod]
        public void RepeatActor_RepeatLast()
        {
            var actual = Build("CapsLock A repeat");
            Assert.AreEqual(ActionableType.Repeat, actual.Actor.ActionableType);
            var actor = (RepeatActor) actual.Actor;
            Assert.AreEqual("last", actor.RepeatLastCount);
            Assert.AreEqual(1, actor.RepeatLastCount);
        }
        */

        /*
        [TestMethod]
        public void RepeatActor_RepeatLast4()
        {
            var actual = Build("CapsLock A repeat last 4");
            Assert.AreEqual(ActionableType.Repeat, actual.Actor.ActionableType);
            var actor = (RepeatActor) actual.Actor;
            Assert.IsTrue(actor.Has(actor.Last));
            Assert.AreEqual(4, actor.RepeatLastCount);
        }

        [TestMethod]
        public void TypeActor_TypeTheContentsOfTheClipboard()
        {
            var actual = Build("CapsLock A type clipboard");
            Assert.AreEqual(ActionableType.Type, actual.Actor.ActionableType);
            var actor = (TypeActor) actual.Actor;
            Clipboard.SetText("123");
            Assert.AreEqual("123", actor.ClipboardText());
        }
        */

        private static CustomKeySequence Build(string input)
        {
            var manager = new CustomPhraseManager(null, input);
            var actual = (CustomKeySequence) manager.Keyboard.KeySequences[0];
            return actual;
        }
    }
}

