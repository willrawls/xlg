using System;
using System.IO;
using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Scripts
{
    [TestClass]
    public class XlgQuickScriptTemplateTests
    {
        [TestMethod]
        public void ResolveExeTemplate()
        {
            var data = new XlgQuickScriptTemplate(@"Templates\Exe", "Fred");

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Assets);
            Assert.AreEqual(4, data.Assets.Count);
            Assert.AreEqual("Fred", data.Name);

            Assert.AreEqual(4, data.Assets.Items.Length);
            CollectionAssert.AreEqual(new [] { "Program.cs", "QuickScriptProcessor.cs", "_.csproj", "_.sln" }, data.Assets.Keys);

            var settings = new ActualizationSettings(data, Guid.NewGuid().ToString("N"), true);
            settings.Answers["DestinationFilePath"].Value = "AAA";
            settings.Answers["InputFilePath"].Value = "BBB";
            settings.Answers["NameInstance"].Value = "CCC";
            settings.Answers["Usings"].Value = "using DDD.D;\n";

            var actual = data.Actualize(settings);

            Assert.IsNotNull(actual);
            Assert.IsNull(actual.ErrorText);
            Assert.AreEqual(4, actual.OutputFiles.Count);
            Assert.IsFalse(actual.OutputFiles["_.csproj"].Name?.Contains("_"));
            Console.WriteLine(actual.Settings.OutputFolder);
        }
    }
}