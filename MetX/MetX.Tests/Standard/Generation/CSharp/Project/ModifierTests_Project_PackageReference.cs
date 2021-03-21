using System.IO;
using System.Xml;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    public class ModifierTests_PackageReference
    {
        [TestMethod]
        public void InsertOne_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            var packageName = "Microsoft.CodeAnalysis.Common";
            var version = "3.9.0";
            var packageReference = modifier.ItemGroup.PackageReference.InsertOrUpdate(packageName, version);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.IsNotNull(version, packageReference.Attributes["Version"]?.Value);
        }
        
        [TestMethod]
        public void InsertOneWithAssets_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            var packageName = "Microsoft.CodeAnalysis.Analyzers";
            var version = "3.3.2";
            var privateAssets = "all";
            var includeAssets = "runtime; build; native; contentfiles; analyzers; buildtransitive";
            var packageReference = modifier.ItemGroup.PackageReference.InsertOrUpdate(packageName, version, privateAssets, includeAssets);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.AreEqual(version, packageReference.Attributes["Version"]?.Value);
            
            Assert.AreEqual(2, packageReference.ChildNodes.Count);
            Assert.AreEqual("PrivateAssets", packageReference.ChildNodes[0]?.Name);
            Assert.AreEqual(privateAssets, packageReference.ChildNodes[0]?.InnerText);
            
            Assert.AreEqual("IncludeAssets", packageReference.ChildNodes[1]?.Name);
            Assert.AreEqual(includeAssets, packageReference.ChildNodes[1]?.InnerText);
        }
        
        [TestMethod]
        public void InsertOne_Present()
        {
            var modifier = Piece.GetFullClient();
            var packageName = "Microsoft.CodeAnalysis.Common";
            var version = "3.9.0";
            var packageReference = modifier.ItemGroup.PackageReference.InsertOrUpdate(packageName, version);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.IsNotNull(version, packageReference.Attributes["Version"]?.Value);
        }
        
        [TestMethod]
        public void InsertOneWithAssets_Present()
        {
            var modifier = Piece.GetFullClient();
            var packageName = "Microsoft.CodeAnalysis.Analyzers";
            var version = "3.3.2";
            var privateAssets = "all";
            var includeAssets = "runtime; build; native; contentfiles; analyzers; buildtransitive";
            var packageReference = modifier.ItemGroup.PackageReference.InsertOrUpdate(packageName, version, privateAssets, includeAssets);
            
            Assert.IsNotNull(packageReference);
            Assert.AreEqual(packageName, packageReference.Attributes["Include"]?.Value);
            Assert.AreEqual(version, packageReference.Attributes["Version"]?.Value);
            
            Assert.AreEqual(2, packageReference.ChildNodes.Count);
            Assert.AreEqual("PrivateAssets", packageReference.ChildNodes[0]?.Name);
            Assert.AreEqual(privateAssets, packageReference.ChildNodes[0]?.InnerText);
            
            Assert.AreEqual("IncludeAssets", packageReference.ChildNodes[1]?.Name);
            Assert.AreEqual(includeAssets, packageReference.ChildNodes[1]?.InnerText);
        }
        
    }
}