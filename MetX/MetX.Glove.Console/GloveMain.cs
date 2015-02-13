using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MetX.Data;
using MetX.IO;
using MetX.Library;
using XLG.Pipeliner.Properties;

namespace XLG.Pipeliner
{
    public partial class GloveMain : Form
    {
        public static string ClipScriptProcessorSourceTemplate = null;
        private static XlgAppData m_AppData;

        public static XlgAppData AppData
        {
            get
            {
                if (m_AppData == null)
                {
                    m_AppData = XlgAppData.Load();
                }
                return m_AppData;
            }
            set { m_AppData = value; }
        }

        private readonly FileSystemWatchers m_FSWs = new FileSystemWatchers();
        private readonly object m_SyncRoot = new object();
        private bool m_AutoGenActive = false;
        private XlgSource m_CurrSource = null;
        private bool m_RefreshingList;
        public XlgSettings Settings;

        public GloveMain()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(AppData.LastXlgsFile))
            {
                RefreshList(AppData.LastXlgsFile);
            }
            else
            {
                RefreshList();
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                UpdateCurrentSource();
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
            {
                return openFileDialog1.FileName;
            }
            return Default;
        }

        private void buttonChooseXlgFile_Click(object sender, EventArgs e)
        {
            try
            {
                textXlgFile.Text = ChooseFile(textXlgFile.Text, "xlg");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                if (openFileDialog1.FileName != null)
                {
                    textAppXlgXsl.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                if (saveFileDialog1.FileName != null)
                {
                    textOutput.Text = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonRegen_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(buttonRegen_Click), sender, e);
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

        private void RefreshList()
        {
            if (Settings != null && !string.IsNullOrEmpty(Settings.Filename))
            {
                if (File.Exists(Settings.Filename))
                {
                    RefreshList(Settings.Filename);
                }
                else
                {
                    Settings.Filename = null;
                }
                return;
            }
            RefreshList(null);
        }

        private void RefreshList(string xlgSourceFilename)
        {
            if (m_RefreshingList || (string.IsNullOrEmpty(xlgSourceFilename) && Settings == null))
            {
                return;
            }

            if (m_RefreshingList)
            {
                return;
            }

            m_RefreshingList = true;
            try
            {
                if (string.IsNullOrEmpty(xlgSourceFilename) && Settings != null)
                {
                    MetadataSources.Items.Clear();
                    foreach (XlgSource currSource in Settings.Sources)
                    {
                        ListViewItem lvi = new ListViewItem(currSource.DisplayName)
                        {Tag = currSource, Checked = currSource.Selected};
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
                        ListViewItem lvi = new ListViewItem(currSource.DisplayName)
                        {Tag = currSource, Checked = currSource.Selected};
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
                    {
                        MetadataSources.Items[MetadataSources.Items.Count - 1].Selected = true;
                    }
                }
                else
                {
                    MessageBox.Show(xlgSourceFilename + " does not exist.");
                }
            }
            finally
            {
                m_RefreshingList = false;
            }
        }

        private void GloveMain_Load(object sender, EventArgs e)
        {
            MetadataSources.Focus();
            if (MetadataSources.Items.Count > 0)
            {
                MetadataSources.SelectedIndices.Add(0);
            }
        }

        private void GloveMain_FormClosing(object sender, FormClosingEventArgs e) { }

        private void buttonAutoGen_Click(object sender, EventArgs e)
        {
            lock (m_SyncRoot)
            {
                if (m_FSWs.IsActive)
                {
                    m_FSWs.End();
                    autoRegenToolbarButton.Image = MetX.Properties.Resources.circle_blue;
                    autoRegenOnChangedXSLToolStripMenuItem.Image = MetX.Properties.Resources.circle_blue;
                }
                else
                {
                    m_FSWs.Begin(Settings, FSW_Changed, FSW_Error);
                    autoRegenToolbarButton.Image = MetX.Properties.Resources.circle_green;
                    autoRegenOnChangedXSLToolStripMenuItem.Image = MetX.Properties.Resources.circle_green;
                }
                toolStrip1.Invalidate();
                toolStrip1.Refresh();
            }
        }

        private void FSW_Error(object sender, ErrorEventArgs e) { MessageBox.Show(e.GetException().ToString()); }

        private void FSW_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(".xlg.xsl", StringComparison.Ordinal) <= 0)
            {
                return;
            }
            try
            {
                lock (m_SyncRoot)
                {
                    if (m_AutoGenActive)
                    {
                        return;
                    }
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

        private void SynchAutoRegen()
        {
            autoRegenToolbarButton.Image = MetX.Properties.Resources.circle_orange;
            autoRegenOnChangedXSLToolStripMenuItem.Image = MetX.Properties.Resources.circle_orange;
            m_FSWs.EnableRaisingEvents = false;
            buttonRegen_Click(null, null);
            autoRegenToolbarButton.Image = MetX.Properties.Resources.circle_green;
            autoRegenOnChangedXSLToolStripMenuItem.Image = MetX.Properties.Resources.circle_green;
        }

        private void MetadataSources_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ((XlgSource) e.Item.Tag).Selected = e.Item.Checked;
        }

        private void MetadataSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateCurrentSource();
                if (MetadataSources.SelectedItems.Count <= 0)
                {
                    return;
                }
                m_CurrSource = (XlgSource) MetadataSources.SelectedItems[0].Tag;
                textAppXlgXsl.Text = m_CurrSource.XslFilename;
                textOutput.Text = m_CurrSource.OutputFilename;
                textOutputXml.Text = m_CurrSource.OutputXml;
                textXlgFile.Text = m_CurrSource.XlgDocFilename;
                textConnectionName.Text = m_CurrSource.DisplayName;
                textConnectionStringName.Text = m_CurrSource.ConnectionName;
                textConnectionString.Text = m_CurrSource.ConnectionString;
                //textSqlToXml.Text = m_CurrSource.SqlToXml;
                checkRegenerateOnly.Checked = m_CurrSource.RegenerateOnly;
                int index = comboProviderName.FindString(m_CurrSource.ProviderName);
                if (index > -1)
                {
                    comboProviderName.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateCurrentSource()
        {
            if (m_CurrSource == null)
            {
                return;
            }
            m_CurrSource.XslFilename = textAppXlgXsl.Text;
            m_CurrSource.OutputFilename = textOutput.Text;
            m_CurrSource.OutputXml = textOutputXml.Text;
            m_CurrSource.XlgDocFilename = textXlgFile.Text;
            m_CurrSource.DisplayName = textConnectionName.Text;
            m_CurrSource.ConnectionName = textConnectionStringName.Text;
            m_CurrSource.ConnectionString = textConnectionString.Text;
            m_CurrSource.RegenerateOnly = checkRegenerateOnly.Checked;
//            m_CurrSource.SqlToXml = textSqlToXml.Text;
            if (comboProviderName.SelectedIndex > -1)
            {
                m_CurrSource.ProviderName = (string) comboProviderName.SelectedItem;
            }
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
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = (Settings != null && !string.IsNullOrEmpty(Settings.Filename)
                    ? Settings.Filename
                    : string.Empty);
                if (string.IsNullOrEmpty(openFileDialog1.FileName) && Directory.Exists(AppData.SupportPath))
                {
                    openFileDialog1.InitialDirectory = AppData.SupportPath;
                }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string itemName = "CLONE";

                if (UI.InputBox("DATABASE NAME", "What is the name of the database you wish to walk?", ref itemName)
                    == DialogResult.Cancel
                    || string.IsNullOrEmpty(itemName))
                {
                    return;
                }

                if (m_CurrSource == null && Settings.Sources.Count > 0)
                {
                    m_CurrSource = Settings.Sources[0];
                }

                XlgSource newSource = null;
                if (itemName == "CLONE")
                {
                    newSource = Xml.FromXml<XlgSource>(Xml.ToXml<XlgSource>(m_CurrSource, false));
                }
                else
                {
                    newSource = new XlgSource(AppData.BasePath, AppData.ParentNamespace,
                        (m_CurrSource != null
                            ? m_CurrSource.ProviderName
                            : (Settings.Sources.Count + 1).ToString()).LastToken(".") + ": " + itemName, itemName);

                    if (m_CurrSource != null)
                    {
                        newSource.BasePath = m_CurrSource.OutputFilename.TokensBefore(m_CurrSource.OutputFilename.TokenCount(@"\") - 1, @"\") + @"\" + itemName
                                             + @"\";
                    }
                    else
                    {
                        newSource.BasePath = AppData.BasePath + itemName + @"\";
                    }
                    FileSystem.InsureFolderExists(newSource.BasePath, false);
                    // NewSource.ConfigFilename = CurrSource.ConfigFilename;

                    if (m_CurrSource != null)
                    {
                        newSource.ConnectionString = m_CurrSource.ConnectionString;
                        newSource.OutputFilename = newSource.BasePath + itemName + ".Glove."
                                                   + Path.GetExtension(m_CurrSource.OutputFilename);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_CurrSource == null)
                {
                    return;
                }
                var messageBoxResult =
                    MessageBox.Show(
                        "Are you sure you want to remove the current step?\r\n\tThere is no undo for this action.",
                        "REMOVE STEP", MessageBoxButtons.YesNo);
                if (messageBoxResult == DialogResult.No)
                {
                    return;
                }
                Settings.Sources.Remove(m_CurrSource);
                m_CurrSource = null;
                buttonSave_Click(null, null);
                RefreshList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonEditXlgFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileSystem.InsureFolderExists(textXlgFile.Text, true);
                if (!File.Exists(textXlgFile.Text))
                {
                    File.WriteAllText(textXlgFile.Text,
                        DefaultXlg.xml.Replace("[Default]", textConnectionStringName.Text));
                }
                Process.Start(AppData.TextEditor, textXlgFile.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonEditXlgXslFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileSystem.InsureFolderExists(textAppXlgXsl.Text, true);
                Process.Start(AppData.TextEditor, textAppXlgXsl.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonViewOutputFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FileSystem.InsureFolderExists(textOutput.Text, true);
                Process.Start(AppData.TextEditor, textOutput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonViewtextOutputXml_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textOutputXml.Text))
                {
                    textOutputXml.Text = textOutput.Text.Substring(0,
                        textOutput.Text.Length - Path.GetExtension(textOutput.Text).Length) + ".xml";
                }
                FileSystem.InsureFolderExists(textOutputXml.Text, true);
                Process.Start(AppData.TextEditor, textOutputXml.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonChoosetextOutputXml_Click(object sender, EventArgs e)
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
                if (saveFileDialog1.FileName != null)
                {
                    textOutputXml.Text = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XlgSettings t = Settings;
                Settings = new XlgSettings(this)
                {
                    Sources = new List<XlgSource>(),
                    ClipScripts = new List<XlgQuickScript>(),
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
                if (string.IsNullOrEmpty(saveFileDialog1.FileName) || File.Exists(saveFileDialog1.FileName))
                {
                    return;
                }
                Settings.Filename = saveFileDialog1.FileName;
                buttonSave_Click(null, null);
                RefreshList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = Settings.Filename;
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = ".xlgs";
                saveFileDialog1.Filter = "*.xlgs|*.xlgs";
                saveFileDialog1.ShowDialog(this);
                if (string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    return;
                }
                Settings.Filename = saveFileDialog1.FileName;
                buttonSave_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                MessageBox.Show(ex.ToString());
            }
        }

        private void generateSelectedToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void regenerateSelectedToolStripMenuItem_Click(object sender, EventArgs e) { }

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

        private void EditClipScript_Click(object sender, EventArgs e)
        {
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xlgQuickScripts.exe");
            if (!File.Exists(exePath)) MessageBox.Show(this, "Quick scripts missing: " + exePath);
            else Process.Start(exePath, string.Empty);
        }
    }
}