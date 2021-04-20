using System.IO;
using System.Xml;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod]
        public void Target_FromScratch_RemovesWhenPresent()
        {
            var modifier = Piece.GetFullClient();
            modifier.Targets.Insert();
            modifier.Targets.Remove(); 
            
            Assert.IsNull(modifier.Targets.AddSourceGeneratedFiles);
            Assert.IsNull(modifier.Targets.RemoveSourceGeneratedFiles);
            
            var innerXml = modifier.Document.InnerXml;
            var innerXmlFormatted = innerXml.Replace("><", ">\n\t<");
            Assert.IsFalse(innerXml.Contains(@"Generated\**"), innerXmlFormatted);
            Assert.IsFalse(innerXml.Contains(@"AddSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsFalse(innerXml.Contains(@"RemoveSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsNotNull(modifier.Document.SelectSingleNode(XPaths.TargetByName("SomePreExistingTarget1")));
            Assert.IsNotNull(modifier.Document.SelectSingleNode(XPaths.TargetByName("SomePreExistingTarget2")));
            Assert.IsNotNull(modifier.Document.SelectSingleNode(XPaths.TargetByName("SomePreExistingTarget3")));
        }
        
        [TestMethod]
        public void Target_FromScratch_RemovesWhenNotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            modifier.Targets.Remove(); 
            
            Assert.IsNull(modifier.Targets.AddSourceGeneratedFiles);
            Assert.IsNull(modifier.Targets.RemoveSourceGeneratedFiles);
            
            var innerXml = modifier.Document.InnerXml;
            var innerXmlFormatted = innerXml.Replace("><", ">\n\t<");
            Assert.IsFalse(innerXml.Contains(@"Generated\**"), innerXmlFormatted);
            Assert.IsFalse(innerXml.Contains(@"AddSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsFalse(innerXml.Contains(@"RemoveSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsNotNull(modifier.Document.SelectSingleNode(XPaths.TargetByName("SomePreExistingTarget1")));
        }
        
        [TestMethod]
        public void Target_FromScratch_InsertsWhenNotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            modifier.Targets.Insert(); 
            
            Assert.IsNotNull(modifier.Targets);
            Assert.IsNotNull(modifier.Targets.AddSourceGeneratedFiles);
            Assert.IsNotNull(modifier.Targets.RemoveSourceGeneratedFiles);
            
            var innerXml = modifier.Document.InnerXml;
            var innerXmlFormatted = innerXml.Replace("><", ">\n\t<");
            Assert.IsTrue(innerXml.Contains(@"Generated\**"), innerXmlFormatted);
            Assert.IsTrue(innerXml.Contains(@"AddSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsTrue(innerXml.Contains(@"RemoveSourceGeneratedFiles"), innerXmlFormatted);
            Assert.IsNotNull(modifier.Document.SelectSingleNode(XPaths.TargetByName("SomePreExistingTarget1")));
        }

        [TestMethod]
        public void Target_FromScratch_GeneratesCorrectInnerXml()
        {
            var modifier = Piece.GetEmptyClient();
            modifier.Targets.Insert();
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
    }
}