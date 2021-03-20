using System.Formats.Asn1;
using System.IO;
using System.Xml;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    public class Project_PackageReference
    {
        public const string EmptyClientXmlFilePath = @"Standard\ProjectPieces\EmptyClient.xml";
        public const string FullClientXmlFilePath = @"Standard\ProjectPieces\FullClient.xml";
        
        [TestMethod]
        public void InsertOne_NotPresent()
        {
            var modifier = GetEmptyClient();
            var packageName = "Microsoft.CodeAnalysis.Common";
            var version = "3.9.0";
            var packageReference = modifier.ItemGroup.PackageReference.GetOrInsert(packageName, version);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.IsNotNull(version, packageReference.Attributes["Version"]?.Value);
        }
        
        [TestMethod]
        public void InsertOne_NotPresentWithAssets()
        {
            var modifier = GetEmptyClient();
            var packageName = "Microsoft.CodeAnalysis.Analyzers";
            var version = "3.3.2";
            var privateAssets = "all";
            var includeAssets = "runtime; build; native; contentfiles; analyzers; buildtransitive";
            var packageReference = modifier.ItemGroup.PackageReference.GetOrInsert(packageName, version, privateAssets, includeAssets);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.AreEqual(version, packageReference.Attributes["Version"]?.Value);
            
            Assert.AreEqual(2, packageReference.ChildNodes.Count);
            Assert.AreEqual("PrivateAssets", packageReference.ChildNodes[0]?.Name);
            Assert.AreEqual(privateAssets, packageReference.ChildNodes[0]?.Value);
            
            Assert.AreEqual("IncludeAssets", packageReference.ChildNodes[1]?.Name);
            Assert.AreEqual(includeAssets, packageReference.ChildNodes[1]?.Value);
        }
        
        [TestMethod]
        public void InsertOne_Present()
        {
            var modifier = GetFullClient();
            var packageName = "Microsoft.CodeAnalysis.Common";
            var version = "3.9.0";
            var packageReference = modifier.ItemGroup.PackageReference.GetOrInsert(packageName, version);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.IsNotNull(version, packageReference.Attributes["Version"]?.Value);
        }
        
        [TestMethod]
        public void Target_FromScratch_RemovesWhenNotPresent()
        {
            var modifier = GetEmptyClient();
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
            var modifier = GetEmptyClient();
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
            var modifier = GetEmptyClient();
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