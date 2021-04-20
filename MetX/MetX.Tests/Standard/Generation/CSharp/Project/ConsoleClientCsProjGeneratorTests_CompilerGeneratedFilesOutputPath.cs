using System.IO;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    public partial class ConsoleClientCsProjGeneratorTests
    {
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_GenerateToPathMissing()
        {
            Assert.IsTrue(Piece.Get(Piece.Missing, Piece.GenerateToPath).PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.IsNull(Piece.Get(Piece.Missing, Piece.GenerateToPath).PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_PropertyGroupMissing()
        {
            Assert.IsTrue(Piece.Get(Piece.Missing, Piece.GenerateToPath).PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.IsNull(Piece.Get(Piece.Missing, Piece.GenerateToPath).PropertyGroups.CompilerGeneratedFilesOutputPath);
        }

        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_EqualsGenerated()
        {
            var project = Piece.Get("EqualsGenerated", Piece.GenerateToPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.AreEqual("Generated", project.PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_Blank()
        {
            var project = Piece.Get("EqualsBlank", Piece.GenerateToPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_GeneratedToBlank() // Blank or null means remove that node
        {
            var project = Piece.Get("EqualsGenerated", Piece.GenerateToPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = "";
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_GeneratedToNullAsBlank()
        {
            var project = Piece.Get("EqualsGenerated", Piece.GenerateToPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = null;
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_MissingToGenerated()
        {
            var project = Piece.Get(Piece.Missing, Piece.GenerateToPath);
            Assert.IsTrue(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            var generated = "Generated";
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = generated;
            Assert.AreEqual(generated, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetGenerateToPathWhen_MissingToNull() // Don't add it if there's no value
        {
            var project = Piece.Get(Piece.Missing, Piece.GenerateToPath);
            Assert.IsTrue(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = "";
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
    }
}