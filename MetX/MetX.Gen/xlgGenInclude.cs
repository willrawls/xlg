using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Runtime.InteropServices;
using CustomToolGenerator;

using MetX;

namespace MetX.Gen
{
    [Guid("790BB2D7-BF4D-40D1-8161-C6BA5563D8D8")]
    [ProgId("MetXGen.Include")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class MetXGenInclude : BaseCodeGeneratorWithSite
    {

        public const string x = "y";

        public override string GetDefaultExtension()
        {

            return ".GenInclude" + base.GetDefaultExtension();
        }

        protected override byte[] GenerateCode(string file, string contents)
        {
              
            StringBuilder code = new StringBuilder();
            StringBuilder AlteredContents = new StringBuilder(contents);
            AlteredContents.Replace("\\", "\\\\");
            AlteredContents.Replace("\n", "\\n");
            AlteredContents.Replace("\r", "\\r");
            AlteredContents.Replace("\"", "\\\"");
            AlteredContents.Replace("\r", "\\r");

            string constantName = Path.GetExtension(file);
            constantName = constantName.Substring(1).Replace(".", "_");
            
            code.AppendLine("namespace " + FileNameSpace + " {");
            code.AppendLine("   /// <summary>Provides access to static virtual file content for files</summary>");
            code.AppendLine("   public partial class " + Path.GetFileNameWithoutExtension(file) + " {");
            code.AppendLine("       /// <summary>The static contents of the file: \"" + file + "\" as it existed at compile time.</summary>");
            code.Append("       public const string " + constantName + " = \"");
            code.Append(AlteredContents);
            code.AppendLine("\";");
            code.AppendLine("       /// <summary>Returns " + constantName + " inside a StringBuilder.</summary>");
            code.AppendLine("       /// <returns>A StringBuilder with the compile time file contents</returns>");
            code.AppendLine("       public static System.Text.StringBuilder " + constantName + "StringBuilder { get { return new System.Text.StringBuilder(" + constantName + "); } }");
            code.AppendLine("   }");
            code.AppendLine("}");
            return Encoding.ASCII.GetBytes(code.ToString());
        }
    }
}