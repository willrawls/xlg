using MetX.Standard.Primary.Generation;

namespace MetX.Standard.Test.Generation.CSharp.Project;

[TestClass]
public class CsProjGeneratorOptionsTests
{
    [TestMethod]
    public void AssertValid_OptionsAreValid()
    {
        var data = CsProjGeneratorOptions.Defaults();
        var actual = data.AssertValid();

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual);
    }

}