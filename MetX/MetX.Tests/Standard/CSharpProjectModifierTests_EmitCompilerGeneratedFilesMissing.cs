using System.IO;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class CSharpProjectModifierTests_EmitCompilerGeneratedFiles
    {
        public const string PiecesDirectory = @"Standard\ProjectPieces\Emit";
        
        [TestMethod]
        public void CheckForEmit_EmitMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void CheckForEmit_PropertyGroupMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }

        [TestMethod]
        public void CheckForEmit_False()
        {
            var project = GetProjectPiece("EqualsFalse");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void CheckForEmit_True()
        {
            var project = GetProjectPiece("EqualsTrue");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void SetEmit_FromFalseToTrue()
        {
            var project = GetProjectPiece("EqualsFalse");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void SetEmitWhenMissing_True()
        {
            var project = GetProjectPiece("Missing");
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }

        private static Modifier GetProjectPiece(string pieceName)
        {
            var filePath = $@"{PiecesDirectory}\{pieceName}.xml";
            Assert.IsTrue(File.Exists(filePath));
            return Modifier.LoadFile(filePath);
        }

    }
}