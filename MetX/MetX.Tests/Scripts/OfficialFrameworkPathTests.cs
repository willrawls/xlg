using MetX.Standard.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetX.Tests.Scripts
{
    [TestClass]
    public class OfficialFrameworkPathTests
    {
        [TestMethod]
        public void GetFrameworkAssemblyPath_netstandard()
        {
            var actual = OfficialFrameworkPath.GetFrameworkAssemblyPath("netstandard");
            Assert.IsNotNull(actual);
            var expected = @"C:\Program Files (x86)\dotnet\shared\Microsoft.NETCore.App\5.0.9\netstandard.dll";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFrameworkAssemblyPath_MicrosoftAspNetCoreWebSockets()
        {
            var actual = OfficialFrameworkPath.GetFrameworkAssemblyPath("Microsoft.AspNetCore.WebSockets");
            Assert.IsNotNull(actual);
            var expected = @"C:\Program Files (x86)\dotnet\shared\Microsoft.AspNetCore.App\5.0.9\Microsoft.AspNetCore.WebSockets.dll";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFrameworkAssemblyPath_SystemWindowsForms()
        {
            var actual = OfficialFrameworkPath.GetFrameworkAssemblyPath("System.Windows.Forms");
            Assert.IsNotNull(actual);
            var expected = @"C:\Program Files (x86)\dotnet\shared\Microsoft.WindowsDesktop.App\5.0.9\System.Windows.Forms.dll";
            Assert.AreEqual(expected, actual);
        }
    }
}