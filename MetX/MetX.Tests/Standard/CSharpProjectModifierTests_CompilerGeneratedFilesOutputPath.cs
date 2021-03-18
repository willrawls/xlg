using System.IO;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class CSharpProjectModifierTests_CompilerGeneratedFilesOutputPath
    {
        public const string PiecesDirectory = @"Standard\ProjectPieces\GenerateToPath";
        
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_GenerateToPathMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.IsNull(GetProjectPiece("Missing").PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_PropertyGroupMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.IsNull(GetProjectPiece("Missing").PropertyGroups.CompilerGeneratedFilesOutputPath);
        }

        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_EqualsGenerated()
        {
            var project = GetProjectPiece("EqualsGenerated");
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.AreEqual("Generated", project.PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void CompilerGeneratedFilesOutputPath_Blank()
        {
            var project = GetProjectPiece("EqualsBlank");
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_GeneratedToBlank() // Blank or null means remove that node
        {
            var project = GetProjectPiece("EqualsGenerated");
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = "";
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_GeneratedToNullAsBlank()
        {
            var project = GetProjectPiece("EqualsGenerated");
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = null;
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetCompilerGeneratedFilesOutputPath_MissingToGenerated()
        {
            var project = GetProjectPiece("Missing");
            Assert.IsTrue(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            var generated = "Generated";
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = generated;
            Assert.AreEqual(generated, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }
        
        [TestMethod]
        public void SetGenerateToPathWhen_MissingToNull() // Don't add it if there's no value
        {
            var project = GetProjectPiece("Missing");
            Assert.IsTrue(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
            project.PropertyGroups.CompilerGeneratedFilesOutputPath = "";
            Assert.AreEqual(null, project.PropertyGroups.CompilerGeneratedFilesOutputPath);
            Assert.IsFalse(project.PropertyGroups.CompilerGeneratedFilesOutputPathMissing);
        }

        private static Modifier GetProjectPiece(string pieceName)
        {
            var filePath = $@"{PiecesDirectory}\{pieceName}.xml";
            Assert.IsTrue(File.Exists(filePath));
            return Modifier.LoadFile(filePath);
        }

    }
}