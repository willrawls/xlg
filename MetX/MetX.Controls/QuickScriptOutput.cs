using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;
using MetX.Standard.Scripts;
using MetX.Standard.Interfaces;

namespace MetX.Controls
{
    public sealed partial class QuickScriptOutput : ScriptRunningWindow, IShowText
    {
        public XlgQuickScript Script;
        public IRunQuickScript Scriptr;
        public FileSystemWatcher Watcher;

        public QuickScriptOutput(XlgQuickScript script, IRunQuickScript scriptr, string title, string output, IGenerationHost host) 
        {
            InitializeComponent();
            Text = "QuickScript Output - " + title;
            Output.Text = output;
            Script = script;
            Scriptr = scriptr;
            Host = host;
            if (!string.IsNullOrEmpty(script.InputFilePath))
            {
                watchForChangesToolStripMenuItem.Enabled = true;
            }
        }

        public string Title
        {
            get => Text;
            set => Text = value;
        }

        public string TextToShow
        {
            get => Output.Text;
            set => Output.Text = value;
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
                var result = saveFileDialog1.ShowDialog(this);
                if (result == DialogResult.OK && saveFileDialog1.FileName != null)
                {
                    File.WriteAllText(saveFileDialog1.FileName, Output.Text);
                }
            }
            catch (Exception ex)
            {
                Host.MessageBox.Show(ex.ToString());
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
                Host.MessageBox.Show(exception.ToString());
            }
        }

        private void runAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Window.Host ??= Host;
                Context.RunQuickScript(this, Script, null, Host);
                if (!string.IsNullOrEmpty(Script.InputFilePath))
                {
                    watchForChangesToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
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
                else if (!string.IsNullOrEmpty(Script.InputFilePath) && File.Exists(Script.InputFilePath))
                {
                    var path = Script.InputFilePath.TokensBeforeLast(@"\");
                    var file = Script.InputFilePath.LastToken(@"\");
                    Watcher = new FileSystemWatcher(path, file)
                    {
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = false,
                    };
                    Watcher.Changed += OnChanged;
                    watchForChangesToolStripMenuItem.Text = "Stop &watching";
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }

        private void StopWatching()
        {
            watchForChangesToolStripMenuItem.Text = "&Watch for changes";
            Task.Factory.StartNew(() =>
            {
                Watcher?.Dispose();
                Watcher = null;
            });
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Script.InputFilePath))
                {
                    Window.Host ??= Host;
                    Scriptr.RunQuickScript(this, Script, this);
                }
                else
                {
                    StopWatching();
                }
            }
            catch (Exception exception)
            {
                Host.MessageBox.Show(exception.ToString());
            }
        }
    }
}