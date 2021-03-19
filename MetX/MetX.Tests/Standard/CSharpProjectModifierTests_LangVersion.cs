using System.IO;
using MetX.Standard.Generation.CSharp.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Standard
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class CSharpProjectModifierTests_LangVersion
    {
        public const string PiecesDirectory = @"Standard\ProjectPieces\LangVersion";
        
        [TestMethod]
        public void LangVersion_GenerateToPathMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.LangVersionMissing);
            Assert.IsNull(GetProjectPiece("Missing").PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void LangVersion_PropertyGroupMissing()
        {
            Assert.IsTrue(GetProjectPiece("Missing").PropertyGroups.LangVersionMissing);
            Assert.IsNull(GetProjectPiece("Missing").PropertyGroups.LangVersion);
        }

        [TestMethod]
        public void LangVersion_EqualsXyz()
        {
            var project = GetProjectPiece("EqualsXyz");
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            Assert.AreEqual("Xyz", project.PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void LangVersion_Blank()
        {
            var project = GetProjectPiece("EqualsBlank");
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
        }
        
        [TestMethod]
        public void SetLangVersion_XyzToBlank() // Blank or null means remove that node
        {
            var project = GetProjectPiece("EqualsXyz");
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = "";
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetLangVersion_XyzToNullAsBlank()
        {
            var project = GetProjectPiece("EqualsXyz");
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = null;
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetLangVersion_MissingToXyz()
        {
            var project = GetProjectPiece("Missing");
            Assert.IsTrue(project.PropertyGroups.LangVersionMissing);
            var Xyz = "Xyz";
            project.PropertyGroups.LangVersion = Xyz;
            Assert.AreEqual(Xyz, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }
        
        [TestMethod]
        public void SetGenerateToPathWhen_MissingToNull() // Don't add it if there's no value
        {
            var project = GetProjectPiece("Missing");
            Assert.IsTrue(project.PropertyGroups.LangVersionMissing);
            project.PropertyGroups.LangVersion = "";
            Assert.AreEqual(null, project.PropertyGroups.LangVersion);
            Assert.IsFalse(project.PropertyGroups.LangVersionMissing);
        }

        private static Modifier GetProjectPiece(string pieceName)
        {
            var filePath = $@"{PiecesDirectory}\{pieceName}.xml";
            Assert.IsTrue(File.Exists(filePath));
            return Modifier.LoadFile(filePath);
        }

    }
}