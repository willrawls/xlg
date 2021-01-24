using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using MetX.Data;
using MetX.IO;
using MetX.Library;
using Mvp.Xml.Common.Xsl;
using Mvp.Xml.Exslt;

namespace MetX.Pipelines
{
    /// <summary>Generates Data and xlg specific code</summary>
    public class CodeGenerator
    {
        /// <summary>The class name to contain the Stored Procedures</summary>
        public const string SpClassName = "SPs";

        public static string AppDomainAppPath;

        /// <summary>
        /// List of all the C# keywords
        /// </summary>
        public readonly List<string> CSharpKeywords = new List<string>(new[]
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

        public readonly Form Gui;
        public XmlDocument CodeXmlDocument;

        /// <summary>The namespace that should be passed into the XSL</summary>
        public string Namespace = "xlg";

        public string OutputFolder;

        /// <summary>Set internally (overridden externally) indicating the base vitual directory.</summary>
        public string VDirName;

        /// <summary>Set externally indicating the path/virtual (sub directory) path to any overriding template(s).</summary>
        public string VirtualPath;

        /// <summary>Set externally indicating the file of any overriding template(s)</summary>
        public string VirtualxlgFilePath;

        /// <summary>The Data XML file to generate against</summary>
        public string XlgDataXml = "*";

        /// <summary>The file containing the XSL to render code against.
        /// <para>NOTE: This file does not have to exist. If it doesn't the internal XSL rendering C# will be used.</para>
        /// </summary>
        public string XlgFilename = "app.xlg.xsl";

        public Guid XlgInstanceId;
        private static string _mFullName;

        private XmlElement _mStoredProceduresToRender;

        private XmlElement _mTablesToRender;

        private string _mUrlExtension;

        private XmlDocument _mXlgDataXmlDoc = new XmlDocument();

        private XmlElement _mXslsToRender;

        /// <summary>Default constructor. Does nothing</summary>
        public CodeGenerator()
        {
            if (_mFullName == null)
            {
                _mFullName = Assembly.GetExecutingAssembly().FullName;
            }
        }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on VirtualxlgFilePath</summary>
        public CodeGenerator(string xlgFilePath, string xlgXslFilePath, string settingsFilePath, Form gui)
            : this()
        {
            Gui = gui;
            Initialize(xlgFilePath, xlgXslFilePath, settingsFilePath);
        }

        /// <summary>
        /// Returns an XmlDocument containing a xlgData document with the child elements: Tables, StoredProcedures, and Xsls relative to the list indicated by the supplied include/skip lists.
        /// </summary>
        public XmlDocument DataXml
        {
            get
            {
                ParseDataXml();
                var xmlDoc = new XmlDocument();
                var root = xmlDoc.CreateElement("xlgDoc");

                if (_mXlgDataXmlDoc.DocumentElement == null)
                {
                    return null;
                }

                foreach (XmlAttribute currAttribute in _mXlgDataXmlDoc.DocumentElement.Attributes)
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
                root.SetAttribute("XlgInstanceID", XlgInstanceId.ToString().ToUpper());

                root.SetAttribute("MetXAssemblyString", MetXAssemblyString);

                xmlDoc.AppendChild(root);
                foreach (XmlAttribute currAttribute in _mXlgDataXmlDoc.DocumentElement.Attributes)
                {
                    root.SetAttribute(currAttribute.Name, currAttribute.Value);
                }
                if (_mTablesToRender != null)
                {
                    if (TablesXml(xmlDoc) == null) return null;
                }
                if (_mStoredProceduresToRender != null)
                {
                    StoredProceduresXml(xmlDoc);
                }
                if (_mXslsToRender != null)
                {
                    XslXml(xmlDoc);
                }
                foreach (XmlElement currChild in _mXlgDataXmlDoc.DocumentElement.ChildNodes)
                {
                    root.AppendChild(xmlDoc.ImportNode(currChild, true));
                }

                // AddAttribute(root, "xmlDoc", xmlDoc.InnerXml.Replace("><", ">\n<"));
                return xmlDoc;
            }
        }

        public string MetXAssemblyString { get { return _mFullName; } }

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string GenerateCode()
        {
            var xlgXsl = GetVirtualFile(XlgFilename);
            if (xlgXsl == null || xlgXsl.Length < 5)
            {
                throw new Exception("xlg.xsl missing (1).");
            }
            XlgInstanceId = Guid.NewGuid();
            CodeXmlDocument = DataXml;
            if (CodeXmlDocument == null) return null;
            return Helper.GenerateViaXsl(CodeXmlDocument, xlgXsl).ToString();
        }

        public void Initialize(string xlgFilePath, string xlgXslFilePath, string settingsFilePath)
        {
            VirtualPath = Path.GetDirectoryName(xlgFilePath).AsString().Replace("\\", "/");
            VirtualxlgFilePath = xlgFilePath;
            XlgFilename = xlgXslFilePath;
            XlgDataXml = GetVirtualFile(VirtualxlgFilePath);

            Namespace = Path.GetFileNameWithoutExtension(VirtualxlgFilePath).AsString();
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
            {
                Namespace = Namespace.Substring(0, Namespace.Length - 6);
            }
            VDirName = Namespace;

            if (!string.IsNullOrEmpty(settingsFilePath))
            {
                AppDomainAppPath = Path.GetDirectoryName(settingsFilePath);
            }
            else
            {
                AppDomainAppPath = Path.GetDirectoryName(XlgFilename);
            }

            if (!string.IsNullOrEmpty(settingsFilePath) && settingsFilePath.ToLower().Contains(".config"))
            {
                var configFile = new ExeConfigurationFileMap { ExeConfigFilename = settingsFilePath };
                DataService.ConnectionStrings = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings;
            }
        }

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string RegenerateCode(XmlDocument xmlDoc)
        {
            var xlgXsl = GetVirtualFile(XlgFilename);
            if (xlgXsl == null || xlgXsl.Length < 5)
            {
                throw new Exception("xlg.xsl missing (2).");
            }
            //xlgXsl = MetX.Data.xlg.xsl;
            return Helper.GenerateViaXsl(xmlDoc, xlgXsl).ToString();
        }

        private string Dav(XmlDocument x, string name, string defaultValue)
        {
            string ret;
            if (x.DocumentElement != null && x.DocumentElement.Attributes[name] != null)
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

        private string GetxlgPath(string path)
        {
            return path.Replace("/xsl/", "/").ToLower();
        }

        private void ParseDataXml()
        {
            _mXlgDataXmlDoc = new XmlDocument();
            if (XlgDataXml == null || XlgDataXml.StartsWith("*"))
            {
                _mXlgDataXmlDoc.LoadXml(DefaultXlg.Xml.Replace("[Default]", Namespace));
            }
            else
            {
                _mXlgDataXmlDoc.LoadXml(XlgDataXml);
            }

            _mTablesToRender = (XmlElement)_mXlgDataXmlDoc.SelectSingleNode("/*/Render/Tables");
            _mStoredProceduresToRender = (XmlElement)_mXlgDataXmlDoc.SelectSingleNode("/*/Render/StoredProcedures");
            _mXslsToRender = (XmlElement)_mXlgDataXmlDoc.SelectSingleNode("/*/Render/Xsls");

            var connectionStringName = Dav(_mXlgDataXmlDoc, "ConnectionStringName", "Default");
            //if (xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"] == null)
            //    ConnectionStringName = "Default";
            //else
            //    ConnectionStringName = xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"].Value;
            DataService.Instance = DataService.GetDataService(connectionStringName);
            if (DataService.Instance == null)
            {
                throw new Exception("No valid connection name (from xlgd): " + connectionStringName);
            }

            AddElement(_mXslsToRender, "Exclude", "Name", "~/security/xsl/xlg");
            AddElement(_mXslsToRender, "Exclude", "Name", "~/App_Code");
            AddElement(_mXslsToRender, "Exclude", "Name", "~/App_Data");
            AddElement(_mXslsToRender, "Exclude", "Name", "~/theme");
            AddElement(_mXslsToRender, "Exclude", "Name", "~/bin");
            AddElement(_mXslsToRender, "Exclude", "Name", "_svn");
            AddElement(_mXslsToRender, "Exclude", "Name", ".svn");
            AddElement(_mXslsToRender, "Exclude", "Name", "_vti_pvt");
            AddElement(_mXslsToRender, "Exclude", "Name", "_vti_cnf");
            AddElement(_mXslsToRender, "Exclude", "Name", "_vti_script");
            AddElement(_mXslsToRender, "Exclude", "Name", "_vti_txt");

            AddElement(_mStoredProceduresToRender, "Exclude", "Name", "sp_*");
            AddElement(_mStoredProceduresToRender, "Exclude", "Name", "dt_*");
        }

        private void ProcessXslPath(XmlDocument xmlDoc, string renderPath, string xlgPath, string path, XmlElement parent)
        {
            foreach (var xslFile in Directory.GetFiles(path))
            {
                if (!string.IsNullOrEmpty(xslFile) && Path.GetExtension(xslFile) == ".xsl" && !xslFile.EndsWith(".xlg.xsl") && IsIncluded(_mXslsToRender, xslFile)
                    && IsIncluded(_mXslsToRender, renderPath + "/" + Path.GetFileNameWithoutExtension(xslFile)))
                {
                    var xmlXsl = xmlDoc.CreateElement("XslEndpoint");
                    var classname = Path.GetFileNameWithoutExtension(xslFile).Replace(" ", "_");

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
                    AddAttribute(xmlXsl, "xlgPath", GetxlgPath(xlgPath + "/" + Path.GetFileNameWithoutExtension(xslFile) + "." + _mUrlExtension));
                    AddAttribute(xmlXsl, "FilePath", xslFile);
                    AddAttribute(xmlXsl, "Path", Path.GetDirectoryName(xslFile));
                    AddAttribute(xmlXsl, "Filename", Path.GetFileName(xslFile));
                    AddAttribute(xmlXsl, "Filepart", Path.GetFileNameWithoutExtension(xslFile));
                    AddAttribute(xmlXsl, "Extension", Path.GetExtension(xslFile));
                    AddAttribute(xmlXsl, "IsVirtual", "false");

                    parent.AppendChild(xmlXsl);
                }
            }
            foreach (var xslFolder in Directory.GetDirectories(path))
            {
                var folderName = xslFolder.LastToken(@"\");
                if (IsIncluded(_mXslsToRender, folderName) && IsIncluded(_mXslsToRender, renderPath + "/" + folderName))
                {
                    var xmlXsls = xmlDoc.CreateElement("XslEndpoints");
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

        // ReSharper disable once UnusedMethodReturnValue.Local
        private XmlDocument StoredProceduresXml(XmlDocument xmlDoc)
        {
            //get the SP list from the DB
            var sPs = DataService.Instance.GetSpList();
            var root = xmlDoc.DocumentElement;

            var xmlStoredProcedures = xmlDoc.CreateElement("StoredProcedures");
            AddAttribute(xmlStoredProcedures, "ClassName", SpClassName);
            // ReSharper disable once PossibleNullReferenceException
            root.AppendChild(xmlStoredProcedures);

            var sprocIndex = 1;
            foreach (var spName in sPs)
            {
                // Make sure there is a stored proc to process
                //  (is blank when there are no stored procedures in the database)
                if (spName.Length > 0 && !spName.StartsWith("dt_") && IsIncluded(_mStoredProceduresToRender, spName))
                {
                    var xmlStoredProcedure = xmlDoc.CreateElement("StoredProcedure");
                    AddAttribute(xmlStoredProcedure, "StoredProcedureName", spName);
                    AddAttribute(xmlStoredProcedure, "MethodName", GetProperName(string.Empty, spName, string.Empty).Replace("_", string.Empty).Replace(" ", string.Empty));
                    AddAttribute(xmlStoredProcedure, "Location", (sprocIndex++).ToString());
                    //grab the parameters
                    var paramReader = DataService.Instance.GetSpParams(spName);

                    var xmlParameters = xmlDoc.CreateElement("Parameters");
                    xmlStoredProcedure.AppendChild(xmlParameters);
                    var parameterIndex = 1;
                    while (paramReader.Read())
                    {
                        //loop the params, pulling out the names and dataTypes
                        var xmlParameter = xmlDoc.CreateElement("Parameter");
                        var dbType = DataService.Instance.GetDbType(paramReader["DataType"].ToString().ToLower());
                        var paramName = paramReader["Name"].ToString();

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
                        AddAttribute(xmlParameter, "Location", (parameterIndex++).ToString());
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

        private XmlDocument TablesXml(XmlDocument xmlDoc)
        {
            var root = xmlDoc.DocumentElement;
            var xmlTables = xmlDoc.CreateElement("Tables");
            if (root == null)
            {
                return null;
            }

            root.AppendChild(xmlTables);
            var tables = DataService.Instance.GetTables();
            foreach (var table in tables)
            {
                if (string.IsNullOrEmpty(table) || !IsInList(table))
                {
                    continue;
                }

                TableSchema.Table tbl;
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
                                AddElement(_mTablesToRender, "Exclude", "Name", table);
                                break;

                            case DialogResult.No:
                                break;

                            case DialogResult.Cancel:
                                return null;
                        }
                    }
                    tbl = null;
                }

                if (tbl == null)
                {
                    continue;
                }

                var xmlTable = xmlDoc.CreateElement("Table");
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

                var xmlColumns = xmlDoc.CreateElement("Columns");
                xmlTable.AppendChild(xmlColumns);
                for (var index = 0; index < tbl.Columns.Count; index++)
                {
                    var col = tbl.Columns[index];
                    var xmlColumn = xmlDoc.CreateElement("Column");
                    AddAttribute(xmlColumn, "ColumnName", col.ColumnName);
                    AddAttribute(xmlColumn, "PropertyName", GetProperName(tbl.Name, col.ColumnName, "Field", true));
                    AddAttribute(xmlColumn, "CSharpVariableType", GetCSharpVariableType(col.DataType));
                    AddAttribute(xmlColumn, "Location", (index + 1).ToString());
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
                    AddAttribute(xmlColumn, "SourceType", col.SourceType);
                    AddAttribute(xmlColumn, "DomainName", col.DomainName);
                    AddAttribute(xmlColumn, "Precision", col.Precision.ToString());
                    AddAttribute(xmlColumn, "Scale", col.Scale.ToString());
                    xmlColumns.AppendChild(xmlColumn);
                }
                if (tbl.Keys.Count > 0)
                {
                    var xmlKeys = xmlDoc.CreateElement("Keys");
                    xmlTable.AppendChild(xmlKeys);
                    for (var index = 0; index < tbl.Keys.Count; index++)
                    {
                        var key = tbl.Keys[index];
                        var xmlKey = xmlDoc.CreateElement("Key");
                        AddAttribute(xmlKey, "Name", key.Name);
                        AddAttribute(xmlKey, "IsPrimary", key.IsPrimary.ToString());
                        AddAttribute(xmlKey, "Location", index.ToString());
                        var xmlKeyColumns = xmlDoc.CreateElement("Columns");
                        for (var i = 0; i < key.Columns.Count; i++)
                        {
                            var col = key.Columns[i];
                            var xmlKeyColumn = xmlDoc.CreateElement("Column");
                            AddAttribute(xmlKeyColumn, "Column", col.Column);
                            if (col.Related != null)
                            {
                                AddAttribute(xmlKeyColumn, "Related", col.Related);
                            }
                            AddAttribute(xmlKeyColumn, "Location", i.ToString());
                            xmlKeyColumns.AppendChild(xmlKeyColumn);
                        }
                        xmlKey.AppendChild(xmlKeyColumns);
                        xmlKeys.AppendChild(xmlKey);
                    }
                }
                if (tbl.Indexes.Count > 0)
                {
                    var xmlIndexes = xmlDoc.CreateElement("Indexes");
                    xmlTable.AppendChild(xmlIndexes);
                    for (var i = 0; i < tbl.Indexes.Count; i++)
                    {
                        var index = tbl.Indexes[i];
                        var xmlIndex = xmlDoc.CreateElement("Index");
                        AddAttribute(xmlIndex, "IndexName", index.Name);
                        AddAttribute(xmlIndex, "IsClustered", index.IsClustered.ToString());
                        AddAttribute(xmlIndex, "SingleColumnIndex", index.Columns.Count == 1
                            ? "True"
                            : "False");
                        AddAttribute(xmlIndex, "PropertyName", GetProperName(tbl.Name, index.Name, "Index"));
                        AddAttribute(xmlIndex, "Location", i.ToString());
                        var xmlIndexColumns = xmlDoc.CreateElement("IndexColumns");
                        xmlIndex.AppendChild(xmlIndexColumns);
                        for (var columnIndex = 0; columnIndex < index.Columns.Count; columnIndex++)
                        {
                            var indexColumn = index.Columns[columnIndex];
                            var xmlIndexColumn = xmlDoc.CreateElement("IndexColumn");
                            AddAttribute(xmlIndexColumn, "IndexColumnName", indexColumn);
                            AddAttribute(xmlIndexColumn, "Location", columnIndex.ToString());
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

        // ReSharper disable once UnusedMethodReturnValue.Local
        private XmlDocument XslXml(XmlDocument xmlDoc)
        {
            var renderPath = _mXslsToRender.GetAttribute("Path");
            if (renderPath.Length == 0)
            {
                renderPath = "~";
            }

            _mUrlExtension = _mXslsToRender.GetAttribute("UrlExtension");
            if (string.IsNullOrEmpty(_mUrlExtension))
            {
                _mUrlExtension = "aspx";
            }
            else if (_mUrlExtension.StartsWith("."))
            {
                _mUrlExtension = _mUrlExtension.Substring(1);
            }

            var root = xmlDoc.DocumentElement;
            var path = Helper.VirtualPathToPhysical(renderPath);

            var xmlXsls = xmlDoc.CreateElement("XslEndpoints");
            AddAttribute(xmlXsls, "VirtualPath", renderPath);
            AddAttribute(xmlXsls, "xlgPath", GetxlgPath("/" + VDirName));
            AddAttribute(xmlXsls, "VirtualDir", string.Empty);
            AddAttribute(xmlXsls, "Path", path);
            AddAttribute(xmlXsls, "Folder", path.LastToken(@"\"));
            // ReSharper disable once PossibleNullReferenceException
            root.AppendChild(xmlXsls);

            var xmlNodeList = _mXslsToRender.SelectNodes("Virtual");
            if (xmlNodeList != null)
            {
                foreach (XmlElement currVirtual in xmlNodeList)
                {
                    var xslFile = currVirtual.GetAttribute("Name");
                    var classname = xslFile.Replace(" ", "_").Replace("/", ".");
                    var xmlXsl = xmlDoc.CreateElement("XslEndpoint");

                    if (CSharpKeywords.Contains(classname))
                    {
                        classname = "_" + classname;
                    }

                    if (xmlDoc.SelectSingleNode("/*/Tables/Table[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null ||
                        xmlDoc.SelectSingleNode("/*/StoredProcedures[@ClassName=\"" + Xml.AttributeEncode(classname) + "\"]") != null)
                    {
                        classname += "PageHandler";
                    }

                    AddAttribute(xmlXsl, "xlgPath", GetxlgPath("/" + VDirName + "/" + xslFile + "." + _mUrlExtension));
                    AddAttribute(xmlXsl, "VirtualPath", renderPath + "/" + xslFile + "." + _mUrlExtension);
                    AddAttribute(xmlXsl, "ClassName", classname);
                    AddAttribute(xmlXsl, "Filepart", xslFile);
                    AddAttribute(xmlXsl, "IsVirtual", "true");

                    xmlXsls.AppendChild(xmlXsl);
                }
            }

            ProcessXslPath(xmlDoc, renderPath, "/" + VDirName.ToLower(), path, xmlXsls);

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
                if (virtualFilename.FirstToken(":/") != string.Empty || virtualFilename.Replace("\\", "/").StartsWith(VirtualPath))
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
            /// <summary>Performs a simple XSL transformation on a XmlDocument object</summary>
            /// <param name="xmlDoc">The XmlDocument to convert</param>
            /// <param name="sXsl">The XSLT contents to use in the conversion.</param>
            /// <returns>The rendered content</returns>
            public static StringBuilder GenerateViaXsl(XmlDocument xmlDoc, string sXsl)
            {
                var sOut = new StringBuilder();
                try
                {
                    //xml Transformer = new xml();
                    //sOut = Transformer.xslTransform(XmlDoc, sXsl);

                    var xslt = new MvpXslTransform
                    {
                        SupportedFunctions = ExsltFunctionNamespace.All,
                        MultiOutput = true
                    };
                    var xal = new XsltArgumentList();
                    xal.AddExtensionObject("urn:xlg", new XlgUrn());
                    xslt.Load(XmlReader.Create(new StringReader(sXsl)));
                    using (var sw = new StringWriter(sOut))
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

                    using (var inFile = File.Open(virtualFilename, FileMode.Create ))
                    {
                        var rdr = new StreamReader(inFile);
                        var contents = rdr.ReadToEnd();
                        rdr.Close();
                        rdr.Dispose();
                        return contents;
                    }
                }
                catch
                {
                    // ignored
                }
                return null;
            }

            /// <summary>Attemptes to convert a virtual path into a physical one. Physical path is not guarenteed to exist.</summary>
            /// <param name="virtualPath">The virtual path to map</param>
            /// <returns>The physical file system path represented by VirtualPath</returns>
            public static string VirtualPathToPhysical(string virtualPath) { return virtualPath.Replace("/", @"\").Replace("~", AppDomainAppPath).Replace(@"\\", @"\"); }
        }

        #endregion "Helper Functions"

        #region "Support Functions"

        // Anytime a database column is named any of these words, it causes a code issue.
        //  Make sure a suffix is added to property names in these cases
        private static readonly List<string> MTypeNames = new List<string>(new[] { "guid", "int", "string", "timespan", "double", "single", "float", "decimal", "array" });

        private readonly Dictionary<string, Regex> _mPatterns = new Dictionary<string, Regex>();

        /// <summary>Translates a table name into a CLSCompliant class name</summary>
        /// <param name="tableName">The name of the table, stored procedure, etc to translate</param>
        /// <returns>The translated class name</returns>
        public static string GetClassName(string tableName)
        {
            var className = GetProperName(tableName.Replace(" ", string.Empty));
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

        /// <summary>Translates a DbType into the .net equivalent type to convert another value to (so "Currency" becomes "Convert.ToCurrency")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The portion of code necessary to convert another value to the same type as this</returns>
        public static string GetConvertToPart(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return "Worker.AsString";
                case DbType.AnsiStringFixedLength:
                    return "Worker.AsString";
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
                    return "Worker.AsString";
                case DbType.StringFixedLength:
                    return "Worker.AsString";
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
                    return "Worker.AsString";
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

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string tableName, string fieldName, string suffix, bool keepUnderscores = false)
        {
            if (fieldName != null && fieldName.Length > 1)
            {
                var propertyName = UnderscoreToCamelcase(fieldName, keepUnderscores);
                var cleanTableName = UnderscoreToCamelcase(tableName, keepUnderscores);
                var classTableName = GetClassName(tableName);

                if (propertyName.EndsWith("TypeCode"))
                {
                    propertyName = propertyName.Substring(0, propertyName.Length - 4);
                }
                if (tableName == fieldName || tableName == propertyName || cleanTableName == fieldName || cleanTableName == propertyName || propertyName == "TableName"
                    || MTypeNames.Contains(propertyName.ToLower()))
                {
                    propertyName += suffix;
                }
                else if (classTableName == fieldName || classTableName == propertyName || MTypeNames.Contains(classTableName.ToLower()))
                {
                    propertyName += suffix;
                }
                return propertyName;
            }
            return fieldName;
        }

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string fieldName)
        {
            if (fieldName != null && fieldName.Length > 1)
            {
                var propertyName = UnderscoreToCamelcase(fieldName);
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

        public static string UnderscoreToCamelcase(string toConvert, bool keepUnderscores = false)
        {
            if (toConvert != null && toConvert.Length > 1)
            {
                if (keepUnderscores)
                {
                    toConvert = toConvert.Replace("_", "~~~~");
                }

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
                    var af = toConvert.TokensAfter(1, "_");
                    if (af.Length > 0)
                    {
                        af = af[0].ToString().ToUpper() + af.Substring(1);
                        toConvert = toConvert.FirstToken("_") + af;
                    }
                    else
                    {
                        toConvert = toConvert.FirstToken("_");
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

                if (keepUnderscores)
                {
                    toConvert = toConvert.Replace("~~~~", "_");
                }
            }
            return toConvert;
        }

        /// <summary>Simplified way of adding an attribute to a XmlElement</summary>
        /// <param name="target">The XmlElement to add the attribute to</param>
        /// <param name="attributeName">The name of the attribute to add</param>
        /// <param name="attributeValue">The value of the attribute to add</param>
        public void AddAttribute(XmlElement target, string attributeName, string attributeValue)
        {
            if (target?.OwnerDocument != null)
            {
                var ret = target.OwnerDocument.CreateAttribute(attributeName);
                ret.Value = attributeValue;
                target.Attributes.Append(ret);
            }
        }

        /// <summary>Simplified way of adding an XmlElement to a XmlDocument</summary>
        /// <param name="target">The XmlDocment to add the Element onto</param>
        /// <param name="elementName">The node name of the element</param>
        /// <param name="attributeName">Name of an attribute to add</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns>The XmlElement added</returns>
        public void AddElement(XmlElement target, string elementName, string attributeName, string attributeValue)
        {
            if (target == null)
            {
                return;
            }
            // ReSharper disable once PossibleNullReferenceException
            var x = target.OwnerDocument.CreateElement(elementName);
            AddAttribute(x, attributeName, attributeValue);
            target.AppendChild(x);
        }

        private bool IsAuditField(string colName)
        {
            var bOut = colName.ToLower() == "createdby" || colName.ToLower() == "createdon" || colName.ToLower() == "modifiedby" || colName.ToLower() == "modifiedon";
            return bOut;
        }

        private bool IsExcluded(XmlElement toCheck, string toFind)
        {
            var ret = false;
            if (toCheck != null)
            {
                if (toFind.EndsWith("*"))
                {
                    toFind = toFind.Substring(0, toFind.Length - 1);
                    ret = toCheck.SelectSingleNode(
                        "Exclude[@Name='*" +
                        "' or starts-with(@Name,'" + toFind +
                        "') or starts-with(@Name,'" + Path.GetFileName(toFind) +
                        "') or starts-with(@Name,'" + Path.GetFileNameWithoutExtension(toFind) + "')]") != null;
                }
                else
                {
                    if (toCheck.SelectSingleNode("Include[@Name='" + toFind + "']") != null)
                    {
                        // Specifically included
                        // ReSharper disable once RedundantAssignment
                        ret = false;
                    }
                    else
                    {
                        ret = toCheck.SelectSingleNode(
                            "Exclude[@Name='*" +
                            "' or @Name='" + toFind +
                            "' or @Name='" + Path.GetFileName(toFind) +
                            "' or @Name='" + Path.GetFileNameWithoutExtension(toFind) + "']") != null;
                    }
                }
            }
            return ret;
        }

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

            var xmlNodeList = toCheck.SelectNodes("Include");
            if (xmlNodeList == null)
            {
                return false;
            }

            foreach (XmlElement includer in xmlNodeList)
            {
                var name = includer.Attributes["Name"];
                Regex regex;
                if (!_mPatterns.ContainsKey(name.Value))
                {
                    var pattern = Worker.ConvertWildcardToRegex(name.Value);
                    regex = new Regex(pattern, RegexOptions.Compiled);
                    _mPatterns.Add(name.Value, regex);
                }
                else
                {
                    regex = _mPatterns[name.Value];
                }
                if (regex != null)
                {
                    return regex.IsMatch(toFind);
                }
            }
            return false;
        }

        private bool IsInList(string tableName)
        {
            return tableName != "dtproperties" && IsIncluded(_mTablesToRender, tableName);
        }

        #endregion "Support Functions"
    }
}