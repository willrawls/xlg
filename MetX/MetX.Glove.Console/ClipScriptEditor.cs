using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MetX.Data;
using MetX.Glove;
using MetX.Library;
using Microsoft.CSharp;

namespace XLG.Pipeliner
{
    public partial class ClipScriptEditor : Form
    {
        private readonly GloveMain Main;
        public XlgClipScript CurrentScript;

        public ClipScriptEditor(GloveMain main)
        {
            InitializeComponent();
            Main = main;
        }

        private void RunClipScript_Click(object sender, EventArgs e)
        {
            RunClipScriptNow();
        }

        private void RunClipScriptNow()
        {
            try
            {
                var toParse = Clipboard.GetText();
                if (string.IsNullOrEmpty(toParse)) return;

                var lines = toParse.Replace("\r", string.Empty)
                    .Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length <= 0) return;

                var clipScriptProcessor = GetClipScriptProcessor();
                if (clipScriptProcessor == null) return;

                var sb = new StringBuilder();
                var lineCount = lines.Length;
                var d = new Dictionary<string, string>();

                if (lines.Where((t, index) => !clipScriptProcessor.ProcessLine(sb, t, index, lineCount, d)).Any()) return;
                if (sb.Length <= 0) return;

                try
                {
                    switch (DestinationList.Text.ToLower().Replace(" ", string.Empty))
                    {
                        case "textbox":
                            Main.OpenNewClipScriptOutput(ClipScriptList.Text + " at " + DateTime.Now.ToString("G"), sb.ToString());
                            break;

                        case "clipboard":
                            Clipboard.Clear();
                            Clipboard.SetText(sb.ToString());
                            break;

                        case "notepad":
                            ViewTextInNotepad(sb.ToString());
                            break;

                        case "file":
                            // TODO
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private IProcessForClipScript GetClipScriptProcessor()
        {
            if (GloveMain.ClipScriptProcessorSourceTemplate == null)
            {
                var assembly = Assembly.GetAssembly(typeof (IProcessForClipScript));
                using (var stream = assembly.GetManifestResourceStream("MetX.Library.ClipScriptProcessor.cs"))
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    GloveMain.ClipScriptProcessorSourceTemplate = new StreamReader(stream).ReadToEnd();
                }
            }
            if (string.IsNullOrEmpty(GloveMain.ClipScriptProcessorSourceTemplate))
            {
                return null;
            }
            var expandedClipScriptLines = ClipScriptInput.Lines;
            for (var i = 0; i < expandedClipScriptLines.Length; i++)
            {
                var currScriptLine = expandedClipScriptLines[i];
                var indent = currScriptLine.Length - currScriptLine.Trim().Length;

                if (currScriptLine.Contains("~~:"))
                {
                    currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

                    while (currScriptLine.Contains("%"))
                    {
                        var variableContent = currScriptLine.TokenAt(2, "%");
                        var resolvedContent = string.Empty;
                        if (variableContent.Length > 0)
                        {
                            if (variableContent.Contains(" "))
                            {
                                var variableName = variableContent.FirstToken();
                                var variableIndex = variableContent.TokenAt(2);
                                if (variableName != "d")
                                    resolvedContent = "\" + (" + variableName + ".Length <= " + variableIndex + " ? string.Empty : " + variableName + "[" + variableIndex + "]) + \"";
                                else
                                    resolvedContent = "\" + " + variableName + "[" + variableIndex.Replace("~#~$", "\"") + "] + \"";
                            }
                            else
                            {
                                resolvedContent = "\" + " + variableContent + " + \"";
                            }
                        }
                        currScriptLine = currScriptLine.Replace("%" + variableContent + "%", resolvedContent);
                    }
                    currScriptLine = "sb.AppendLine(\"" + currScriptLine.Mid(3) + "\");";
                    currScriptLine =
                        currScriptLine.Replace("AppendLine(\" + ", "AppendLine(")
                            .Replace(" + \"\")", ")")
                            .Replace("sb.AppendLine(\"\")", "sb.AppendLine()");
                    currScriptLine = (new string(' ', indent)) + "            " +
                                     currScriptLine.Replace(" + \"\" + ", string.Empty)
                                         .Replace("~$~$", @"\%")
                                         .Replace("~#~$", "\\\"");
                    expandedClipScriptLines[i] = currScriptLine;
                }
                else
                {
                    expandedClipScriptLines[i] = (new string(' ', indent)) + "            " + currScriptLine;
                }
            }

            var source = GloveMain.ClipScriptProcessorSourceTemplate.Replace("//~~ProcessLine~~//", string.Join(Environment.NewLine, expandedClipScriptLines));

            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };
            compilerParameters
                .ReferencedAssemblies
                .AddRange(
                    AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(a => !a.IsDynamic)
                        .Select(a => a.Location)
                        .ToArray());
            var compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters, source);

            if (compilerResults.Errors.Count <= 0)
            {
                var assembly = compilerResults.CompiledAssembly;
                var clipScriptProcessor =
                    assembly.CreateInstance("MetX.ClipScriptProcessor") as IProcessForClipScript;
                return clipScriptProcessor;
            }

            for (var index = 0; index < compilerResults.Errors.Count; index++)
            {
                MessageBox.Show("Compile error #" + (index + 1) + ": " + compilerResults.Errors[index]);
            }

            ViewTextInNotepad(source);

            return null;
        }

        private static void ViewTextInNotepad(string source)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, source);
                Process.Start(GloveMain.AppData.TextEditor, tempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateBeforeSave()
        {
            if (CurrentScript != null)
            {
                CurrentScript.Update(ClipScriptList.Text, DestinationList.Text, ClipScriptInput.Text);
            }
        }

        private void DestinationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            start here
        }

        private void ClipScriptList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}