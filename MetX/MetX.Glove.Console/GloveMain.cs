using MetX.Fimm.Glove;
using MetX.Fimm.Glove.Pipelines;
using MetX.Standard.Library.ML;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;
using MetX.Standard.Primary.IO;
using MetX.Standard.Primary.Pipelines;
using MetX.Standard.Strings;
using MetX.Windows.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;
using NHotPhrase.WindowsForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using XLG.Pipeliner.Properties;

namespace XLG.Pipeliner;

public partial class GloveMain : Form
{
    private static XlgAppData _mAppData;

    private readonly FileSystemWatchers _fileSystemWatchers = new();
    private readonly object _mSyncRoot = new();
    private bool _mAutoGenActive;
    private bool _mRefreshingList;
    private XlgSource _xlgSource;
    public XlgSettings Settings;

    public GloveMain()
    {
        InitializeComponent();

        Host = new WinFormGenerationHost<GloveMain>(this, Clipboard.GetText);

        if (!string.IsNullOrEmpty(AppData.LastXlgsFile))
            RefreshList(AppData.LastXlgsFile);
        else
            RefreshList();

        InitializeHotKeys();
    }

    public static XlgAppData AppData => _mAppData ??= XlgAppData.Load();

    public IGenerationHost Host { get; set; }

    public HotPhraseManagerForWinForms Manager { get; set; } = new();

    private void InitializeHotKeys()
    {
        Manager.Keyboard.AddOrReplace("RegenerateNow",
            new List<PKey> { PKey.RControlKey, PKey.LControlKey, PKey.RControlKey, PKey.RControlKey },
            OnHotKeyRegenerateNow);
    }

