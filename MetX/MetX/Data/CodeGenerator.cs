using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using MetX.IO;
using MetX.Library;
using Mvp.Xml.Common.Xsl;
using Mvp.Xml.Exslt;

namespace MetX.Data
{
    /// <summary>Generates Data and xlg specific code</summary>
    public class CodeGenerator
    {
        public static string AppDomainAppPath;
        private static string m_FullName;

        /// <summary>
        /// Returns an XmlDocument containing a xlgData document with the child elements: Tables, StoredProcedures, and Xsls relative to the list indicated by the supplied include/skip lists.
        /// </summary>
        public XmlDocument DataXml
        {
            get
            {
                ParseDataXml();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("xlgDoc");

                if (m_XlgDataXmlDoc.DocumentElement == null)
                {
                    return null;
                }

                foreach (XmlAttribute currAttribute in m_XlgDataXmlDoc.DocumentElement.Attributes)
                {
                    root.SetAttribute(currAttribute.Name, currAttribute.Value);
                }

                root.SetAttribute("Namespace", Namespace);
                root.SetAttribute("VDirName", VDirName);
                if (DataService.Instance != null)
                {
                    root.SetAttribute("DatabaseProvider", DataService.Instance.Provider.Name);
                    root.SetAttribute("ConnectionStringName", DataService.Instance.Settings.Name);
                    root.SetAttribute("ProviderName", DataService.Instance.Settings.ProviderName);
                    root.SetAttribute("MetXObjectName", DataService.Instance.MetXObjectName);
                    root.SetAttribute("MetXProviderAssemblyString", DataService.Instance.MetXProviderAssemblyString);
                    root.SetAttribute("ProviderAssemblyString", DataService.Instance.ProviderAssemblyString);
                }
                root.SetAttribute("OutputFolder", OutputFolder);
                root.SetAttribute("Now", DateTime.Now.ToString("s"));
                root.SetAttribute("XlgInstanceID", XlgInstanceID.ToString().ToUpper());

                root.SetAttribute("MetXAssemblyString", MetXAssemblyString);

                xmlDoc.AppendChild(root);
                foreach (XmlAttribute currAttribute in m_XlgDataXmlDoc.DocumentElement.Attributes)
                {
                    root.SetAttribute(currAttribute.Name, currAttribute.Value);
                }
                if (m_TablesToRender != null)
                {
                    if (TablesXml(xmlDoc) == null) return null;
                }
                if (m_StoredProceduresToRender != null)
                {
                    StoredProceduresXml(xmlDoc);
                }
                if (m_XslsToRender != null)
                {
                    XslXml(xmlDoc);
                }
                foreach (XmlElement currChild in m_XlgDataXmlDoc.DocumentElement.ChildNodes)
                {
                    root.AppendChild(xmlDoc.ImportNode(currChild, true));
                }

                // AddAttribute(root, "xmlDoc", xmlDoc.InnerXml.Replace("><", ">\n<")); 
                return xmlDoc;
            }
        }

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string GenerateCode()
        {
            string xlgXsl = GetVirtualFile(xlgFilename);
            if (xlgXsl == null || xlgXsl.Length < 5)
            {
                throw new Exception("xlg.xsl missing (1).");
            }
            XlgInstanceID = Guid.NewGuid();
            CodeXmlDocument = DataXml;
            if (CodeXmlDocument == null) return null;
            return Helper.GenerateViaXsl(CodeXmlDocument, xlgXsl).ToString();
        }

        public string MetXAssemblyString { get { return m_FullName; } }

        public XmlDocument CodeXmlDocument = null;

        /// <summary>
        /// List of all the C# keywords
        /// </summary>
        public List<string> CSharpKeywords = new List<string>(new[]
        {
            "abstract", "event", "new", "struct", "as", "explicit",
            "null", "switch", "base", "extern", "object", "this",
            "bool", "false", "operator", "throw",
            "break", "finally", "out", "true",
            "byte", "fixed", "override", "try",
            "case", "float", "params", "typeof",
            "catch", "for", "private", "uint",
            "char", "foreach", "protected", "ulong",
            "checked", "goto", "public", "unchecked",
            "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort",
            "continue", "in", "return", "using",
            "decimal", "int", "sbyte", "virtual",
            "default", "interface", "sealed", "volatile",
            "delegate", "internal", "short", "void",
            "do", "is", "sizeof", "while",
            "double", "lock", "stackalloc",
            "else", "long", "static",
            "enum", "namespace", "string"
        });

        public Form Gui;

        /// <summary>The namespace that should be passed into the XSL</summary>
        public string Namespace = "xlg";

        public string OutputFolder;

        /// <summary>Only used for static generation, this is the file that contains the necessary connection string</summary>
        public string SettingsFilePath;

        /// <summary>The class name to contain the Stored Procedures</summary>
        public string spClassName = "SPs";

        private XmlElement m_StoredProceduresToRender;
        private XmlElement m_TablesToRender;
        private string m_UrlExtension;

        /// <summary>Set internally (overridden externally) indicating the base vitual directory.</summary>
        public string VDirName;

        /// <summary>Set externally indicating the path/virtual (sub directory) path to any overriding template(s).</summary>
        public string VirtualPath;

        /// <summary>Set externally indicating the file of any overriding template(s)</summary>
        public string VirtualxlgFilePath;

        /// <summary>The Data XML file to generate against</summary>
        public string xlgDataXml = "*";

        private XmlDocument m_XlgDataXmlDoc = new XmlDocument();

        /// <summary>The file containing the XSL to render code against.
        /// <para>NOTE: This file does not have to exist. If it doesn't the internal XSL rendering C# will be used.</para>
        /// </summary>
        public string xlgFilename = "app.xlg.xsl";

        public Guid XlgInstanceID;
        private XmlElement m_XslsToRender;

