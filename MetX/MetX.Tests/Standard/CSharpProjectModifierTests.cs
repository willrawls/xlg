using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    public class CSharpProjectModifierTests
    {
        public const string PiecesDirectory = @"Standard\ProjectPieces";
        
        [TestMethod]
        public void CheckForEmit_EmitMissing()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\WithoutEmit.xml");
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void CheckForEmit_PropertyGroupMissing()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\WithoutEmit.xml");
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void CheckForEmit_False()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\WithEmitFalse.xml");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void CheckForEmit_True()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\\WithEmitTrue.xml");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void SetEmitWhenFalse_True()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\WithEmitFalse.xml");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void SetEmitWhenMissing_True()
        {
            var project = Modifier.LoadFile($@"{PiecesDirectory}\WithoutEmit.xml");
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
    }
}