using System.Xml;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod]
        public void PackageReference_InsertOne_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            var element = modifier.ItemGroup.Common.InsertOrUpdate();
            
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.Common.PackageName, element.Attributes["Include"]?.Value);
            Assert.IsNotNull(modifier.ItemGroup.Common.Version, element.Attributes["Version"]?.Value);
        }
        
        [TestMethod]
        public void PackageReference_InsertOneWithAssets_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            var element = modifier.ItemGroup.Analyzers.InsertOrUpdate();
            
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.Analyzers.PackageName, element.Attributes["Include"]?.Value);
            Assert.AreEqual(modifier.ItemGroup.Analyzers.Version, element.Attributes["Version"]?.Value);
            
            Assert.AreEqual(2, element.ChildNodes.Count);
            Assert.AreEqual("PrivateAssets", element.ChildNodes[0]?.Name);
            Assert.AreEqual(modifier.ItemGroup.Analyzers.PrivateAssets, element.ChildNodes[0]?.InnerText);
            
            Assert.AreEqual("IncludeAssets", element.ChildNodes[1]?.Name);
            Assert.AreEqual(modifier.ItemGroup.Analyzers.IncludeAssets, element.ChildNodes[1]?.InnerText);
        }
        
        [TestMethod]
        public void PackageReference_InsertOne_Present()
        {
            var modifier = Piece.GetFullClient();
            var element = modifier.ItemGroup.Common.InsertOrUpdate();
            AssertPackageReference(element, modifier.ItemGroup.Common);
        }
        
        [TestMethod]
        public void PackageReference_InsertOneWithAssets_Present()
        {
            var modifier = Piece.GetFullClient();
            var element = modifier.ItemGroup.Analyzers.InsertOrUpdate();
            AssertPackageReference(element, modifier.ItemGroup.Analyzers);
        }

        [TestMethod]
        public void InsertOrUpdateAllForGeneration_Simple()
        {
            var modifier = Piece.GetFullClient();
            var element = modifier.ItemGroup.Analyzers.InsertOrUpdate();
            AssertPackageReference(element, modifier.ItemGroup.Analyzers);
        }

        [TestMethod]
        public void InsertOrUpdateAll_Simple()
        {
            var modifier = Piece.GetEmptyClient();
            
            var analyzersPR = modifier.ItemGroup.Analyzers.InsertOrUpdate();
            var commonPR = modifier.ItemGroup.Common.InsertOrUpdate();
            var workspacesPR = modifier.ItemGroup.CSharpWorkspaces.InsertOrUpdate();

            AssertPackageReference(analyzersPR, modifier.ItemGroup.Analyzers);
            AssertPackageReference(commonPR, modifier.ItemGroup.Common);
            AssertPackageReference(workspacesPR, modifier.ItemGroup.CSharpWorkspaces);
        }

        [TestMethod]
        public void RemoveAll_AlreadyThere()
        {
            var modifier = Piece.GetFullClient();
            
            var analyzersPR = modifier.ItemGroup.Analyzers.Remove();
            var commonPR = modifier.ItemGroup.Common.Remove();
            var workspacesPR = modifier.ItemGroup.CSharpWorkspaces.Remove();
            Assert.IsNull(analyzersPR);
            Assert.IsNull(commonPR);
            Assert.IsNull(workspacesPR);
        }

        [TestMethod]
        public void RemoveAll_NothingThere()
        {
            var modifier = Piece.GetEmptyClient();
            
            var analyzersPR = modifier.ItemGroup.Analyzers.Remove();
            var commonPR = modifier.ItemGroup.Common.Remove();
            var workspacesPR = modifier.ItemGroup.CSharpWorkspaces.Remove();

            Assert.IsNull(analyzersPR);
            Assert.IsNull(commonPR);
            Assert.IsNull(workspacesPR);
        }

        
        public static void AssertPackageReference(XmlElement element, PackageReference reference)
        {
            Assert.IsNotNull(element);
            Assert.AreEqual(reference.PackageName, element.Attributes["Include"]?.Value);
            Assert.AreEqual(reference.Version, element.Attributes["Version"]?.Value);

            if(reference.PrivateAssets.IsNotEmpty())
            {
                Assert.IsTrue(element.ChildNodes.Count is > 0 and < 3);
                Assert.AreEqual("PrivateAssets", element.ChildNodes[0]?.Name);
                Assert.AreEqual(reference.PrivateAssets, element.ChildNodes[0]?.InnerText);
            }
            
            if(reference.IncludeAssets.IsNotEmpty())
            {
                Assert.IsTrue(element.ChildNodes.Count is > 0 and < 3);
                Assert.AreEqual("IncludeAssets", element.ChildNodes[1]?.Name);
                Assert.AreEqual(reference.IncludeAssets, element.ChildNodes[1]?.InnerText);
            }

        }
       
    }
}