        /// <summary>Default constructor. Does nothing</summary>
        public CodeGenerator()
        {
            if (m_FullName == null)
            {
                m_FullName = Assembly.GetExecutingAssembly().FullName;
            }
        }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on VirtualxlgFilePath</summary>
        /// <param name="virtualxlgFilePath">The virtual path and filename containing the xlg / Data XML</param>
        public CodeGenerator(string virtualxlgFilePath)
            : this()
        {
            VirtualPath = Path.GetDirectoryName(virtualxlgFilePath).Replace("\\", "/");
            this.VirtualxlgFilePath = virtualxlgFilePath;
            xlgFilename = virtualxlgFilePath + ".xsl";
            xlgDataXml = GetVirtualFile(virtualxlgFilePath);

            Namespace = Path.GetFileNameWithoutExtension(virtualxlgFilePath);
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
            {
                Namespace = Namespace.Substring(0, Namespace.Length - 6);
            }

            VDirName = StringExtensions.TokenAt(virtualxlgFilePath, 1, "/");
            if (VDirName.Length == 0)
            {
                VDirName = StringExtensions.TokenAt(virtualxlgFilePath, 2, "/");
            }
            try
            {
                AppDomainAppPath = HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                AppDomainAppPath = Path.GetDirectoryName(SettingsFilePath);
            }
        }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on xlg file name and contents</summary>
        public CodeGenerator(string gloveFilename, string contents)
            : this() { Initialize(gloveFilename, contents); }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on VirtualxlgFilePath</summary>
        public CodeGenerator(string xlgFilePath, string xlgXslFilePath, string settingsFilePath, Form gui)
            : this()
        {
            this.Gui = gui;
            Initialize(xlgFilePath, xlgXslFilePath, settingsFilePath);
        }

        public void Initialize(string file, string contents)
        {
            VirtualPath = Path.GetDirectoryName(file).Replace("\\", "/");
            VirtualxlgFilePath = file;
            SettingsFilePath = "";
            xlgFilename = file + ".xsl";
            xlgDataXml = contents;

            Namespace = Path.GetFileNameWithoutExtension(file);
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
            {
                Namespace = Namespace.Substring(0, Namespace.Length - 6);
            }

            VDirName = Namespace;

            if (file.IndexOf("\\App_Code\\") > -1)
            {
                SettingsFilePath = StringExtensions.FirstToken(file, "\\App_Code\\") + "\\web.config";
            }
            else
            {
                SettingsFilePath = StringExtensions.TokensBefore(file, StringExtensions.TokenCount(file, "\\"), "\\") + "\\app.config";
            }
            try
            {
                AppDomainAppPath = HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                if (!string.IsNullOrEmpty(SettingsFilePath))
                {
                    AppDomainAppPath = Path.GetDirectoryName(SettingsFilePath);
                }
                else
                {
                    AppDomainAppPath = Path.GetDirectoryName(xlgFilename);
                }
            }

            if (!string.IsNullOrEmpty(SettingsFilePath) && SettingsFilePath.ToLower().Contains(".config"))
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = SettingsFilePath;
                DataService.ConnectionStrings = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings;
            }
        }

