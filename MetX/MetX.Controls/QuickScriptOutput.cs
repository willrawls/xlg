using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.Scripts;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Windows.Controls
{
    public sealed partial class QuickScriptOutput : ScriptRunningWindow, IShowText
    {
        public XlgQuickScript Script;
        public IRunQuickScript Scriptr;
        public FileSystemWatcher Watcher;

        public static Rectangle LastLocation;

        private const int FIRST_OFFSET_Y = 50;
        private const int FOLLOWING_OFFSET_Y = 25;
        
        private const int FIRST_OFFSET_X = 25;
        private const int FOLLOWING_OFFSET_X = 15;
        
        private static int nextOffsetX = FIRST_OFFSET_X;
        private static int nextOffsetY = FIRST_OFFSET_Y;

        public QuickScriptOutput(XlgQuickScript script, IRunQuickScript scriptr, string title, string output, IGenerationHost host) 
        {
            InitializeComponent();
            Text = "qkScrptR Output - " + title;
            Output.Text = output;
            Script = script;
            Scriptr = scriptr;
            Host = host;
            if (!string.IsNullOrEmpty(script.InputFilePath))
            {
                watchForChangesToolStripMenuItem.Enabled = true;
            }
        }

        public static QuickScriptOutput View(string title, string output, bool addLineNumbers, List<int> keyLines,
            bool wrapText, IGenerationHost host, QuickScriptOutput putNextToThisWindow = null) 
        {
            var quickScriptOutput = new QuickScriptOutput(title, output, addLineNumbers, keyLines, wrapText, host);

            Thread.Sleep(1);
            quickScriptOutput.Show();
            Thread.Sleep(1);
            quickScriptOutput.BringToFront();
            Thread.Sleep(1);
            quickScriptOutput.MoveIntoPosition(putNextToThisWindow);
            
            return quickScriptOutput;
        }

        private void MoveIntoPosition(QuickScriptOutput putNextToThisWindow = null)
        {
            if (putNextToThisWindow != null)
            {
                var boundsX = putNextToThisWindow.Bounds.X + putNextToThisWindow.Bounds.Width;
                var boundsY = putNextToThisWindow.Bounds.Y;

                LastLocation = new Rectangle(
                    boundsX, boundsY, 
                    putNextToThisWindow.Width, putNextToThisWindow.Height);
            }
            else if (LastLocation == Rectangle.Empty)
            {
                nextOffsetX = FIRST_OFFSET_X;
                nextOffsetY = FIRST_OFFSET_Y;
                LastLocation = Host.Boundary with
                {
                    X = Host.Boundary.Left + nextOffsetX, 
                    Y = Host.Boundary.Top + nextOffsetY,
                    Width = 700, 
                    Height = 500
                };
            }
            else
            {
                LastLocation = new Rectangle(LastLocation.X + nextOffsetX, LastLocation.Y + nextOffsetY, Width, Height);

                if(LastLocation.Top + 100 > Host.Boundary.Top + Host.Boundary.Height / 2
                   || LastLocation.Left + 100 > Host.Boundary.Left + Host.Boundary.Width / 2)
                {
                    LastLocation = Host.Boundary with
                    {
                        X = Host.Boundary.Left + nextOffsetX, 
                        Y = Host.Boundary.Top + nextOffsetY,
                    };
                }
            }

            nextOffsetX = FOLLOWING_OFFSET_X;
            nextOffsetY = FOLLOWING_OFFSET_Y;

            Bounds = LastLocation;
        }

        public void Find(string toFind)
        {
            var start = Output.Text.IndexOf(toFind, StringComparison.InvariantCultureIgnoreCase);
            if (start < 1)
                return;

            if(Output.Text.Length > 500)
            {
                Output.SelectionStart = Output.Text.Length - 500;
                Output.SelectionLength = 0;
                Output.ScrollToCaret();
            }

            if(start < 500)
            {
                Output.SelectionStart = start;
                Output.SelectionLength = toFind.Length;
                Output.ScrollToCaret();
            }
            else
            {
                Output.SelectionStart = start - 500;
                Output.SelectionLength = toFind.Length;
                Output.ScrollToCaret();
            }
        }

        public QuickScriptOutput(string title, string output, bool addLineNumbers, List<int> keyLines, bool wrapText, IGenerationHost host) 
        {
            InitializeComponent();
            Text = "qkScrptR Output - " + title;
            Host = host;

            Output.Text = addLineNumbers 
                ? output.InsertLineNumbers(keyLines) 
                : output;
            Output.WordWrap = wrapText;
            Output.SelectionStart = 0;
            Output.SelectionLength = 0;
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
            BringToFront();
            Output.SelectionStart = 0;
            Output.SelectionLength = 0;
            Output.Focus();
            Thread.Sleep(1);
            MoveIntoPosition();
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
                ToolWindow.Host ??= Host;
                GuiContext.RunQuickScript(this, Script, null, Host);
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
                    ToolWindow.Host ??= Host;
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