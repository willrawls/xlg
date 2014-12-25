using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Microsoft.CSharp;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Hosting;

namespace MetX.Data
{
    /// <summary>The XLG file build provider encorporating Data level functionality and HttpHandler style processing without additional files</summary>
    public class xlgBuildProvider : System.Web.Compilation.BuildProvider
    {
        /// <summary>Generates the code unit via CodeGenerator</summary>
        /// <param name="assemblyBuilder">The assemblyBuilder to build code into</param>
        public override void GenerateCode(System.Web.Compilation.AssemblyBuilder assemblyBuilder)
        {
            try
            {
                CodeGenerator Gen = new CodeGenerator(VirtualPath);
                CodeSnippetCompileUnit unit = new CodeSnippetCompileUnit(Gen.Code);
                assemblyBuilder.AddCodeCompileUnit(this, unit);
            }
            catch (Exception x)
            {
                throw new Exception(x.Message + " " + x.StackTrace, x);
            }
        }

    }
}
