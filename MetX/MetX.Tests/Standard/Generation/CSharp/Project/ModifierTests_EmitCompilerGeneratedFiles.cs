using System.IO;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class ModifierTests_EmitCompilerGeneratedFiles
    {
        public const string Area = @"Emit";
        
        [TestMethod]
        public void CheckForEmit_EmitMissing()
        {
            Assert.IsTrue(Piece.Get("Missing", Area).PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void CheckForEmit_PropertyGroupMissing()
        {
            Assert.IsTrue(Piece.Get("Missing", Area).PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }

        [TestMethod]
        public void CheckForEmit_False()
        {
            var project = Piece.Get("EqualsFalse", Area);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void CheckForEmit_True()
        {
            var project = Piece.Get("EqualsTrue", Area);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        }
        
        [TestMethod]
        public void SetEmit_FromFalseToTrue()
        {
            var project = Piece.Get("EqualsFalse", Area);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }
        
        [TestMethod]
        public void SetEmitWhenMissing_True()
        {
            var project = Piece.Get("Missing", Area);
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
            project.PropertyGroups.EmitCompilerGeneratedFiles = true;
            Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
            Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        }

    }
}