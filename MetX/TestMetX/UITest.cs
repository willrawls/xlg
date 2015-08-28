using MetX.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;

namespace MetX.Tests
{
    [TestClass]
    public class UITest
    {
        [TestMethod]
        public void InputBoxTest()
        {
            string title = string.Empty; 
            string promptText = string.Empty; 
            string value = string.Empty; 
            string valueExpected = string.Empty; 
            DialogResult expected = new DialogResult(); 
            var actual = UI.InputBox(title, promptText, ref value);
            Assert.AreEqual(valueExpected, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
