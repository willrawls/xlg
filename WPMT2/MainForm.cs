using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetX.Standard.Library;
using MetX.Standard.Library.Extensions;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using WilliamPersonalMultiTool.Custom;
using WilliamPersonalMultiTool.Properties;
using Win32Interop.Enums;

namespace WilliamPersonalMultiTool
{
    public partial class MainForm : Form
    {
        public CustomPhraseManager Manager { get; set; }
        public List<CustomKeySequence> StaticSequences { get; set; }
        //public WindowWorker WindowWorker { get; set; }

        public bool HideStaticSequences;

        public MainForm()
        {
            Manager = new CustomPhraseManager(this);
            InitializeComponent();

            BuildStaticSequences();

            SetupHotPhrases();
            UpdateListView();

            WindowState = Debugger.IsAttached
                ? FormWindowState.Normal
                : FormWindowState.Minimized;
        }

        private void BuildStaticSequences()
        {
            StaticSequences = new List<CustomKeySequence>()
            {
                new CustomKeySequence("Reload sequences", new List<PKey> {PKey.RControlKey, PKey.RControlKey, PKey.RShiftKey, PKey.RShiftKey}, OnReloadKeySequences, 0),
                new CustomKeySequence("Edit sequences", new List<PKey> {PKey.RControlKey, PKey.RControlKey, PKey.Alt, PKey.Alt}, OnEditKeySequences, 0),
                new CustomKeySequence("Turn off all sequences", new List<PKey> {PKey.RControlKey, PKey.Shift, PKey.Alt, PKey.RControlKey}, OnToggleOnOff, 0),
            };
            StaticSequences.ForEach(s =>
            {
                s.BackColor = Color.CadetBlue;
                s.ForeColor = Color.White;
            });
        }

        public string Encode(string plainText) 
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return "";
        }