        public void Initialize(string xlgFilePath, string xlgXslFilePath, string settingsFilePath)
        {
            VirtualPath = Path.GetDirectoryName(xlgFilePath).Replace("\\", "/");
            VirtualxlgFilePath = xlgFilePath;
            this.SettingsFilePath = settingsFilePath;
            xlgFilename = xlgXslFilePath;
            xlgDataXml = GetVirtualFile(VirtualxlgFilePath);

            Namespace = Path.GetFileNameWithoutExtension(VirtualxlgFilePath);
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
            {
                Namespace = Namespace.Substring(0, Namespace.Length - 6);
            }
            VDirName = Namespace;

            
            try
            {
                AppDomainAppPath = HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                if (!string.IsNullOrEmpty(settingsFilePath))
                {
                    AppDomainAppPath = Path.GetDirectoryName(settingsFilePath);
                }
                else
                {
                    AppDomainAppPath = Path.GetDirectoryName(xlgFilename);
                }
            }

            if (!string.IsNullOrEmpty(settingsFilePath) && settingsFilePath.ToLower().Contains(".config"))
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = settingsFilePath;
                DataService.ConnectionStrings = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings;
            }
        }

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string RegenerateCode(XmlDocument xmlDoc)
        {
            string xlgXsl = GetVirtualFile(xlgFilename);
            if (xlgXsl == null || xlgXsl.Length < 5)
            {
                throw new Exception("xlg.xsl missing (2).");
            }
            //xlgXsl = MetX.Data.xlg.xsl;
            return Helper.GenerateViaXsl(xmlDoc, xlgXsl).ToString();
        }

        private string Dav(XmlDocument x, string name, string defaultValue)
        {
            string ret = null;
            if (x.DocumentElement.Attributes[name] != null)
            {
                ret = x.DocumentElement.Attributes[name].Value;
                if (string.IsNullOrEmpty(ret))
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
            return ret;
        }

        private void ParseDataXml()
        {
            m_XlgDataXmlDoc = new XmlDocument();
            if (xlgDataXml == null || xlgDataXml.StartsWith("*"))
            {
                m_XlgDataXmlDoc.LoadXml(DefaultXlg.xml.Replace("[Default]", Namespace));
            }
            else
            {
                m_XlgDataXmlDoc.LoadXml(xlgDataXml);
            }

            m_TablesToRender = (XmlElement)m_XlgDataXmlDoc.SelectSingleNode("/*/Render/Tables");
            m_StoredProceduresToRender = (XmlElement)m_XlgDataXmlDoc.SelectSingleNode("/*/Render/StoredProcedures");
            m_XslsToRender = (XmlElement)m_XlgDataXmlDoc.SelectSingleNode("/*/Render/Xsls");

            string connectionStringName = Dav(m_XlgDataXmlDoc, "ConnectionStringName", "Default");
            //if (xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"] == null)
            //    ConnectionStringName = "Default";
            //else
            //    ConnectionStringName = xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"].Value;
            DataService.Instance = DataService.GetDataService(connectionStringName);
            if (DataService.Instance == null)
            {
                throw new Exception("No valid connection name (from xlgd): " + connectionStringName);
            }

            AddElement(m_XslsToRender, "Exclude", "Name", "~/security/xsl/xlg");
            AddElement(m_XslsToRender, "Exclude", "Name", "~/App_Code");
            AddElement(m_XslsToRender, "Exclude", "Name", "~/App_Data");
            AddElement(m_XslsToRender, "Exclude", "Name", "~/theme");
            AddElement(m_XslsToRender, "Exclude", "Name", "~/bin");
            AddElement(m_XslsToRender, "Exclude", "Name", "_svn");
            AddElement(m_XslsToRender, "Exclude", "Name", ".svn");
            AddElement(m_XslsToRender, "Exclude", "Name", "_vti_pvt");
            AddElement(m_XslsToRender, "Exclude", "Name", "_vti_cnf");
            AddElement(m_XslsToRender, "Exclude", "Name", "_vti_script");
            AddElement(m_XslsToRender, "Exclude", "Name", "_vti_txt");

            AddElement(m_StoredProceduresToRender, "Exclude", "Name", "sp_*");
            AddElement(m_StoredProceduresToRender, "Exclude", "Name", "dt_*");
        }

        private string GetxlgPath(string path)
        {
            return path.Replace("/xsl/", "/").ToLower();
            ;
        }

        private XmlDocument XslXml(XmlDocument xmlDoc)
        {
            string renderPath = m_XslsToRender.GetAttribute("Path");
            if (renderPath == null || renderPath.Length == 0)
            {
                renderPath = "~";
            }

            m_UrlExtension = m_XslsToRender.GetAttribute("UrlExtension");
            if (m_UrlExtension == null || m_UrlExtension.Length == 0)
            {
                m_UrlExtension = "aspx";
            }
            else if (m_UrlExtension.StartsWith("."))
            {
                m_UrlExtension = m_UrlExtension.Substring(1);
            }

            XmlElement root = xmlDoc.DocumentElement;
            string path = Helper.VirtualPathToPhysical(renderPath);

            XmlElement xmlXsls = xmlDoc.CreateElement("XslEndpoints");
            AddAttribute(xmlXsls, "VirtualPath", renderPath);
            AddAttribute(xmlXsls, "xlgPath", GetxlgPath("/" + VDirName));
            AddAttribute(xmlXsls, "VirtualDir", string.Empty);
            AddAttribute(xmlXsls, "Path", path);
            AddAttribute(xmlXsls, "Folder", StringExtensions.LastToken(path, @"\"));
            root.AppendChild(xmlXsls);

            foreach (XmlElement currVirtual in m_XslsToRender.SelectNodes("Virtual"))
            {
                string xslFile = currVirtual.GetAttribute("Name");
                string classname = xslFile.Replace(" ", "_").Replace("/", ".");
                XmlElement xmlXsl = xmlDoc.CreateElement("XslEndpoint");

                if (CSharpKeywords.Contains(classname))
                {
                    classname = "_" + classname;
                }

                if (xmlDoc.SelectSingleNode("/*/Tables/Table[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null ||
                    xmlDoc.SelectSingleNode("/*/StoredProcedures[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null)
                {
                    classname += "PageHandler";
                }

                AddAttribute(xmlXsl, "xlgPath", GetxlgPath("/" + VDirName + "/" + xslFile + "." + m_UrlExtension));
                AddAttribute(xmlXsl, "VirtualPath", renderPath + "/" + xslFile + "." + m_UrlExtension);
                AddAttribute(xmlXsl, "ClassName", classname);
                AddAttribute(xmlXsl, "Filepart", xslFile);
                AddAttribute(xmlXsl, "IsVirtual", "true");

                xmlXsls.AppendChild(xmlXsl);
            }

            ProcessXslPath(xmlDoc, renderPath, "/" + VDirName.ToLower(), path, xmlXsls);

            return xmlDoc;
        }

        private void ProcessXslPath(XmlDocument xmlDoc, string renderPath, string xlgPath, string path, XmlElement parent)
        {
            foreach (string xslFile in Directory.GetFiles(path))
            {
                if (xslFile != null && xslFile.Length > 0 && Path.GetExtension(xslFile) == ".xsl" && !xslFile.EndsWith(".xlg.xsl") && IsIncluded(m_XslsToRender, xslFile)
                    && IsIncluded(m_XslsToRender, renderPath + "/" + Path.GetFileNameWithoutExtension(xslFile)))
                {
                    XmlElement xmlXsl = xmlDoc.CreateElement("XslEndpoint");
                    string classname = Path.GetFileNameWithoutExtension(xslFile).Replace(" ", "_");

                    if (CSharpKeywords.Contains(classname))
                    {
                        classname = "_" + classname;
                    }

                    if (xmlDoc.SelectSingleNode("/*/Tables/Table[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null ||
                        xmlDoc.SelectSingleNode("/*/StoredProcedures[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null)
                    {
                        classname += "PageHandler";
                    }

                    AddAttribute(xmlXsl, "ClassName", classname);
                    AddAttribute(xmlXsl, "xlgPath", GetxlgPath(xlgPath + "/" + Path.GetFileNameWithoutExtension(xslFile) + "." + m_UrlExtension));
                    AddAttribute(xmlXsl, "FilePath", xslFile);
                    AddAttribute(xmlXsl, "Path", Path.GetDirectoryName(xslFile));
                    AddAttribute(xmlXsl, "Filename", Path.GetFileName(xslFile));
                    AddAttribute(xmlXsl, "Filepart", Path.GetFileNameWithoutExtension(xslFile));
                    AddAttribute(xmlXsl, "Extension", Path.GetExtension(xslFile));
                    AddAttribute(xmlXsl, "IsVirtual", "false");

                    parent.AppendChild(xmlXsl);
                }
            }
            foreach (string xslFolder in Directory.GetDirectories(path))
            {
                string folderName = StringExtensions.LastToken(xslFolder, @"\");
                if (IsIncluded(m_XslsToRender, folderName) && IsIncluded(m_XslsToRender, renderPath + "/" + folderName))
                {
                    XmlElement xmlXsls = xmlDoc.CreateElement("XslEndpoints");
                    AddAttribute(xmlXsls, "VirtualPath", renderPath + "/" + folderName);
                    AddAttribute(xmlXsls, "xlgPath", GetxlgPath(xlgPath + "/" + folderName));
                    AddAttribute(xmlXsls, "VirtualDir", folderName);
                    AddAttribute(xmlXsls, "Path", xslFolder);
                    AddAttribute(xmlXsls, "Folder", folderName);
                    parent.AppendChild(xmlXsls);
                    ProcessXslPath(xmlDoc, renderPath + "/" + folderName, GetxlgPath(xlgPath + "/" + folderName), xslFolder, xmlXsls);
                }
            }
        }

        private XmlDocument TablesXml(XmlDocument xmlDoc)
        {
            XmlElement root = xmlDoc.DocumentElement;
            XmlElement xmlTables = xmlDoc.CreateElement("Tables");
            if (root == null)
            {
                return null;
            }

            root.AppendChild(xmlTables);
            string[] tables = DataService.Instance.GetTables();
            foreach (string table in tables)
            {
                if (string.IsNullOrEmpty(table) || !IsInList(table))
                {
                    continue;
                }

                TableSchema.Table tbl = null;
                try
                {
                    tbl = DataService.Instance.GetTableSchema(table);
                }
                catch (Exception)
                {
                    if (Gui != null)
                    {
                        switch (MessageBox.Show(Gui, "Unable to get a schema for table: " + table + "\n\n\tAdd table to skip list and continue ?", "CONTINUE ?", MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
                        {
                            case DialogResult.Yes:
                                AddElement(m_TablesToRender, "Exclude", "Name", table);
                                break;

                            case DialogResult.No:
                                break;
                            case DialogResult.Cancel:
                                return null;
                                break;
                        }
                    }
                    tbl = null;
                }

                if (tbl == null)
                {
                    continue;
                }

                XmlElement xmlTable = xmlDoc.CreateElement("Table");
                AddAttribute(xmlTable, "TableName", tbl.Name);
                AddAttribute(xmlTable, "ClassName", GetClassName(tbl.Name));
                if (tbl.PrimaryKey != null)
                {
                    AddAttribute(xmlTable, "PrimaryKeyColumnName", GetProperName(tbl.Name, tbl.PrimaryKey.ColumnName, "Field"));
                }
                else
                {
                    AddAttribute(xmlTable, "PrimaryKeyColumnName", string.Empty);
                }

                XmlElement xmlColumns = xmlDoc.CreateElement("Columns");
                xmlTable.AppendChild(xmlColumns);
                foreach (TableSchema.TableColumn col in tbl.Columns)
                {
                    XmlElement xmlColumn = xmlDoc.CreateElement("Column");
                    AddAttribute(xmlColumn, "ColumnName", col.ColumnName);
                    AddAttribute(xmlColumn, "PropertyName", GetProperName(tbl.Name, col.ColumnName, "Field"));
                    AddAttribute(xmlColumn, "CSharpVariableType", GetCSharpVariableType(col.DataType));
                    AddAttribute(xmlColumn, "IsDotNetObject", GetIsDotNetObject(col.DataType).ToString());
                    AddAttribute(xmlColumn, "CovertToPart", GetConvertToPart(col.DataType));
                    AddAttribute(xmlColumn, "VBVariableType", GetVbVariableType(col.DataType));
                    AddAttribute(xmlColumn, "AuditField", IsAuditField(col.ColumnName).ToString());
                    AddAttribute(xmlColumn, "DbType", col.DataType.ToString());
                    AddAttribute(xmlColumn, "AutoIncrement", col.AutoIncrement.ToString());
                    AddAttribute(xmlColumn, "IsForiegnKey", col.IsForiegnKey.ToString());
                    AddAttribute(xmlColumn, "IsNullable", col.IsNullable.ToString());
                    AddAttribute(xmlColumn, "IsIdentity", col.IsIdentity.ToString());
                    AddAttribute(xmlColumn, "IsPrimaryKey", col.IsPrimaryKey.ToString());
                    AddAttribute(xmlColumn, "IsIndexed", col.IsIndexed.ToString());
                    AddAttribute(xmlColumn, "MaxLength", col.MaxLength.ToString());
                    AddAttribute(xmlColumn, "SourceType", col.SourceType.ToString());
                    AddAttribute(xmlColumn, "DomainName", col.DomainName.ToString());
                    xmlColumns.AppendChild(xmlColumn);
                }
                if (tbl.Keys.Count > 0)
                {
                    XmlElement xmlKeys = xmlDoc.CreateElement("Keys");
                    xmlTable.AppendChild(xmlKeys);
                    foreach (TableSchema.TableKey key in tbl.Keys)
                    {
                        XmlElement xmlKey = xmlDoc.CreateElement("Key");
                        AddAttribute(xmlKey, "Name", key.Name);
                        AddAttribute(xmlKey, "IsPrimary", key.IsPrimary.ToString());
                        XmlElement xmlKeyColumns = xmlDoc.CreateElement("Columns");
                        foreach (TableSchema.TableKeyColumn col in key.Columns)
                        {
                            XmlElement xmlKeyColumn = xmlDoc.CreateElement("Column");
                            AddAttribute(xmlKeyColumn, "Column", col.Column);
                            if (col.Related != null)
                            {
                                AddAttribute(xmlKeyColumn, "Related", col.Related);
                            }
                            xmlKeyColumns.AppendChild(xmlKeyColumn);
                        }
                        xmlKey.AppendChild(xmlKeyColumns);
                        xmlKeys.AppendChild(xmlKey);
                    }
                }
                if (tbl.Indexes.Count > 0)
                {
                    XmlElement xmlIndexes = xmlDoc.CreateElement("Indexes");
                    xmlTable.AppendChild(xmlIndexes);
                    foreach (TableSchema.TableIndex index in tbl.Indexes)
                    {
                        XmlElement xmlIndex = xmlDoc.CreateElement("Index");
                        AddAttribute(xmlIndex, "IndexName", index.Name);
                        AddAttribute(xmlIndex, "IsClustered", index.IsClustered.ToString());
                        AddAttribute(xmlIndex, "SingleColumnIndex", (index.Columns.Count == 1
                            ? "True"
                            : "False"));
                        AddAttribute(xmlIndex, "PropertyName", GetProperName(tbl.Name, index.Name, "Index"));
                        XmlElement xmlIndexColumns = xmlDoc.CreateElement("IndexColumns");
                        xmlIndex.AppendChild(xmlIndexColumns);
                        foreach (string indexColumn in index.Columns)
                        {
                            XmlElement xmlIndexColumn = xmlDoc.CreateElement("IndexColumn");
                            AddAttribute(xmlIndexColumn, "IndexColumnName", indexColumn);
                            AddAttribute(xmlIndexColumn, "PropertyName", GetProperName(tbl.Name, indexColumn, "Field"));
                            xmlIndexColumns.AppendChild(xmlIndexColumn);
                        }
                        xmlIndexes.AppendChild(xmlIndex);
                    }
                }
                xmlTables.AppendChild(xmlTable);
            }
            return xmlDoc;
        }

        private XmlDocument StoredProceduresXml(XmlDocument xmlDoc)
        {
            //get the SP list from the DB
            string[] sPs = DataService.Instance.GetSPList();
            IDataReader paramReader = null;

            XmlElement root = xmlDoc.DocumentElement;

            XmlElement xmlStoredProcedures = xmlDoc.CreateElement("StoredProcedures");
            AddAttribute(xmlStoredProcedures, "ClassName", spClassName);
            root.AppendChild(xmlStoredProcedures);

            foreach (string spName in sPs)
            {
                // Make sure there is a stored proc to process 
                //  (is blank when there are no stored procedures in the database)
                if (spName.Length > 0 && !spName.StartsWith("dt_") && IsIncluded(m_StoredProceduresToRender, spName))
                {
                    XmlElement xmlStoredProcedure = xmlDoc.CreateElement("StoredProcedure");
                    AddAttribute(xmlStoredProcedure, "StoredProcedureName", spName);
                    AddAttribute(xmlStoredProcedure, "MethodName", GetProperName(string.Empty, spName, string.Empty).Replace("_", string.Empty).Replace(" ", string.Empty));

                    //grab the parameters
                    paramReader = DataService.Instance.GetSPParams(spName);

                    XmlElement xmlParameters = xmlDoc.CreateElement("Parameters");
                    xmlStoredProcedure.AppendChild(xmlParameters);
                    while (paramReader.Read())
                    {
                        //loop the params, pulling out the names and dataTypes
                        XmlElement xmlParameter = xmlDoc.CreateElement("Parameter");
                        DbType dbType = DataService.Instance.GetDbType(paramReader["DataType"].ToString().ToLower());
                        string paramName = paramReader["Name"].ToString();

                        if (CSharpKeywords.Contains(paramName))
                        {
                            paramName = "_" + paramName;
                        }

                        bool isInput = false, isOutput = false;
                        switch (paramReader["ParamType"].ToString())
                        {
                            case "IN":
                            case "1":
                                isInput = true;
                                break;
                            case "OUT":
                            case "2":
                                isOutput = true;
                                break;
                            case "":
                                isInput = true;
                                break;
                            default:
                                isInput = true;
                                isOutput = true;
                                break;
                        }
                        AddAttribute(xmlParameter, "DataType", dbType.ToString());
                        AddAttribute(xmlParameter, "CSharpVariableType", GetCSharpVariableType(dbType));
                        AddAttribute(xmlParameter, "CovertToPart", GetConvertToPart(dbType));
                        AddAttribute(xmlParameter, "VBVariableType", GetVbVariableType(dbType));
                        AddAttribute(xmlParameter, "IsDotNetObject", GetIsDotNetObject(dbType).ToString());
                        AddAttribute(xmlParameter, "ParameterName", paramName);
                        AddAttribute(xmlParameter, "VariableName", GetProperName(paramName.Replace("@", string.Empty).Replace(" ", string.Empty)));
                        AddAttribute(xmlParameter, "IsInput", isInput.ToString());
                        AddAttribute(xmlParameter, "IsOutput", isOutput.ToString());
                        xmlParameters.AppendChild(xmlParameter);
                    }
                    paramReader.Close();
                    xmlStoredProcedures.AppendChild(xmlStoredProcedure);
                }
            }
            return xmlDoc;
        }

        #region "Helper Functions"

        /// <summary>Loads a file from the virutal file system relative to VirtualPath</summary>
        /// <param name="virtualFilename">The Virtual file to load</param>
        /// <returns>The contents of the virtual file</returns>
        public string GetVirtualFile(string virtualFilename)
        {
            if (VirtualPath != null)
            {
                if (StringExtensions.FirstToken(virtualFilename, ":/") != string.Empty || virtualFilename.Replace("\\", "/").StartsWith(VirtualPath))
                {
                    return Helper.GetVirtualFile(virtualFilename);
                }
                else
                {
                    return Helper.GetVirtualFile(VirtualPath + "/" + virtualFilename);
                }
            }
            return null;
        }

        /// <summary>
        /// Helper function for loading a virtual file from the virtual file system
        /// </summary>
        public static class Helper
        {
            /// <summary>Load a file from the virtual file system</summary>
            /// <param name="virtualFilename">The virtual path and file to load</param>
            /// <returns>The contents of the virtual file</returns>
            public static string GetVirtualFile(string virtualFilename)
            {
                try
                {
                    if (File.Exists(virtualFilename))
                    {
                        return FileSystem.FileToString(virtualFilename);
                    }
                    else
                    {
                        using (Stream inFile = VirtualPathProvider.OpenFile(virtualFilename))
                        {
                            StreamReader rdr = new StreamReader(inFile);
                            string contents = rdr.ReadToEnd();
                            rdr.Close();
                            rdr.Dispose();
                            return contents;
                        }
                    }
                }
                catch
                { }
                return null;
            }

            /// <summary>Returns a pysical list of files given a virtual path. Only files that acually exist in the virtual path will be returned. An empty list will be returned if the virtual path could not be mapped physically.</summary>
            /// <param name="virtualPath">The virtual path to retrieve a physical list of files</param>
            /// <returns>The physical list of files</returns>
            public static string[] GetPhysicalFileListFromVirtual(string virtualPath)
            {
                try
                {
                    if (virtualPath != null)
                    {
                        return Directory.GetFiles(VirtualPathToPhysical(virtualPath));
                    }
                }
                catch
                { }
                return new string[] { };
            }

            /// <summary>Returns a pysical list of sub directories physically residing in the physical path equivalent to a given virtual path. Only folders that acually exist in the virtual path will be returned. An empty list will be returned if the virtual path could not be mapped physically.</summary>
            /// <param name="virtualPath">The virtual path to retrieve a physical list of sub directories</param>
            /// <returns>The physical list of files</returns>
            public static string[] GetPhysicalFolderListFromVirtual(string virtualPath)
            {
                try
                {
                    if (virtualPath != null)
                    {
                        return Directory.GetDirectories(VirtualPathToPhysical(virtualPath));
                    }
                }
                catch
                { }
                return new string[] { };
            }

            /// <summary>Attemptes to convert a virtual path into a physical one. Physical path is not guarenteed to exist.</summary>
            /// <param name="virtualPath">The virtual path to map</param>
            /// <returns>The physical file system path represented by VirtualPath</returns>
            public static string VirtualPathToPhysical(string virtualPath) { return virtualPath.Replace("/", @"\").Replace("~", AppDomainAppPath).Replace(@"\\", @"\"); }

            /// <summary>Performs a simple XSL transformation on a XmlDocument object</summary>
            /// <param name="xmlDoc">The XmlDocument to convert</param>
            /// <param name="sXsl">The XSLT contents to use in the conversion.</param>
            /// <returns>The rendered content</returns>
            public static StringBuilder GenerateViaXsl(XmlDocument xmlDoc, string sXsl)
            {
                StringBuilder sOut = new StringBuilder();
                try
                {
                    //xml Transformer = new xml();
                    //sOut = Transformer.xslTransform(XmlDoc, sXsl);

                    MvpXslTransform xslt = new MvpXslTransform
                    {
                        SupportedFunctions = ExsltFunctionNamespace.All,
                        MultiOutput = true
                    };
                    XsltArgumentList xal = new XsltArgumentList();
                    xal.AddExtensionObject("urn:xlg", new XlgUrn());
                    xslt.Load(XmlReader.Create(new StringReader(sXsl)));
                    using (StringWriter sw = new StringWriter(sOut))
                    {
                        xslt.Transform(new XmlInput(xmlDoc), xal, new XmlOutput(sw));
                    }

                    sOut.Replace("&amp;", "&");
                    sOut.Replace("&gt;", ">");
                    sOut.Replace("&lt;", "<");
                    //string x = xslt.TemporaryFiles.BasePath;
                }
                catch (Exception x)
                {
                    throw new Exception("(CodeGenerator.Helper.GenerateViaXsl) " + x.Message + "\n\n" + x.StackTrace, x);
                }
                return sOut;
            }
        }

        #endregion

        #region "Support Functions"

        private bool IsInList(string tableName) { return tableName != "dtproperties" && IsIncluded(m_TablesToRender, tableName); }

        private bool IsExcluded(XmlElement toCheck, string toFind)
        {
            bool ret = false;
            if (toCheck != null)
            {
                if (toFind.EndsWith("*"))
                {
                    toFind = toFind.Substring(0, toFind.Length - 1);
                    ret = (toCheck.SelectSingleNode(
                        "Exclude[@Name='*" +
                        "' or starts-with(@Name,'" + toFind +
                        "') or starts-with(@Name,'" + Path.GetFileName(toFind) +
                        "') or starts-with(@Name,'" + Path.GetFileNameWithoutExtension(toFind) + "')]") != null);
                }
                else
                {
                    if (toCheck.SelectSingleNode("Include[@Name='" + toFind + "']") != null)
                    {
                        // Specifically included
                        ret = false;
                    }
                    else
                    {
                        ret = (toCheck.SelectSingleNode(
                            "Exclude[@Name='*" +
                            "' or @Name='" + toFind +
                            "' or @Name='" + Path.GetFileName(toFind) +
                            "' or @Name='" + Path.GetFileNameWithoutExtension(toFind) + "']") != null);
                    }
                }
            }
            return ret;
        }

        private readonly Dictionary<string, Regex> m_Patterns = new Dictionary<string, Regex>();

        private bool IsIncluded(XmlElement toCheck, string toFind)
        {
            if (toCheck.ChildNodes.Count == 0)
            {
                return true;
            }
            if (IsExcluded(toCheck, toFind))
            {
                return false;
            }
            if (toCheck.SelectSingleNode("Include[@Name=\"*\"]") != null)
            {
                return true;
            }

            XmlNodeList xmlNodeList = toCheck.SelectNodes("Include");
            if (xmlNodeList == null)
            {
                return false;
            }

            foreach (XmlElement includer in xmlNodeList)
            {
                XmlAttribute name = includer.Attributes["Name"];
                Regex regex = null;
                if (!m_Patterns.ContainsKey(name.Value))
                {
                    string pattern = Worker.ConvertWildcardToRegex(name.Value);
                    regex = new Regex(pattern, RegexOptions.Compiled);
                    m_Patterns.Add(name.Value, regex);
                }
                else
                {
                    regex = m_Patterns[name.Value];
                }
                if (regex != null)
                {
                    return regex.IsMatch(toFind);
                }
            }
            return false;
            //if(ToFind.EndsWith("*"))
            //    return ToCheck.SelectSingleNode("Include[starts-with(@Name,\"" + ToFind.Substring(0,ToFind.Length - 1) + "\")]") != null;
            //if (!ToFind.Contains("*"))
            //    return ToCheck.SelectSingleNode("Include[@Name=\"" + ToFind + "\"]") != null;

            //int starIndex = ToFind.IndexOf("*", StringComparison.Ordinal);
            //XmlNodeList parts = ToCheck.SelectNodes("Include[starts-with(@Name,\"" + ToFind.Substring(0, starIndex - 1) + "\")]");
            //if (parts == null || parts.Count == 0)
            //    return false;
            //foreach (XmlNode part in parts)
            //{
            //    if (part.Attributes == null) { continue; }
            //    XmlAttribute name = part.Attributes["Name"];
            //    if (name == null) { continue; }
            //    if (name.Value.EndsWith(ToFind.Substring(starIndex + 1)))
            //        return true;
            //}
            //return ToCheck.SelectSingleNode("Include[@Name=\"" + ToFind + "\"]") != null;
        }

        private bool IsAuditField(string colName)
        {
            bool bOut = false;
            if (colName.ToLower() == "createdby" || colName.ToLower() == "createdon" || colName.ToLower() == "modifiedby" || colName.ToLower() == "modifiedon")
            {
                bOut = true;
            }
            return bOut;
        }

        //bool isEnum(string tableName)
        //{
        //    if (tableName.ToLower().EndsWith("type") || tableName.ToLower().EndsWith("status"))
        //        return true;
        //    return false;
        //}

        /// <summary>Translates a table name into a CLSCompliant class name</summary>
        /// <param name="tableName">The name of the table, stored procedure, etc to translate</param>
        /// <returns>The translated class name</returns>
        public static string GetClassName(string tableName)
        {
            string className = GetProperName(tableName.Replace(" ", string.Empty));
            //if the table is a plural, make it singular
            if (className.EndsWith("ies"))
            {
                //remove the ies at the end
                className = className.Remove(className.Length - 3, 3);
                //add y
                className += "y";
            }
            else if (!className.EndsWith("ss") && !className.EndsWith("us") && className.EndsWith("s"))
            {
                //remove the s
                className = className.Remove(className.Length - 1, 1);
            }
            className = UnderscoreToCamelcase(className);
            return className;
        }

        public static string UnderscoreToCamelcase(string toConvert)
        {
            if (toConvert != null && toConvert.Length > 1)
            {
                toConvert = toConvert.Replace("$", "_");
                toConvert = toConvert.Replace("#", "_");
                toConvert = toConvert.Replace("!", "_");
                toConvert = toConvert.Replace("@", "_");
                toConvert = toConvert.Replace("%", "_");
                toConvert = toConvert.Replace("^", "_");
                toConvert = toConvert.Replace("&", "_");
                toConvert = toConvert.Replace("*", "_");
                toConvert = toConvert.Replace("(", "_");
                toConvert = toConvert.Replace(")", "_");

                if (toConvert.ToLower().StartsWith("tb"))
                {
                    toConvert = toConvert.Substring(2);
                }

                toConvert = toConvert.Substring(0, 1).ToUpper() + toConvert.Substring(1, toConvert.Length - 1);

                toConvert = toConvert.Replace("____", "_")
                                     .Replace("___", "_")
                                     .Replace("__", "_")
                                     .Replace("__", "_")
                                     .Replace("__", "_");

                while (toConvert.IndexOf("_", StringComparison.Ordinal) > -1)
                {
                    string af = StringExtensions.TokensAfter(toConvert, 1, "_");
                    if (af.Length > 0)
                    {
                        af = af[0].ToString().ToUpper() + af.Substring(1);
                        toConvert = StringExtensions.FirstToken(toConvert, "_") + af;
                    }
                    else
                    {
                        toConvert = StringExtensions.FirstToken(toConvert, "_");
                    }
                }
                if (toConvert == "Type")
                {
                    toConvert = "TypeTable";
                }
                if (toConvert[0] >= '0' && toConvert[0] <= '9')
                {
                    toConvert = "_" + toConvert;
                }
            }
            return toConvert;
        }

        /// <summary>Simplified way of adding an XmlElement to a XmlDocument</summary>
        /// <param name="target">The XmlDocment to add the Element onto</param>
        /// <param name="elementName">The node name of the element</param>
        /// <param name="attributeName">Name of an attribute to add</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns>The XmlElement added</returns>
        public XmlElement AddElement(XmlElement target, string elementName, string attributeName, string attributeValue)
        {
            if (target == null)
            {
                return null;
            }
            XmlElement x = target.OwnerDocument.CreateElement(elementName);
            AddAttribute(x, attributeName, attributeValue);
            target.AppendChild(x);
            return x;
        }

        /// <summary>Simplified way of adding an attribute to a XmlElement</summary>
        /// <param name="target">The XmlElement to add the attribute to</param>
        /// <param name="attributeName">The name of the attribute to add</param>
        /// <param name="attributeValue">The value of the attribute to add</param>
        public void AddAttribute(XmlElement target, string attributeName, string attributeValue)
        {
            if (target == null)
            {
                return;
            }
            XmlAttribute ret = target.OwnerDocument.CreateAttribute(attributeName);
            ret.Value = attributeValue;
            target.Attributes.Append(ret);
        }

        // Anytime a database column is named any of these words, it causes a code issue. 
        //  Make sure a suffix is added to property names in these cases
        private static readonly List<string> m_TypeNames = new List<string>(new string[] { "guid", "int", "string", "timespan", "double", "single", "float", "decimal", "array" });

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <param name="sIn">The string to proper case</param>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string tableName, string fieldName, string suffix)
        {
            if (fieldName != null && fieldName.Length > 1)
            {
                string propertyName = UnderscoreToCamelcase(fieldName);
                string cleanTableName = UnderscoreToCamelcase(tableName);
                string classTableName = GetClassName(tableName);

                if (propertyName.EndsWith("TypeCode"))
                {
                    propertyName = propertyName.Substring(0, propertyName.Length - 4);
                }
                if (tableName == fieldName || tableName == propertyName || cleanTableName == fieldName || cleanTableName == propertyName || propertyName == "TableName"
                    || m_TypeNames.Contains(propertyName.ToLower()))
                {
                    propertyName += suffix;
                }
                else if (classTableName == fieldName || classTableName == propertyName || m_TypeNames.Contains(classTableName.ToLower()))
                {
                    propertyName += suffix;
                }
                return propertyName;
            }
            return fieldName;
        }

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <param name="sIn">The string to proper case</param>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string fieldName)
        {
            if (fieldName != null && fieldName.Length > 1)
            {
                string propertyName = UnderscoreToCamelcase(fieldName);
                if (propertyName.EndsWith("TypeCode"))
                {
                    propertyName = propertyName.Substring(0, propertyName.Length - 4);
                }
                return propertyName;
            }
            return fieldName;
        }

        /// <summary>Translates a DbType into the VB.NET equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The VB.NET type string</returns>
        public static string GetVbVariableType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return "String";
                case DbType.AnsiStringFixedLength:
                    return "String";
                case DbType.Binary:
                    return "Byte()";
                case DbType.Boolean:
                    return "Bool";
                case DbType.Byte:
                    return "Byte";
                case DbType.Currency:
                    return "Decimal";
                case DbType.Date:
                    return "DateTime";
                case DbType.DateTime:
                    return "DateTime";
                case DbType.Decimal:
                    return "Decimal";
                case DbType.Double:
                    return "Double";
                case DbType.Guid:
                    return "Guid";
                case DbType.Int16:
                    return "Short";
                case DbType.Int32:
                    return "Int";
                case DbType.Int64:
                    return "Long";
                case DbType.Object:
                    return "Object";
                case DbType.SByte:
                    return "sbyte";
                case DbType.Single:
                    return "Float";
                case DbType.String:
                    return "String";
                case DbType.StringFixedLength:
                    return "String";
                case DbType.Time:
                    return "TimeSpan";
                case DbType.UInt16:
                    return "UShort";
                case DbType.UInt32:
                    return "UInt";
                case DbType.UInt64:
                    return "ULong";
                case DbType.VarNumeric:
                    return "decimal";
                default:
                    return "String";
            }
        }

        /// <summary>Translates a DbType into the C# equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The C# type string</returns>
        public static string GetCSharpVariableType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return "string";
                case DbType.AnsiStringFixedLength:
                    return "string";
                case DbType.Binary:
                    return "byte[]";
                case DbType.Boolean:
                    return "bool";
                case DbType.Byte:
                    return "byte";
                case DbType.Currency:
                    return "decimal";
                case DbType.Date:
                    return "DateTime";
                case DbType.DateTime:
                    return "DateTime";
                case DbType.Decimal:
                    return "decimal";
                case DbType.Double:
                    return "double";
                case DbType.Guid:
                    return "Guid";
                case DbType.Int16:
                    return "short";
                case DbType.Int32:
                    return "int";
                case DbType.Int64:
                    return "long";
                case DbType.Object:
                    return "object";
                case DbType.SByte:
                    return "sbyte";
                case DbType.Single:
                    return "float";
                case DbType.String:
                    return "string";
                case DbType.StringFixedLength:
                    return "string";
                case DbType.Time:
                    return "TimeSpan";
                case DbType.UInt16:
                    return "ushort";
                case DbType.UInt32:
                    return "uint";
                case DbType.UInt64:
                    return "ulong";
                case DbType.VarNumeric:
                    return "decimal";
                default:
                    return "string";
            }
        }

        /// <summary>Translates a DbType into the C# equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The C# type string</returns>
        public static bool GetIsDotNetObject(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Boolean:
                    return false;
                case DbType.Byte:
                    return false;
                case DbType.Currency:
                    return false;
                case DbType.Date:
                    return false;
                case DbType.DateTime:
                    return false;
                case DbType.Decimal:
                    return false;
                case DbType.Double:
                    return false;
                case DbType.Int16:
                    return false;
                case DbType.Int32:
                    return false;
                case DbType.Int64:
                    return false;
                case DbType.SByte:
                    return false;
                case DbType.Single:
                    return false;
                case DbType.Time:
                    return false;
                case DbType.UInt16:
                    return false;
                case DbType.UInt32:
                    return false;
                case DbType.UInt64:
                    return false;
                case DbType.VarNumeric:
                    return false;
            }
            return true;
        }

        /// <summary>Translates a DbType into the .net equivalent type to convert another value to (so "Currency" becomes "Convert.ToCurrency")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The portion of code necessary to convert another value to the same type as this</returns>
        public static string GetConvertToPart(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return "Worker.nzString";
                case DbType.AnsiStringFixedLength:
                    return "Worker.nzString";
                case DbType.Binary:
                    return "Worker.nzByteArray";
                case DbType.Boolean:
                    return "Worker.nzBoolean";
                case DbType.Byte:
                    return "Worker.nzByte";
                case DbType.Currency:
                    return "Worker.nzDecimal";
                case DbType.Date:
                    return "Worker.nzDateTime";
                case DbType.DateTime:
                    return "Worker.nzDateTime";
                case DbType.Decimal:
                    return "Worker.nzDecimal";
                case DbType.Double:
                    return "Worker.nzDouble";
                case DbType.Guid:
                    return "Worker.nzGuid";
                case DbType.Int16:
                    return "Worker.nzShort";
                case DbType.Int32:
                    return "Worker.nzInteger";
                case DbType.Int64:
                    return "Worker.nzLong";
                case DbType.Object:
                    return string.Empty;
                case DbType.SByte:
                    return "Worker.nzSByte";
                case DbType.Single:
                    return "Worker.nzFloat";
                case DbType.String:
                    return "Worker.nzString";
                case DbType.StringFixedLength:
                    return "Worker.nzString";
                case DbType.Time:
                    return "Worker.nzTimeSpan";
                case DbType.UInt16:
                    return "Worker.nzUShort";
                case DbType.UInt32:
                    return "Worker.nzUInt";
                case DbType.UInt64:
                    return "Worker.nzULong";
                case DbType.VarNumeric:
                    return "Worker.nzDecimal";
                default:
                    return "Worker.nzString";
            }
        }

        #endregion
    }
}