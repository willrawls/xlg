using MetX.Console.Tests.Standard.Generation.CSharp.Project.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Console.Tests.Standard.Generation.CSharp.Project;

[TestClass]
public partial class ConsoleClientCsProjGeneratorTests
{
    [TestMethod]
    public void CheckForEmit_EmitMissing()
    {
        Assert.IsTrue(Piece.Get(Piece.Missing, Piece.Emit).PropertyGroups.EmitCompilerGeneratedFilesMissing);
    }
        
    [TestMethod]
    public void CheckForEmit_PropertyGroupMissing()
    {
        Assert.IsTrue(Piece.Get(Piece.Missing, Piece.Emit).PropertyGroups.EmitCompilerGeneratedFilesMissing);
    }

    [TestMethod]
    public void CheckForEmit_False()
    {
        var project = Piece.Get("EqualsFalse", Piece.Emit);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
    }
        
    [TestMethod]
    public void CheckForEmit_True()
    {
        var project = Piece.Get("EqualsTrue", Piece.Emit);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
    }
        
    [TestMethod]
    public void SetEmit_FromFalseToTrue()
    {
        var project = Piece.Get("EqualsFalse", Piece.Emit);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
        project.PropertyGroups.EmitCompilerGeneratedFiles = true;
        Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
    }
        
    [TestMethod]
    public void SetEmitWhenMissing_True()
    {
        var project = Piece.Get(Piece.Missing, Piece.Emit);
        Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFiles);
        project.PropertyGroups.EmitCompilerGeneratedFiles = true;
        Assert.IsTrue(project.PropertyGroups.EmitCompilerGeneratedFiles);
        Assert.IsFalse(project.PropertyGroups.EmitCompilerGeneratedFilesMissing);
    }

}