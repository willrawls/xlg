using System;
using System.IO;
using MetX.Standard.Library;
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

            XlgQuickScript source = new XlgQuickScript("Freddy", "a = b;");
            var settings = new ActualizationSettings(data, true, source);

            settings.Answers["DestinationFilePath"].Value = "AAA";
            settings.Answers["InputFilePath"].Value = "BBB";
            settings.Answers["NameInstance"].Value = "CCC";
            settings.Answers["Usings"].Value = "using DDD.D;\n";

            settings.Answers["ClassMembers"].Value = "string fred;";
            settings.Answers["Finish"].Value = "fred = \"finish\";";
            settings.Answers["ProcessLine"].Value = "fred = line;";
            settings.Answers["ReadInput"].Value = "return true;";
            settings.Answers["Start"].Value = "~~:Starting";
            settings.Answers["Project Name"].Value = settings.ProjectName;
            settings.Answers["UserName"].Value = Environment.UserName.LastToken(@"\");
            settings.Answers["Guid Config"].Value = Guid.NewGuid().ToString("N");
            settings.Answers["Guid Project 1"].Value = Guid.NewGuid().ToString("N");
            settings.Answers["Guid Project 2"].Value = Guid.NewGuid().ToString("N");
            settings.Answers["Guid Solution"].Value = Guid.NewGuid().ToString("N");

            ActualizationResult actual = data.ActualizeCode(settings);

            Assert.IsNotNull(actual);
            Assert.IsNull(actual.ActualizeErrorText);
            Assert.AreEqual(4, actual.OutputFiles.Count);
            Assert.IsFalse(actual.OutputFiles["_.csproj"].Name?.Contains("_"));

            if (actual.Warnings.Count == 0)
                return;

            Console.WriteLine(actual.Settings.OutputFolder);
            Console.WriteLine();
            Console.WriteLine("----- Warnings ------");
            Console.WriteLine();
            foreach(var warning in actual.Warnings)
                Console.WriteLine($"{warning}");
            Console.WriteLine();

            bool compileResult = actual.Compile();
            Assert.IsNotNull(actual.CompiledAssemblyFilePath);
            Assert.IsTrue(File.Exists(actual.CompiledAssemblyFilePath));
            Console.WriteLine($"\nExecutableFilePath is:\n{actual.CompiledAssemblyFilePath}");
        }
    }
}