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
using MetX.Library;
using Microsoft.CSharp;

namespace XLG.Pipeliner
{
    public partial class QuickScriptEditor : Form
    {
        public static List<QuickScriptEditor> Editors = new List<QuickScriptEditor>();
        public static List<QuickScriptOutput> OutputWindows = new List<QuickScriptOutput>();
        public static string Template = null;
        public XlgQuickScript CurrentScript;
        public XlgQuickScriptFile Scripts;

        public QuickScriptEditor(string filePath)
        {
            InitializeComponent();
            LoadQuickScriptsFile(filePath);
        }

        private QuickScriptEditor(XlgQuickScriptFile scripts)
        {
            Scripts = scripts;
            CurrentScript = scripts.Default;
        }

        public void OpenNewEditor()
        {
            QuickScriptEditor quickScriptEditor = new QuickScriptEditor(Scripts);
            Editors.Add(quickScriptEditor);
            quickScriptEditor.Show(this);
            quickScriptEditor.BringToFront();
        }

        public void OpenNewOutput(string title, string output)
        {
            QuickScriptOutput quickScriptOutput = new QuickScriptOutput(title, output);
            OutputWindows.Add(quickScriptOutput);
            quickScriptOutput.Show(this);
            quickScriptOutput.BringToFront();
        }

        public void UpdateBeforeSave()
        {
            if (CurrentScript != null)
            {
                CurrentScript.Update(QuickScriptList.Text, QuickScriptInput.Text, DestinationList.Text);
            }
        }

        private static void ViewTextInNotepad(string source)
        {
            try
            {
                string tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, source);
                Process.Start("notepad", tempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void QuickScriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBeforeSave();
            XlgQuickScript selectedScript = Scripts.FirstOrDefault(x => x.Name == QuickScriptList.Text);
            if (selectedScript != null) UpdateFormWithScript(selectedScript);
        }

        private void UpdateFormWithScript(XlgQuickScript selectedScript)
        {
            CurrentScript = selectedScript;
            QuickScriptList.Text = CurrentScript.Name;
            QuickScriptInput.Text = CurrentScript.Input;
            DestinationList.Text = CurrentScript.Destination.ToString();
        }

        private void DestinationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentScript == null) return;
            try
            {
                if (!Enum.TryParse(DestinationList.Text, out CurrentScript.Destination))
                    MessageBox.Show("That didn't work");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private IProcessForClipScript GetClipScriptProcessor()
        {
            if (Template == null)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(IProcessForClipScript));
                using (Stream stream = assembly.GetManifestResourceStream("MetX.Library.ClipScriptProcessor.cs"))
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    Template = new StreamReader(stream).ReadToEnd();
                }
            }
            if (string.IsNullOrEmpty(Template))
            {
                return null;
            }
            string[] expandedClipScriptLines = QuickScriptInput.Lines;
            for (int i = 0; i < expandedClipScriptLines.Length; i++)
            {
                string currScriptLine = expandedClipScriptLines[i];
                int indent = currScriptLine.Length - currScriptLine.Trim().Length;

                if (currScriptLine.Contains("~~:"))
                {
                    currScriptLine = currScriptLine.Replace(@"\%", "~$~$").Replace("\"", "~#~$").Trim();

                    while (currScriptLine.Contains("%"))
                    {
                        string variableContent = currScriptLine.TokenAt(2, "%");
                        string resolvedContent = string.Empty;
                        if (variableContent.Length > 0)
                        {
                            if (variableContent.Contains(" "))
                            {
                                string variableName = variableContent.FirstToken();
                                string variableIndex = variableContent.TokenAt(2);
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

            string source = Template.Replace("//~~ProcessLine~~//", string.Join(Environment.NewLine, expandedClipScriptLines));

            CompilerParameters compilerParameters = new CompilerParameters
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
            CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters, source);

            if (compilerResults.Errors.Count <= 0)
            {
                Assembly assembly = compilerResults.CompiledAssembly;
                IProcessForClipScript clipScriptProcessor =
                    assembly.CreateInstance("MetX.ClipScriptProcessor") as IProcessForClipScript;
                return clipScriptProcessor;
            }

            for (int index = 0; index < compilerResults.Errors.Count; index++)
            {
                MessageBox.Show("Compile error #" + (index + 1) + ": " + compilerResults.Errors[index]);
            }

            ViewTextInNotepad(source);

            return null;
        }

        private void LoadQuickScriptsFile(string clipScriptsFilePath)
        {
            CurrentScript = null;

            Scripts = XlgQuickScriptFile.Load(clipScriptsFilePath);
            CurrentScript = Scripts.Default;
        }

        private void RunClipScript_Click(object sender, EventArgs e)
        {
            RunClipScriptNow();
        }

        private void RunClipScriptNow()
        {
            try
            {
                string toParse = Clipboard.GetText();
                if (string.IsNullOrEmpty(toParse)) return;

                string[] lines = toParse.Replace("\r", string.Empty)
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length <= 0) return;

                IProcessForClipScript clipScriptProcessor = GetClipScriptProcessor();
                if (clipScriptProcessor == null) return;

                StringBuilder sb = new StringBuilder();
                int lineCount = lines.Length;
                Dictionary<string, string> d = new Dictionary<string, string>();

                if (lines.Where((t, index) => !clipScriptProcessor.ProcessLine(sb, t, index, lineCount, d)).Any()) return;
                if (sb.Length <= 0) return;

                try
                {
                    switch (DestinationList.Text.ToLower().Replace(" ", string.Empty))
                    {
                        case "textbox":
                            OpenNewOutput(QuickScriptList.Text + " at " + DateTime.Now.ToString("G"), sb.ToString());
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

        private void SaveClipScript_Click(object sender, EventArgs e)
        {
            if (Scripts != null)
                Scripts.Save();
        }
    }
}