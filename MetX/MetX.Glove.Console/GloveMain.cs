using System;
using System.IO;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using MetX;
using MetX.IO;
using MetX.Security;
using MetX.Data;


namespace MetX.Glove
{
    public partial class GloveMain : Form
    {
        public XlgSettings Settings;

        private static XlgAppData m_AppData;
        public static XlgAppData AppData
        {
            get
            {
                if (m_AppData == null)
                    m_AppData = XlgAppData.Load();
                return m_AppData;
            }
            set
            {
                m_AppData = value;
            }
        }

        public GloveMain()
        {
            InitializeComponent();


            if (!string.IsNullOrEmpty(AppData.LastXlgsFile))
            {
                RefreshList(AppData.LastXlgsFile);
            }
            else
                RefreshList();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                UpdateItem();
                Settings.Generate(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Enabled = true;
            }
        }

        private string ChooseFile(string Default, string ext)
        {
            openFileDialog1.FileName = Default;
            openFileDialog1.AddExtension = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "." + ext;
            openFileDialog1.Filter = "*." + ext + "|*." + ext;
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.FileName != null)
                return openFileDialog1.FileName;
            return Default;
        }

        private void buttonChooseXlgFile_Click(object sender, EventArgs e)
        {
            textXlgFile.Text = ChooseFile(textXlgFile.Text, "xlg");
        }

