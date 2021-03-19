using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class Project_Target_AddSource
    {
        public const string EmptyClientXmlFilePath = @"Standard\ProjectPieces\EmptyClient.xml";
        public const string FullClientXmlFilePath = @"Standard\ProjectPieces\FullClient.xml";
        
        [TestMethod]
        public void Target_FromScratch_RemovesWhenPresent()
        {
            var modifier = GetFullClient();
            modifier.Targets.Setup();
            modifier.Targets.Remove(); <<< Start here. ItemGroups added twice for some reason
            
            Assert.IsNull(modifier.Targets.AddSourceGeneratedFiles?.TargetElement);
            Assert.IsNull(modifier.Targets.AddSourceGeneratedFiles);
            
            Assert.IsNull(modifier.Targets.RemoveSourceGeneratedFiles?.TargetElement);
            Assert.IsNull(modifier.Targets.RemoveSourceGeneratedFiles);
            
            var innerXml = modifier.Document.InnerXml;
            Assert.IsFalse(innerXml.Contains(@"Generated\**"), innerXml);
            Assert.IsFalse(innerXml.Contains(@"AddSourceGeneratedFiles"), innerXml);
            Assert.IsFalse(innerXml.Contains(@"RemoveSourceGeneratedFiles"), innerXml);
        }

        [TestMethod]
        public void Target_FromScratch_GeneratesCorrectInnerXml()
        {
            var modifier = GetEmptyClient();
            modifier.Targets.Setup();
            Assert.IsNotNull(modifier.Targets.AddSourceGeneratedFiles);
            Assert.IsNotNull(modifier.Targets.AddSourceGeneratedFiles.TargetElement);
            Assert.AreEqual("AddSourceGeneratedFiles", modifier.Targets.AddSourceGeneratedFiles.Name);
            Assert.AreEqual("CoreCompile", modifier.Targets.AddSourceGeneratedFiles.AfterTargets);
            Assert.IsNotNull(modifier.Targets.AddSourceGeneratedFiles.ItemGroup);
            
            var itemGroupCompile = (XmlElement) modifier.Targets.AddSourceGeneratedFiles.ItemGroup.SelectSingleNode("Compile");
            Assert.IsNotNull(itemGroupCompile);
            Assert.AreEqual(@"Generated\**", itemGroupCompile.GetAttribute("Include"));

            
            Assert.AreEqual("CoreCompile", modifier.Targets.AddSourceGeneratedFiles.AfterTargets);
            Assert.AreEqual(null, modifier.Targets.AddSourceGeneratedFiles.BeforeTargets);
        }

        public static Modifier GetFullClient()
        {
            var filePath = FullClientXmlFilePath;
            Assert.IsTrue(File.Exists(filePath));
            return Modifier.LoadFile(filePath);
        }

        public static Modifier GetEmptyClient()
        {
            var filePath = EmptyClientXmlFilePath;
            Assert.IsTrue(File.Exists(filePath));
            return Modifier.LoadFile(filePath);
        }

    }
}