using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX.Standard.Library;

namespace MetX.Standard.Scripts
{
    public class XlgQuickScriptExecutableBuilder
    {
        public XlgQuickScript ScriptToRun { get; set; }
        public string Source { get; set; }
        public InMemoryCompiler<string> Compiler { get; set; }
        public string CsFilePath { get; set; }
        public string ExeFilePath { get; set; }
        public bool FinishedSuccessfully => Compiler?.CompiledSuccessfully == true;
        public string ParentDestination { get; set; }
        public string ExeFolder { get; set; }
        public string ExeFilename { get; set; }

        public XlgQuickScriptExecutableBuilder(XlgQuickScript scriptToRun)
        {
            ScriptToRun = scriptToRun;
            Source = scriptToRun.ToCSharp(true);
            var result = new XlgQuickScriptExecutableBuilder(scriptToRun);

            ParentDestination = scriptToRun.DestinationFilePath.TokensBeforeLast(@"\");
            ParentDestination = Path.Combine(ParentDestination, "bin");
            ExeFolder = Path.Combine(ParentDestination, DateTime
                .Now.ToString("s")
                .RemoveAll("-:".ToCharArray())
                .Replace("T", " "));
            ExeFilename = scriptToRun.Name.AsFilename() + ".exe";
            ExeFilePath = Path.Combine(ExeFolder, ExeFilename);
            CsFilePath = ExeFilePath.Replace(".exe", ".cs");
        }

        public void Compile()
        {
            Compiler = XlgQuickScript.CompileSource(Source, true, OfficialFrameworkPath.NETCore, ExeFolder, ExeFilename);

            if (Compiler.CompiledSuccessfully)
            {
                File.WriteAllText(CsFilePath, Source);
            }
            var sb = new StringBuilder("Compilation failure. Errors found include:" + Environment.NewLine + Environment.NewLine);
            var lines = new List<string>(Source.LineList());
            for (var index = 0; index < Compiler.Failures.Length; index++)
            {
                var error = Compiler.Failures[index].ToString();
                if (error.Contains("("))
                {
                    error = error.TokensAfterFirst("(").Replace(")", string.Empty);
                }
    
                sb.AppendLine(index + 1 + ": Line " + error);
                sb.AppendLine();
                if (error.Contains(Environment.NewLine))
                {
                    lines[Compiler.Failures[index].Location.Line() - 1] += "\t// " + error.Replace(Environment.NewLine, " ");
                }
                else if (Compiler.Failures[index].Location.Line() == 0)
                {
                    lines[0] += "\t// " + error;
                }
                else
                {
                    lines[Compiler.Failures[index].Location.Line()] += "\t// " + error;
                }
            }
        }
    }
}