    private void OnHotKeyRegenerateNow(object sender, PhraseEventArguments e)
    {
        try
        {
            SynchAutoRegenerate();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        e.Handled = true;
    }

    private void buttonGo_Click(object sender, EventArgs e)
    {
        Enabled = false;
        try
        {
            UpdateCurrentSource();
            Settings.Generate(Host);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
        finally
        {
            Enabled = true;
        }
    }

    private string ChooseFile(string @default, string ext)
    {
        openFileDialog1.FileName = @default;
        openFileDialog1.AddExtension = true;
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.CheckPathExists = true;
        openFileDialog1.DefaultExt = "." + ext;
        openFileDialog1.Filter = "*." + ext + "|*." + ext;
        openFileDialog1.Multiselect = false;
        openFileDialog1.ShowDialog(this);
        if (openFileDialog1.FileName != null) return openFileDialog1.FileName;
        return @default;
    }

    private void buttonChooseXlgFile_Click(object sender, EventArgs e)
    {
        try
        {
            textXlgFile.Text = ChooseFile(textXlgFile.Text, "xlg");
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonChooseXlgFileXsl_Click(object sender, EventArgs e)
    {
        try
        {
            openFileDialog1.FileName = textAppXlgXsl.Text;
            openFileDialog1.AddExtension = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = ".xlg.xsl";
            openFileDialog1.Filter = "*.xlg.xsl|*.xlg.xsl";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.FileName != null) textAppXlgXsl.Text = openFileDialog1.FileName;
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonChooseOutput_Click(object sender, EventArgs e)
    {
        try
        {
            saveFileDialog1.FileName = textOutput.Text;
            saveFileDialog1.AddExtension = false;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".cs";
            saveFileDialog1.Filter = "*.*|*.*|*.cs|*.cs|*.vb|*.vb|*.xml|*.xml";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != null) textOutput.Text = saveFileDialog1.FileName;
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonRegenerate_Click(object sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(new EventHandler(buttonRegenerate_Click), sender, e);
            return;
        }

        Enabled = false;
        try
        {
            Settings.Regenerate(Host);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
        finally
        {
            Enabled = true;
        }
    }

    private void RefreshList()
    {
        if (Settings != null && !string.IsNullOrEmpty(Settings.Filename))
        {
            if (File.Exists(Settings.Filename))
                RefreshList(Settings.Filename);
            else
                Settings.Filename = null;
            return;
        }

        RefreshList(null);
    }

    private void RefreshList(string xlgSourceFilename)
    {
        if (_mRefreshingList || string.IsNullOrEmpty(xlgSourceFilename) && Settings == null) return;

        if (_mRefreshingList) return;

        _mRefreshingList = true;
        try
        {
            if (string.IsNullOrEmpty(xlgSourceFilename) && Settings != null)
            {
                MetadataSources.Items.Clear();
                foreach (var currSource in Settings.Sources)
                {
                    var lvi = new ListViewItem(currSource.DisplayName)
                    { Tag = currSource, Checked = currSource.Selected };
                    MetadataSources.Items.Add(lvi);
                }
            }
            else if (File.Exists(xlgSourceFilename))
            {
                Settings = XlgSettings.Load(xlgSourceFilename);
                Settings.Filename = xlgSourceFilename;
                Settings.Gui = Host;
                MetadataSources.Items.Clear();
                foreach (var currSource in Settings.Sources)
                {
                    var lvi = new ListViewItem(currSource.DisplayName)
                    { Tag = currSource, Checked = currSource.Selected };
                    MetadataSources.Items.Add(lvi);
                }

                if (Settings.Filename != AppData.LastXlgsFile)
                {
                    AppData.LastXlgsFile = Settings.Filename;
                    AppData.CheckIfTextEditorExistsAndAskIfNot(Host);
                    AppData.Save();
                }

                if (MetadataSources.Items.Count > 0) MetadataSources.Items[^1].Selected = true;
            }
            else
            {
                var appDataXlg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "XLG");
                if (!Directory.Exists(appDataXlg))
                    Directory.CreateDirectory(appDataXlg);

                Settings = new XlgSettings(Host)
                {
                    Filename = Path.Combine(appDataXlg, "Default.xlgs"),
                    Gui = Host
                };
                AppData.LastXlgsFile = Settings.Filename;
                AppData.Save();
            }
        }
        finally
        {
            _mRefreshingList = false;
        }
    }

    private void GloveMain_Load(object sender, EventArgs e)
    {
        MetadataSources.Focus();
        if (MetadataSources.Items.Count > 0) MetadataSources.SelectedIndices.Add(0);
    }

    private void GloveMain_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void buttonAutoGen_Click(object sender, EventArgs e)
    {
        lock (_mSyncRoot)
        {
            if (_fileSystemWatchers.IsActive)
            {
                _fileSystemWatchers.End();
                autoRegenToolbarButton.Image = Resources.circle_blue;
                autoRegenOnChangedXSLToolStripMenuItem.Image = Resources.circle_blue;
            }
            else
            {
                _fileSystemWatchers.Begin(Settings, FSW_Changed, FSW_Error);
                autoRegenToolbarButton.Image = Resources.circle_green;
                autoRegenOnChangedXSLToolStripMenuItem.Image = Resources.circle_green;
            }

            toolStrip1.Invalidate();
            toolStrip1.Refresh();
        }
    }

    private void FSW_Error(object sender, ErrorEventArgs e)
    {
        Host.MessageBox.Show(e.GetException().ToString());
    }

    private void FSW_Changed(object sender, FileSystemEventArgs e)
    {
        if (e.FullPath.IndexOf(".xlg.xsl", StringComparison.Ordinal) <= 0) return;

        if (Monitor.TryEnter(_mSyncRoot))
            try
            {
                if (_mAutoGenActive) return;

                _mAutoGenActive = true;
                Invoke(new MethodInvoker(SynchAutoRegenerate));
            }
            catch (Exception ex)
            {
                Host.MessageBox.Show(ex.ToString());
            }
            finally
            {
                _mAutoGenActive = false;
                _fileSystemWatchers.EnableRaisingEvents = true;
                Monitor.Exit(_mSyncRoot);
            }
    }

    private void SynchAutoRegenerate()
    {
        autoRegenToolbarButton.Image = Resources.circle_orange;
        autoRegenOnChangedXSLToolStripMenuItem.Image = Resources.circle_orange;
        _fileSystemWatchers.EnableRaisingEvents = false;
        buttonRegenerate_Click(null, null);
        autoRegenToolbarButton.Image = Resources.circle_green;
        autoRegenOnChangedXSLToolStripMenuItem.Image = Resources.circle_green;
    }

    private void MetadataSources_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
        ((XlgSource)e.Item.Tag).Selected = e.Item.Checked;
    }

    private void MetadataSources_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            UpdateCurrentSource();
            if (MetadataSources.SelectedItems.Count <= 0) return;
            _xlgSource = (XlgSource)MetadataSources.SelectedItems[0].Tag;
            textAppXlgXsl.Text = _xlgSource.XslFilename;
            textOutput.Text = _xlgSource.OutputFilename;
            textOutputXml.Text = _xlgSource.OutputXml;
            textXlgFile.Text = _xlgSource.XlgDocFilename;
            textConnectionName.Text = _xlgSource.DisplayName;
            textConnectionStringName.Text = _xlgSource.ConnectionName;
            textConnectionString.Text = _xlgSource.ConnectionString;
            //textSqlToXml.Text = m_CurrSource.SqlToXml;
            checkRegenerateOnly.Checked = _xlgSource.RegenerateOnly;
            var index = comboProviderName.FindString(_xlgSource.ProviderName);
            if (index > -1) comboProviderName.SelectedIndex = index;
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    public void UpdateCurrentSource()
    {
        if (_xlgSource == null) return;
        _xlgSource.XslFilename = textAppXlgXsl.Text;
        _xlgSource.OutputFilename = textOutput.Text;
        _xlgSource.OutputXml = textOutputXml.Text;
        _xlgSource.XlgDocFilename = textXlgFile.Text;
        _xlgSource.DisplayName = textConnectionName.Text;
        _xlgSource.ConnectionName = textConnectionStringName.Text;
        _xlgSource.ConnectionString = textConnectionString.Text;
        _xlgSource.RegenerateOnly = checkRegenerateOnly.Checked;
        //            m_CurrSource.SqlToXml = textSqlToXml.Text;
        if (comboProviderName.SelectedIndex > -1) _xlgSource.ProviderName = (string)comboProviderName.SelectedItem;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateCurrentSource();
            Settings.Save();
            AppData.LastXlgsFile = Settings.Filename;
            AppData.Save();
            if (sender != null) RefreshList();
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonLoad_Click(object sender, EventArgs e)
    {
        try
        {
            openFileDialog1.FileName = Settings != null && !string.IsNullOrEmpty(Settings.Filename)
                ? Settings.Filename
                : string.Empty;
            if (string.IsNullOrEmpty(openFileDialog1.FileName) && Directory.Exists(AppData.SupportPath))
                openFileDialog1.InitialDirectory = AppData.SupportPath;
            openFileDialog1.AddExtension = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = ".xlgs";
            openFileDialog1.Filter = "*.xlgs|*.xlgs";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.FileName != null) RefreshList(openFileDialog1.FileName);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
        try
        {
            var itemName = "CLONE";

            if (Host.InputBox("DATABASE NAME", "What is the name of the database you wish to walk?", ref itemName)
                == MessageBoxResult.Cancel
                || string.IsNullOrEmpty(itemName))
                return;

            if (_xlgSource == null && Settings.Sources.Count > 0) _xlgSource = Settings.Sources[0];

            XlgSource newSource;
            if (itemName == "CLONE")
            {
                newSource = Xml.FromXml<XlgSource>(Xml.ToXml(_xlgSource, false));
            }
            else
            {
                newSource = new XlgSource(AppData.BasePath, AppData.ParentNamespace,
                    (_xlgSource != null
                        ? _xlgSource.ProviderName
                        : (Settings.Sources.Count + 1).ToString()).LastToken(".") + ": " + itemName, itemName);

                if (_xlgSource != null)
                    newSource.BasePath =
                        _xlgSource.OutputFilename.TokensBefore(_xlgSource.OutputFilename.TokenCount(@"\") - 1,
                            @"\") + @"\" + itemName
                        + @"\";
                else
                    newSource.BasePath = AppData.BasePath + itemName + @"\";
                FileSystem.InsureFolderExists(Host, newSource.BasePath, false);
                // NewSource.ConfigFilename = CurrSource.ConfigFilename;

                if (_xlgSource != null)
                {
                    newSource.ConnectionString = _xlgSource.ConnectionString;
                    newSource.OutputFilename = newSource.BasePath + itemName + ".Glove."
                                               + Path.GetExtension(_xlgSource.OutputFilename);
                    newSource.OutputXml = newSource.BasePath + itemName + ".Glove.xml";
                    newSource.ProviderName = _xlgSource.ProviderName;
                    newSource.Selected = true;
                    newSource.XlgDocFilename = newSource.BasePath + itemName + ".xlgd";
                    newSource.XslFilename = _xlgSource.XslFilename;
                }
                else
                {
                    newSource.OutputFilename = newSource.BasePath + itemName + ".Glove.cs";
                    newSource.OutputXml = newSource.BasePath + itemName + ".Glove.xml";
                    newSource.ProviderName = "Microsoft.Data.SqlClient";
                    newSource.Selected = true;
                    newSource.XlgDocFilename = newSource.BasePath + itemName + ".xlgd";
                    newSource.XslFilename = AppData.SupportPath + "CSharp DAL.xlg.xsl";
                }
            }

            //if (ItemName != "CLONE")
            //    foreach (xlgSource CurrItem in Settings.Sources)
            //        CurrItem.Selected = false;

            newSource.Selected = true;
            Settings.Sources.Add(newSource);
            buttonSave_Click(null, null);
            RefreshList();
            MetadataSources.Items[^1].Selected = true;
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (_xlgSource == null) return;
            var messageBoxResult =
                Host.MessageBox.Show(
                    "Are you sure you want to remove the current step?\r\n\tThere is no undo for this action.",
                    "REMOVE STEP", MessageBoxChoices.YesNo);
            if (messageBoxResult == MessageBoxResult.No) return;
            Settings.Sources.Remove(_xlgSource);
            _xlgSource = null;
            buttonSave_Click(null, null);
            RefreshList();
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonEditXlgFile_Click(object sender, EventArgs e)
    {
        try
        {
            FileSystem.InsureFolderExists(Host, textXlgFile.Text, true);
            if (!File.Exists(textXlgFile.Text))
                File.WriteAllText(textXlgFile.Text,
                    DefaultXlg.Xml.Replace("[Default]", textConnectionStringName.Text));

            Process.Start(AppData.CheckIfTextEditorExistsAndAskIfNot(Host), textXlgFile.Text);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonEditXlgXslFile_Click(object sender, EventArgs e)
    {
        try
        {
            FileSystem.InsureFolderExists(Host, textAppXlgXsl.Text, true);
            Process.Start(AppData.CheckIfTextEditorExistsAndAskIfNot(Host), textAppXlgXsl.Text);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonViewOutputFolder_Click(object sender, EventArgs e)
    {
        try
        {
            FileSystem.InsureFolderExists(Host, textOutput.Text, true);
            Process.Start(AppData.CheckIfTextEditorExistsAndAskIfNot(Host), textOutput.Text);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonViewtextOutputXml_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(textOutputXml.Text))
                textOutputXml.Text = textOutput.Text.Substring(0,
                    textOutput.Text.Length - Path.GetExtension(textOutput?.Text ?? "").Length) + ".xml";
            FileSystem.InsureFolderExists(Host, textOutputXml.Text, true);
            Process.Start(AppData.CheckIfTextEditorExistsAndAskIfNot(Host), textOutputXml.Text);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void buttonChooseTextOutputXml_Click(object sender, EventArgs e)
    {
        try
        {
            saveFileDialog1.FileName = textOutputXml.Text;
            saveFileDialog1.AddExtension = false;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xml";
            saveFileDialog1.Filter = "*.*|*.*|*.xml|*.xml";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != null) textOutputXml.Text = saveFileDialog1.FileName;
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void NewScriptFile_Click(object sender, EventArgs e)
    {
        try
        {
            var t = Settings;
            Settings = new XlgSettings(Host)
            {
                Sources = new List<XlgSource>(),
                DefaultConnectionString = t.DefaultConnectionString,
                DefaultProviderName = t.DefaultProviderName
            };

            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xlgs";
            saveFileDialog1.Filter = "*.xlgs|*.xlgs";
            saveFileDialog1.ShowDialog(this);
            if (string.IsNullOrEmpty(saveFileDialog1.FileName) || File.Exists(saveFileDialog1.FileName)) return;
            Settings.Filename = saveFileDialog1.FileName;
            buttonSave_Click(null, null);
            RefreshList();
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (Settings != null) saveFileDialog1.FileName = Settings.Filename;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xlgs";
            saveFileDialog1.Filter = "*.xlgs|*.xlgs";
            saveFileDialog1.ShowDialog(this);
            if (string.IsNullOrEmpty(saveFileDialog1.FileName)) return;

            Settings ??= new XlgSettings(Host);
            Settings.Filename = saveFileDialog1.FileName;

            buttonSave_Click(null, null);
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            buttonSave_Click(null, null);
            Close();
        }
        catch (Exception ex)
        {
            Host.MessageBox.Show(ex.ToString());
        }
    }

    private void generateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void regenerateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void buttonEditConnectionString_Click(object sender, EventArgs e)
    {
    }

    private void EditQuickScript_Click(object sender, EventArgs e)
    {
        var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xlgQuickScripts.exe");
        exePath = FileSystem.FindExecutableAlongPath(exePath,
            new[] { @"\..\..\..\..\MetX.QuickScripts\bin\Debug\net6.0-windows" });
        if (!File.Exists(exePath))
            Host.MessageBox.Show("Qk Scrptr executable missing: " + exePath);
        else
            Process.Start(exePath, string.Empty);
    }
}