        private void buttonChooseXlgFileXsl_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = textAppXlgXsl.Text;
            openFileDialog1.AddExtension = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = ".xlg.xsl";
            openFileDialog1.Filter = "*.xlg.xsl|*.xlg.xsl";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.FileName != null)
                textAppXlgXsl.Text = openFileDialog1.FileName;
        }

        private void buttonChooseOutput_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = textOutput.Text;
            saveFileDialog1.AddExtension = false;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".cs";
            saveFileDialog1.Filter = "*.*|*.*|*.cs|*.cs|*.vb|*.vb|*.xml|*.xml";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != null)
                textOutput.Text = saveFileDialog1.FileName;
        }

        private void buttonRegen_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(buttonRegen_Click), new object[] { sender, e });
                return;
            }

            Enabled = false;
            try
            {
                Settings.Regenerate(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Enabled = true;
            }
        }
        private bool m_RefreshingList;
        private void RefreshList()
        {
            if (Settings != null && !string.IsNullOrEmpty(Settings.Filename))
                RefreshList(Settings.Filename);
            else
                RefreshList(null);
        }
        private void RefreshList(string xlgSourceFilename)
        {
            if (m_RefreshingList || (string.IsNullOrEmpty(xlgSourceFilename) && Settings == null))
                return;

            if (m_RefreshingList) return;
            m_RefreshingList = true;
            try
            {
                if (string.IsNullOrEmpty(xlgSourceFilename) && Settings != null)
                {
                    MetadataSources.Items.Clear();
                    foreach (XlgSource currSource in Settings.Sources)
                    {
                        ListViewItem lvi = new ListViewItem(currSource.DisplayName);
                        lvi.Tag = currSource;
                        lvi.Checked = currSource.Selected;
                        MetadataSources.Items.Add(lvi);
                    }
                }
                else if (File.Exists(xlgSourceFilename))
                {
                    Settings = XlgSettings.Load(xlgSourceFilename);
                    Settings.Filename = xlgSourceFilename;
                    Settings.Gui = this;
                    MetadataSources.Items.Clear();
                    foreach (XlgSource currSource in Settings.Sources)
                    {
                        ListViewItem lvi = new ListViewItem(currSource.DisplayName);
                        lvi.Tag = currSource;
                        lvi.Checked = currSource.Selected;
                        MetadataSources.Items.Add(lvi);
                    }
                    if (Settings.Filename != AppData.LastXlgsFile)
                    {
                        //XLG.Pipeliner.Properties.Settings.Default.LastXlgsFile = Settings.Filename;
                        //XLG.Pipeliner.Properties.Settings.Default.Save();
                        AppData.LastXlgsFile = Settings.Filename;
                        AppData.Save();
                    }

                    if (MetadataSources.Items.Count > 0)
                        MetadataSources.Items[MetadataSources.Items.Count - 1].Selected = true;
                }
                else
                    MessageBox.Show(xlgSourceFilename + " does not exist.");
            }
            finally { m_RefreshingList = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string toParse = Clipboard.GetText();
            StringBuilder sb = new StringBuilder();
            string[] lines = toParse.Replace("\r", string.Empty).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string currLine in lines)
            {
                if (currLine.IndexOf(" = ", StringComparison.Ordinal) > -1)
                {
                    string s = Token.First(currLine, ";");
                    string b = Token.First(s, " = ");
                    int spaces = b.Length - b.Trim().Length;
                    sb.AppendLine(new string(' ', spaces) + Token.After(s, 1, " = ") + " = " + b.Trim() + ";");
                }
                else
                    sb.AppendLine(currLine);
            }
            Clipboard.Clear();
            Clipboard.SetText(sb.ToString());
        }

        private void GloveMain_Load(object sender, EventArgs e)
        {
            MetadataSources.Focus();
            if (MetadataSources.Items.Count > 0)
                MetadataSources.SelectedIndices.Add(0);
        }

        private void GloveMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        bool m_AutoGenActive = false;
        readonly FileSystemWatchers m_FSWs = new FileSystemWatchers();
        private object m_SyncRoot = new object();


        private void buttonAutoGen_Click(object sender, EventArgs e)
        {
            lock (m_SyncRoot)
            {
                if (m_FSWs.IsActive)
                {
                    m_FSWs.End();
                    autoRegenToolbarButton.Image = global::XLG.Pipeliner.Properties.Resources.circle_blue;
                    autoRegenOnChangedXSLToolStripMenuItem.Image = global::XLG.Pipeliner.Properties.Resources.circle_blue;
                }
                else
                {
                    m_FSWs.Begin(Settings, FSW_Changed, FSW_Error);
                    autoRegenToolbarButton.Image = global::XLG.Pipeliner.Properties.Resources.circle_green;
                    autoRegenOnChangedXSLToolStripMenuItem.Image = global::XLG.Pipeliner.Properties.Resources.circle_green;
                }
                toolStrip1.Invalidate();
                toolStrip1.Refresh();
            }
        }

        void FSW_Error(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.GetException().ToString());
        }

        void FSW_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(".xlg.xsl") > 0)
            {
                try
                {
                    lock (m_SyncRoot)
                    {
                        if (m_AutoGenActive)
                            return;
                        m_AutoGenActive = true;
                    }
                    Invoke(new MethodInvoker(SynchAutoRegen));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    m_AutoGenActive = false;
                    m_FSWs.EnableRaisingEvents = true;
                }
            }
        }

        void SynchAutoRegen()
        {
            autoRegenToolbarButton.Image = global::XLG.Pipeliner.Properties.Resources.circle_orange;
            autoRegenOnChangedXSLToolStripMenuItem.Image = global::XLG.Pipeliner.Properties.Resources.circle_orange;
            m_FSWs.EnableRaisingEvents = false;
            buttonRegen_Click(null, null);
            autoRegenToolbarButton.Image = global::XLG.Pipeliner.Properties.Resources.circle_green;
            autoRegenOnChangedXSLToolStripMenuItem.Image = global::XLG.Pipeliner.Properties.Resources.circle_green;
        }

        private void MetadataSources_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ((XlgSource)e.Item.Tag).Selected = e.Item.Checked;
        }

        XlgSource m_CurrSource = null;

        private void MetadataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateItem();
            if (MetadataSources.SelectedItems.Count > 0)
            {
                m_CurrSource = (XlgSource)MetadataSources.SelectedItems[0].Tag;
                textAppXlgXsl.Text = m_CurrSource.XslFilename;
                textOutput.Text = m_CurrSource.OutputFilename;
                textOutputXml.Text = m_CurrSource.OutputXml;
                textXlgFile.Text = m_CurrSource.XlgDocFilename;
                textConnectionName.Text = m_CurrSource.DisplayName;
                textConnectionStringName.Text = m_CurrSource.ConnectionName;
                textConnectionString.Text = m_CurrSource.ConnectionString;
                textSqlToXml.Text = m_CurrSource.SqlToXml;
                checkRegenerateOnly.Checked = m_CurrSource.RegenerateOnly;
                int index = comboProviderName.FindString(m_CurrSource.ProviderName);
                if (index > -1)
                    comboProviderName.SelectedIndex = index;
            }
        }

        public void UpdateItem()
        {
            if (m_CurrSource != null)
            {
                m_CurrSource.XslFilename = textAppXlgXsl.Text;
                m_CurrSource.OutputFilename = textOutput.Text;
                m_CurrSource.OutputXml = textOutputXml.Text;
                m_CurrSource.XlgDocFilename = textXlgFile.Text;
                m_CurrSource.DisplayName = textConnectionName.Text;
                m_CurrSource.ConnectionName = textConnectionStringName.Text;
                m_CurrSource.ConnectionString = textConnectionString.Text;
                m_CurrSource.RegenerateOnly = checkRegenerateOnly.Checked;
                m_CurrSource.SqlToXml = textSqlToXml.Text;
                if (comboProviderName.SelectedIndex > -1)
                    m_CurrSource.ProviderName = (string)comboProviderName.SelectedItem;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            UpdateItem();
            Settings.Save();
            //XLG.Pipeliner.Properties.Settings.Default.LastXlgsFile = Settings.Filename;
            //XLG.Pipeliner.Properties.Settings.Default.Save();
            AppData.LastXlgsFile = Settings.Filename;
            AppData.Save();
            if (sender != null)
                RefreshList();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = (Settings != null && !string.IsNullOrEmpty(Settings.Filename) ? Settings.Filename : string.Empty);
            if (string.IsNullOrEmpty(openFileDialog1.FileName) && Directory.Exists(AppData.SupportPath))
                openFileDialog1.InitialDirectory = AppData.SupportPath;
            openFileDialog1.AddExtension = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = ".xlgs";
            openFileDialog1.Filter = "*.xlgs|*.xlgs";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.FileName != null)
            {
                RefreshList(openFileDialog1.FileName);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string itemName = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the database you wish to walk?", "DATABASE NAME", "CLONE", -1, -1);
            if (!string.IsNullOrEmpty(itemName))
            {
                if (m_CurrSource == null && Settings.Sources.Count > 0)
                    m_CurrSource = Settings.Sources[0];

                XlgSource newSource = null;
                if (itemName == "CLONE")
                {
                    newSource = xml.FromXml<XlgSource>(xml.ToXml<XlgSource>(m_CurrSource, false));
                }
                else
                {
                    newSource = new XlgSource(AppData.BasePath, AppData.ParentNamespace, Token.Last((m_CurrSource != null ? m_CurrSource.ProviderName : (Settings.Sources.Count + 1).ToString()), ".") + ": " + itemName, itemName);

                    if (m_CurrSource != null)
                        newSource.BasePath = Token.Before(m_CurrSource.OutputFilename, Token.Count(m_CurrSource.OutputFilename, @"\") - 1, @"\") + @"\" + itemName + @"\";
                    else
                        newSource.BasePath = AppData.BasePath + itemName + @"\";
                    IO.FileSystem.InsureFolderExists(newSource.BasePath, false);
                    // NewSource.ConfigFilename = CurrSource.ConfigFilename;

                    if (m_CurrSource != null)
                    {
                        newSource.ConnectionString = m_CurrSource.ConnectionString;
                        newSource.OutputFilename = newSource.BasePath + itemName + ".Glove." + Path.GetExtension(m_CurrSource.OutputFilename);
                        newSource.OutputXml = newSource.BasePath + itemName + ".Glove.xml";
                        newSource.ProviderName = m_CurrSource.ProviderName;
                        newSource.Selected = true;
                        newSource.XlgDocFilename = newSource.BasePath + itemName + ".xlgd";
                        newSource.XslFilename = m_CurrSource.XslFilename;
                    }
                    else
                    {
                        newSource.OutputFilename = newSource.BasePath + itemName + ".Glove.cs";
                        newSource.OutputXml = newSource.BasePath + itemName + ".Glove.xml";
                        newSource.ProviderName = "System.Data.SqlClient";
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
                MetadataSources.Items[MetadataSources.Items.Count - 1].Selected = true;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (m_CurrSource != null)
            {
                if (MessageBox.Show("Are you sure you want to remove the current step?\r\n\tThere is no undo for this action.", "REMOVE STEP", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Settings.Sources.Remove(m_CurrSource);
                    m_CurrSource = null;
                    buttonSave_Click(null, null);
                    RefreshList();
                }
            }
        }

        private void buttonEditXlgFile_Click(object sender, EventArgs e)
        {
            IO.FileSystem.InsureFolderExists(textXlgFile.Text, true);
            if (!File.Exists(textXlgFile.Text))
            {
                File.WriteAllText(textXlgFile.Text, MetX.Data.DefaultXlg.xml.Replace("[Default]", textConnectionStringName.Text));
            }
            Process.Start(AppData.TextEditor, textXlgFile.Text);
        }

        private void buttonEditXlgXslFile_Click(object sender, EventArgs e)
        {
            IO.FileSystem.InsureFolderExists(textAppXlgXsl.Text, true);
            Process.Start(AppData.TextEditor, textAppXlgXsl.Text);
        }

        /*
        private void buttonEditConfigFile_Click(object sender, EventArgs e)
        {
            IO.FileSystem.InsureFolderExists(textSettings.Text, true);
            Process.Start(AppData.TextEditor, textSettings.Text);
        }
        */

        private void buttonViewOutputFolder_Click(object sender, EventArgs e)
        {
            IO.FileSystem.InsureFolderExists(textOutput.Text, true);
            Process.Start(AppData.TextEditor, textOutput.Text);
        }

        private void buttonViewtextOutputXml_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textOutputXml.Text))
                textOutputXml.Text = textOutput.Text.Substring(0, textOutput.Text.Length - Path.GetExtension(textOutput.Text).Length) + ".xml";
            IO.FileSystem.InsureFolderExists(textOutputXml.Text, true);
            Process.Start(AppData.TextEditor, textOutputXml.Text);
        }

        private void buttonChoosetextOutputXml_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = textOutputXml.Text;
            saveFileDialog1.AddExtension = false;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xml";
            saveFileDialog1.Filter = "*.*|*.*|*.xml|*.xml";
            saveFileDialog1.ShowDialog(this);
            if (saveFileDialog1.FileName != null)
                textOutputXml.Text = saveFileDialog1.FileName;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XlgSettings t = Settings;
            Settings = new XlgSettings(this);
            Settings.Sources = new List<XlgSource>();
            Settings.DefaultConnectionString = t.DefaultConnectionString;
            Settings.DefaultProviderName = t.DefaultProviderName;

            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xlgs";
            saveFileDialog1.Filter = "*.xlgs|*.xlgs";
            saveFileDialog1.ShowDialog(this);
            if (!string.IsNullOrEmpty(saveFileDialog1.FileName) && !File.Exists(saveFileDialog1.FileName))
            {
                Settings.Filename = saveFileDialog1.FileName;
                buttonSave_Click(null, null);
                RefreshList();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Settings.Filename;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = ".xlgs";
            saveFileDialog1.Filter = "*.xlgs|*.xlgs";
            saveFileDialog1.ShowDialog(this);
            if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                Settings.Filename = saveFileDialog1.FileName;
                buttonSave_Click(null, null);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonSave_Click(null, null);
            Close();
        }

        private void generateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void regenerateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonEditConnectionString_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                object[] args = new object[] { };
                System.Type linkType = System.Type.GetTypeFromProgID("DataLinks");
                object linkObj = Activator.CreateInstance(linkType);
                object connObj = linkType.InvokeMember("PromptNew", System.Reflection.BindingFlags.InvokeMethod, null, linkObj, args);
                if(connObj != null)
                {
                    string connString = connObj.GetType().InvokeMember("ConnectionString", System.Reflection.BindingFlags.GetProperty, null, connObj, args).ToString();
                    if(!string.IsNullOrEmpty(connString))
                    {
                        textConnectionString.Text = connString;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            DataConnectionDialog dcd = null;
            try
            {
                dcd = new DataConnectionDialog();
                DataSource.AddStandardDataSources(dcd);
                /*
                if (!string.IsNullOrEmpty(textConnectionString.Text))
                {
                    dcd.ConnectionString = textConnectionString.Text;
                    dcd.SelectedDataProvider = Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider;
                }
                * /
                if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
                {
                    textConnectionString.Text = dcd.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (dcd != null)
                    dcd.Dispose();
            }
            */

        }

        private void loadStripButton_Click(object sender, EventArgs e)
        {
            buttonLoad_Click(null, null);
        }

        private void scratchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


    }

    internal class FileSystemWatchers : List<FileSystemWatcher>
    {
        public bool IsActive = false;

        public bool EnableRaisingEvents
        {
            set
            {
                if (Count <= 0) { return; }
                foreach (FileSystemWatcher fsw in this)
                    fsw.EnableRaisingEvents = value;
            }
        }

        public void Begin(XlgSettings settings, FileSystemEventHandler onchange, ErrorEventHandler onerror)
        {
            List<string> directories = new List<string> { GloveMain.AppData.SupportPath };
            AddIfDifferent(directories, Path.GetDirectoryName(settings.Filename));           
            foreach (XlgSource setting in settings.Sources)
            {
                AddIfDifferent(directories, setting.BasePath);
                AddIfDifferent(directories, Path.GetDirectoryName(setting.OutputFilename));
            }
            
            foreach (string directory in directories)
            {
                FileSystemWatcher fsw = new FileSystemWatcher(directory);
                fsw.Changed += onchange;
                fsw.Created += onchange;
                fsw.Deleted += onchange;
                fsw.Error += onerror;
                fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                fsw.EnableRaisingEvents = true;
                this.Add(fsw);
            }
            IsActive = true;
        }

        private static void AddIfDifferent(List<string> directories, string currDir)
        {
            currDir = Worker.nzString(currDir).ToLower();
            if (!currDir.EndsWith(@"\"))
                currDir += @"\";
            if (!directories.Contains(currDir) && Directory.Exists(currDir))
                directories.Add(currDir);
        }

        public void End()
        {
            if (!IsActive) { return; }
            foreach (FileSystemWatcher watcher in this)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            IsActive = false;
            Clear();
        }
    }
}
