using System.IO;
using MetX.Standard.Generation.CSharp.Project;
using MetX.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard.Generation.CSharp.Project
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class ModifierTests_LangVersion
    {
        public const string Area = @"LangVersion";
        
        [TestMethod]
        public void LangVersion_GenerateToPathMissing()
        {
            Assert.IsTrue(Piece.Get("Missing", Area).PropertyGroups.LangVersionMissing);
            Assert.IsNull(Piece.Get("Missing", Area).PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void LangVersion_PropertyGroupMissing()
        {
            Assert.IsTrue(Piece.Get("Missing", Area).PropertyGroups.LangVersionMissing);
            Assert.IsNull(Piece.Get("Missing", Area).PropertyGroups.LangVersion);
        }

        [TestMethod]
        public void LangVersion_EqualsXyz()
        {
            var project = Piece.Get("EqualsXyz", Area);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            Assert.AreEqual("Xyz", project.PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void LangVersion_Blank()
        {
            var project = Piece.Get("EqualsBlank", Area);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void SetLangVersion_XyzToBlank() // Blank or null means remove that node
        {
            var project = Piece.Get("EqualsXyz", Area);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = "";
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetLangVersion_XyzToNullAsBlank()
        {
            var project = Piece.Get("EqualsXyz", Area);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = null;
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetLangVersion_MissingToXyz()
        {
            var project = Piece.Get("Missing", Area);
            Assert.IsTrue(project.PropertyGroups.LangVersionMissing);
            var Xyz = "Xyz";
            project.PropertyGroups.LangVersion = Xyz;
            Assert.AreEqual(Xyz, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetGenerateToPathWhen_MissingToNull() // Don't add it if there's no value
        {
            var project = Piece.Get("Missing", Area);
            Assert.IsTrue(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = "";
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }

    }
}