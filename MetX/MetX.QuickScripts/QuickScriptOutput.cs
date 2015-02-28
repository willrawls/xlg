using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Data;
using MetX.Library;

namespace XLG.QuickScripts
{
    public sealed partial class QuickScriptOutput : Form
    {
        public XlgQuickScript Script;
        public QuickScriptEditor Parent;
        public FileSystemWatcher Watcher;

        public QuickScriptOutput(QuickScriptEditor parent, string title, string output, XlgQuickScript script)
        {
            InitializeComponent();
            Text = "QuickScript Output - " + title;
            Output.Text = output;
            Script = script;
            Parent = parent;

            if (!string.IsNullOrEmpty(script.InputFilePath)) watchForChangesToolStripMenuItem.Enabled = true;
        }

        private void QuickScriptOutput_Load(object sender, EventArgs e)
        {
            Output.SelectAll();
            Output.Focus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = Text.Replace(":", string.Empty).Replace("/", "-");
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = ".txt";
                saveFileDialog1.Filter = "All files(*.*)|*.*";
                DialogResult result = saveFileDialog1.ShowDialog(this);
                if (result == DialogResult.OK && saveFileDialog1.FileName != null)
                {
                    File.WriteAllText(saveFileDialog1.FileName, Output.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(Output.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void runAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Parent.RunQuickScript(Script, this);
                if (!string.IsNullOrEmpty(Script.InputFilePath)) 
                    watchForChangesToolStripMenuItem.Enabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void watchForChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (watchForChangesToolStripMenuItem.Text == "Stop &watching")
                {
                    StopWatching();
                }
                else if(!string.IsNullOrEmpty(Script.InputFilePath) &&  File.Exists(Script.InputFilePath))
                {
                    string path = Script.InputFilePath.TokensBeforeLast(@"\");
                    string file = Script.InputFilePath.LastToken(@"\");
                    Watcher = new FileSystemWatcher(path, file)
                    {
                        NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.Size ),
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = false,
                    };
                    Watcher.Changed += OnChanged;
                    watchForChangesToolStripMenuItem.Text = "Stop &watching";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void StopWatching()
        {
            watchForChangesToolStripMenuItem.Text = "&Watch for changes";
            Task.Factory.StartNew(() =>
            {
                if (Watcher != null)
                {
                    Watcher.Dispose();
                }
                Watcher = null;
            });
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Script.InputFilePath)) Parent.RunQuickScript(Script, this);
                else
                {
                    StopWatching();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
