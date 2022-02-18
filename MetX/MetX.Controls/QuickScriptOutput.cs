using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetX.Standard.Interfaces;
using MetX.Standard.Scripts;
using MetX.Standard.Library.Extensions;
using MetX.Standard.Library.Strings;

namespace MetX.Controls
{
    public sealed partial class QuickScriptOutput : ScriptRunningWindow, IShowText
    {
        public XlgQuickScript Script;
        public IRunQuickScript Scriptr;
        public FileSystemWatcher Watcher;

        public static int LastTopLeftPosition;

        public Point? MyTopLeftPosition;

        private static Point[] _topLeftPositions;
        public static Point[] TopLeftPositions
        {
            get
            {
                if (_topLeftPositions != null) return _topLeftPositions;

                _topLeftPositions = new Point[]
                {
                    CalculateTopLeftPosition(5, 5),
                    CalculateTopLeftPosition(25, 5),
                    CalculateTopLeftPosition(45, 5),
                    CalculateTopLeftPosition(65, 5),
                    CalculateTopLeftPosition(85, 5),

                    CalculateTopLeftPosition(5, 35),
                    CalculateTopLeftPosition(25, 35),
                    CalculateTopLeftPosition(45, 35),
                    CalculateTopLeftPosition(65, 35),
                    CalculateTopLeftPosition(85, 35),

                    CalculateTopLeftPosition(5, 75),
                    CalculateTopLeftPosition(25, 75),
                    CalculateTopLeftPosition(45, 75),
                    CalculateTopLeftPosition(65, 75),
                    CalculateTopLeftPosition(85, 75),
                };

                return _topLeftPositions;
            }
        }

        public static Point CalculateTopLeftPosition(int percentX, int percentY)
        {
            var onePercentX = Screen.PrimaryScreen.Bounds.Width / 100.0;
            var onePercentY = Screen.PrimaryScreen.Bounds.Height / 100.0;

            return new Point((int) onePercentX * percentX, (int) onePercentY * percentY);
        }

        public QuickScriptOutput(XlgQuickScript script, IRunQuickScript scriptr, string title, string output, IGenerationHost host) 
        {
            InitializeComponent();
            Text = "qkScrptR Output - " + title;
            MyTopLeftPosition = null;
            Output.Text = output;
            Script = script;
            Scriptr = scriptr;
            Host = host;
            if (!string.IsNullOrEmpty(script.InputFilePath))
            {
                watchForChangesToolStripMenuItem.Enabled = true;
            }
        }

        public static QuickScriptOutput View(string title, string output, bool addLineNumbers, List<int> keyLines, bool wrapText, IGenerationHost host) 
        {
            var quickScriptOutput = new QuickScriptOutput(title, output, addLineNumbers, keyLines, wrapText, host);

            quickScriptOutput.Show();
            Thread.Sleep(1);
            quickScriptOutput.BringToFront();
            Thread.Sleep(1);
            quickScriptOutput.MoveIntoPosition();
            
            return quickScriptOutput;
        }

        private void MoveIntoPosition()
        {
            if (MyTopLeftPosition == null)
            {
                MyTopLeftPosition = TopLeftPositions[LastTopLeftPosition];
                LastTopLeftPosition++;
                if (LastTopLeftPosition > TopLeftPositions.Length - 1)
                    LastTopLeftPosition = 0;
            }
            Top = MyTopLeftPosition.Value.Y;
            Left = MyTopLeftPosition.Value.X;
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
            MyTopLeftPosition = null;
            Host = host;

            Output.Text = addLineNumbers 
                ? output.InsertLineNumbers(keyLines) 
                : output;
            Output.WordWrap = wrapText;
            Output.SelectionStart = 0;
            Output.SelectionLength = 0;
        }

        static QuickScriptOutput()
        {
            LastTopLeftPosition = 0;
            _topLeftPositions = null;
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