        public string Decode(string base64EncodedData) 
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return "";
        }

        private void OnToggleOnOff(object sender, PhraseEventArguments e)
        {
            ToggleOnOffButton_Click(null, null);
        }

        /*
        private void OnEncodeClipboard(object sender, PhraseEventArguments e)
        {
            var text = Clipboard.GetText();
            if (text.IsEmpty())
                return;

            var encodedText = Encode(text);

            Manager.SendBackspaces(2);
            Manager.SendString(encodedText, 2, true);
        }

        private void OnDecodeClipboard(object sender, PhraseEventArguments e)
        {
            var encodedText = Clipboard.GetText();
            if (encodedText.IsEmpty())
                return;

            var text= Decode(encodedText);

            Manager.SendBackspaces(2);
            Manager.SendString(text, 2, true);
        }
        */

        /*
        private void OnTypeWhatsOnTheClipboard(object sender, PhraseEventArguments e)
        {
            var text = Clipboard.GetText();
            if (text.IsEmpty())
                return;
            if (text.Length > 256)
                return;

            Manager.SendString(text, 2, true);
        }
        */

        /*
        private void OnGenerateGuid_N(object sender, PhraseEventArguments e)
        {
            var text = Guid.NewGuid().ToString("N");
            Manager.SendBackspaces(2);
            Manager.SendString(text, 2, true);
        }

        private void OnGenerateGuid_P(object sender, PhraseEventArguments e)
        {
            var text = Guid.NewGuid().ToString("B");
            Manager.SendBackspaces(2);
            Manager.SendString(text, 2, true);
        }
        */

        private void OnEditKeySequences(object sender, PhraseEventArguments e)
        {
            EditButton_Click(null, null);
        }

        private void OnReloadKeySequences(object sender, PhraseEventArguments e)
        {
            ReloadButton_Click(null, null);
        }

        public void SetupHotPhrases()
        {
            var path = WpmtPath();
            
            Manager.Keyboard.KeySequences.Clear();
            
            foreach(var staticSequence in StaticSequences)
                Manager.Keyboard.AddOrReplace(staticSequence);

            Manager.AddFromFile(path);
        }

        private static string WpmtPath(string filename = "Default")
        {
            var appDataXlg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XLG");
            Directory.CreateDirectory(appDataXlg);

            var path = Path.Combine(appDataXlg, $"{filename}.wpmt");
            return path;
        }

        public Color[] NonStaticColors = new Color[] { Color.White, Color.Tan, Color.PaleGoldenrod};

        private void UpdateListView()
        {
            foreach (ListViewItem item in KeySequenceList.Items) item.Tag = null;
            KeySequenceList.Items.Clear();

            foreach (var keySequence in Manager.Keyboard.KeySequences)
            {
                var customKeySequence = (CustomKeySequence) keySequence;
                var keys = "";

                if (HideStaticSequences && NonStaticColors.All(b => b != customKeySequence.BackColor))
                    continue;

                for (var index = 0; index < customKeySequence.Sequence.Count; index++)
                {
                    var key = customKeySequence.Sequence[index];
                    var comma = index == customKeySequence.Sequence.Count - 1 ? "" : ", ";
                    keys += $"{key}{comma}";
                }

                if (customKeySequence.WildcardCount > 0)
                {
                    char matchType;
                    switch (customKeySequence.WildcardMatchType)
                    {
                        case WildcardMatchType.Anything:
                        case WildcardMatchType.AlphaNumeric:
                        case WildcardMatchType.NotAlphaNumeric:
                            matchType = '?';
                            break;

                        case WildcardMatchType.Letters:
                            matchType = '*';
                            break;

                        case WildcardMatchType.Digits:
                            matchType = '#';
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    keys += $", {new string(matchType, customKeySequence.WildcardCount)}";
                }

                var listViewItem = new ListViewItem
                {
                    Text = keys, 
                    BackColor = customKeySequence.BackColor,
                    ForeColor = customKeySequence.ForeColor,
                    Tag = customKeySequence
                };

                var listViewSubItem = new ListViewItem.ListViewSubItem(listViewItem, customKeySequence.Name);
                listViewItem.SubItems.Add(listViewSubItem);
                KeySequenceList.Items.Add(listViewItem);
            }

            KeySequenceList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public void EditButton_Click(object sender, EventArgs e)
        {
            var startInfo = new ProcessStartInfo("notepad.exe", $"\"{WpmtPath()}\"");
            var notepad = Process.Start(startInfo);
        }

        public void ReloadButton_Click(object sender, EventArgs e)
        {
            SetupHotPhrases();
            UpdateListView();
        }

        private void ToggleOnOffButton_Click(object sender, EventArgs e)
        {
            Manager.Keyboard.KeySequences.Clear();
            KeySequenceList.Items.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowPosition();
        }

        private void RestoreWindowPosition()
        {
            if (Settings.Default.HasSetDefaults)
            {
                WindowState = Settings.Default.WindowState;
                Location = Settings.Default.Location;
                Size = Settings.Default.Size;
            }
        }

        private void SaveWindowPosition()
        {
            Settings.Default.WindowState = WindowState;

            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.Location = Location;
                Settings.Default.Size = Size;
            }
            else
            {
                Settings.Default.Location = RestoreBounds.Location;
                Settings.Default.Size = RestoreBounds.Size;
            }

            Settings.Default.HasSetDefaults = true;

            Settings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RestoreWindowPosition();
        }

        private void HideStaticSequencesButton_Click(object sender, EventArgs e)
        {
            HideStaticSequences = !HideStaticSequences;
            UpdateListView();
        }

        private void KeySequenceList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
                e.Effect = DragDropEffects.Copy;
        }

        private void KeySequenceList_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Clipboard.SetText(files.AsString("\n"));
            }
            catch 
            {
                // Ignored
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.ShiftKey)
            {

            }
            else if (ModifierKeys == Keys.Alt)
            {

            }
            else if (ModifierKeys == Keys.Control)
            {

            }
            else
            {
                // Do something here
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                TopMost = !TopMost;
            }
            catch 
            {
                // Ignored
            }
        }

        private void KeySequenceList_DoubleClick(object sender, EventArgs e)
        {
            if (KeySequenceList.SelectedItems.Count == 0) return;
            var customKeySequence = (CustomKeySequence) KeySequenceList.SelectedItems[0].Tag;
            customKeySequence.Act();
        }
    }
}