using System;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using System.Text;
using System.Runtime.InteropServices;
using CustomToolGenerator;

using MetX;
using MetX.IO;
using MetX.Web;
using MetX.Security;
using MetX.SubSonic;
using MetX.Web.Virtual;

namespace MetX.Gen
{
    [Guid("0F864956-1B00-44CF-B689-A514A8F2A3D8")]
    [ProgId("MetXGen.XLGen")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class MetXGenXLGen : BaseCodeGeneratorWithSite
    {
        public const string x = "y";
        public override string GetDefaultExtension()
        {
            return ".XLGen" + base.GetDefaultExtension();
        }

        protected override byte[] GenerateCode(string file, string contents)
        {              
            try
            {
                CodeGenerator Gen = new CodeGenerator(file, contents);
                return Encoding.ASCII.GetBytes(Gen.Code);
            }
            catch (Exception ex)
            {
                MessageBox.Show("--- MetXGen.XLGen FAILED: " + ex.ToString());
            }
            return null;
        }
    }
}