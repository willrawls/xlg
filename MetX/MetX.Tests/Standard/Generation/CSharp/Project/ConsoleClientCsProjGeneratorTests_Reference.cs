using MetX.Tests2.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests2.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod]
        public void InsertOneWithoutHintPath_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            var element = modifier.ItemGroup.ConfigurationManager.InsertOrUpdate();
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.Include, element.Attributes["Include"]?.Value);
        }
        
        [TestMethod]
        public void InsertOneWithHintPath_NotPresent()
        {
            var modifier = Piece.GetEmptyClient();
            modifier.ItemGroup.ConfigurationManager.HintPath = "Frank";
            var element = modifier.ItemGroup.ConfigurationManager.InsertOrUpdate();
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.Include, element.Attributes["Include"]?.Value);
            Assert.IsNotNull(element.FirstChild);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.HintPath, element.FirstChild.InnerText);
        }
        
        [TestMethod]
        public void InsertOneWithoutHintPath_Present()
        {
            var modifier = Piece.GetFullClient();
            var element = modifier.ItemGroup.ConfigurationManager.InsertOrUpdate();
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.Include, element.Attributes["Include"]?.Value);
        }
        
        [TestMethod]
        public void InsertOneWithHintPath_Present()
        {
            var modifier = Piece.GetFullClient();
            modifier.ItemGroup.ConfigurationManager.HintPath = "Frank";
            var element = modifier.ItemGroup.ConfigurationManager.InsertOrUpdate();
            Assert.IsNotNull(element);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.Include, element.Attributes["Include"]?.Value);
            Assert.IsNotNull(element.FirstChild);
            Assert.AreEqual(modifier.ItemGroup.ConfigurationManager.HintPath, element.FirstChild.InnerText);
        }
    }
}