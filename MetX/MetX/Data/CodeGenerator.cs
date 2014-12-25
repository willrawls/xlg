using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Hosting;

using Mvp.Xml.Common.Xsl;

using System.Windows.Forms;

namespace MetX.Data
{
    /// <summary>Generates Data and xlg specific code</summary>
    public class CodeGenerator
    {
		/// <summary>
		/// List of all the C# keywords
		/// </summary>
		public List<string> CSKeywords = new List<string>(new string[] 
		{ 
			"abstract","event","new","struct","as","explicit",
			"null","switch","base","extern","object","this",
			"bool","false","operator","throw",
			"break","finally","out","true",
			"byte","fixed","override","try",
			"case","float","params","typeof",
			"catch","for","private","uint",
			"char","foreach","protected","ulong",
			"checked","goto","public","unchecked",
			"class","if","readonly","unsafe",
			"const","implicit","ref","ushort",
			"continue","in","return","using",
			"decimal","int","sbyte","virtual",
			"default","interface","sealed","volatile",
			"delegate","internal","short","void",
			"do","is","sizeof","while",
			"double","lock","stackalloc",
			"else","long","static",
			"enum","namespace","string"
		});

        /// <summary>The Data XML file to generate against</summary>
        public string xlgDataXml = "*";

        /// <summary>The class name to contain the Stored Procedures</summary>
        public string spClassName = "SPs";

        /// <summary>Set externally indicating the path/virtual (sub directory) path to any overriding template(s).</summary>
        public string VirtualPath;

        /// <summary>Set externally indicating the file of any overriding template(s)</summary>
        public string VirtualxlgFilePath;

        /// <summary>Only used for static generation, this is the file that contains the necessary connection string</summary>
        public string SettingsFilePath;

        /// <summary>The namespace that should be passed into the XSL</summary>
        public string Namespace = "xlg";

        /// <summary>Set internally (overridden externally) indicating the base vitual directory.</summary>
        public string VDirName;

        XmlDocument xlgDataXmlDoc = new XmlDocument();

        public Guid XlgInstanceID;

		public System.Windows.Forms.Form GUI;

        private XmlElement TablesToRender;
        private XmlElement StoredProceduresToRender;
        private XmlElement XslsToRender;
        private string UrlExtension;

        public static string AppDomainAppPath;

        /// <summary>The file containing the XSL to render code against.
        /// <para>NOTE: This file does not have to exist. If it doesn't the internal XSL rendering C# will be used.</para>
        /// </summary>
        public string xlgFilename = "app.xlg.xsl";

        private static string FullName;
        public string MetXAssemblyString
        {
            get { return FullName; }
        }

        /// <summary>Default constructor. Does nothing</summary>
        public CodeGenerator()
        {
            if (FullName == null)
                FullName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on VirtualxlgFilePath</summary>
        /// <param name="VirtualxlgFilePath">The virtual path and filename containing the xlg / Data XML</param>
        public CodeGenerator(string VirtualxlgFilePath) : this()
        {
            VirtualPath = Path.GetDirectoryName(VirtualxlgFilePath).Replace("\\", "/");
            this.VirtualxlgFilePath = VirtualxlgFilePath;
            xlgFilename = VirtualxlgFilePath + ".xsl";
            xlgDataXml = GetVirtualFile(VirtualxlgFilePath);

            Namespace = Path.GetFileNameWithoutExtension(VirtualxlgFilePath);
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
                Namespace = Namespace.Substring(0, Namespace.Length - 6);

            VDirName = Token.Get(VirtualxlgFilePath, 1, "/");
            if(VDirName.Length == 0)
                VDirName = Token.Get(VirtualxlgFilePath, 2, "/");
            try
            {
                AppDomainAppPath = System.Web.HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                AppDomainAppPath = Path.GetDirectoryName(SettingsFilePath);
            }
        }

        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on xlg file name and contents</summary>
        public CodeGenerator(string gloveFilename, string contents) : this()
        {
            Initialize(gloveFilename, contents);
        }


        /// <summary>Internally sets VirtualPath, VirtualxlgFilePath, xlgDataXml, Namespace, and VDirName based on VirtualxlgFilePath</summary>
        public CodeGenerator(string xlgFilePath, string xlgXslFilePath, string SettingsFilePath, System.Windows.Forms.Form GUI) : this()
        {
			this.GUI = GUI;
            Initialize(xlgFilePath, xlgXslFilePath, SettingsFilePath);
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
                Namespace = Namespace.Substring(0, Namespace.Length - 6);

            VDirName = Namespace;

            if (file.IndexOf("\\App_Code\\") > -1)
                SettingsFilePath = Token.First(file, "\\App_Code\\") + "\\web.config";
            else
                SettingsFilePath = Token.Before(file, Token.Count(file, "\\"), "\\") + "\\app.config";
            try
            {
                MetX.Data.CodeGenerator.AppDomainAppPath = System.Web.HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                if (!string.IsNullOrEmpty(SettingsFilePath))
                    AppDomainAppPath = Path.GetDirectoryName(SettingsFilePath);
                else
                    AppDomainAppPath = Path.GetDirectoryName(xlgFilename);
            }

            if (!string.IsNullOrEmpty(SettingsFilePath) && SettingsFilePath.ToLower().Contains(".config"))
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = SettingsFilePath;
                DataService.ConnectionStrings = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings;
            }
        }
        public void Initialize(string xlgFilePath, string xlgXslFilePath, string SettingsFilePath)
        {
            VirtualPath = Path.GetDirectoryName(xlgFilePath).Replace("\\", "/");
            VirtualxlgFilePath = xlgFilePath;
            this.SettingsFilePath = SettingsFilePath;
            xlgFilename = xlgXslFilePath;
            xlgDataXml = GetVirtualFile(VirtualxlgFilePath);

            Namespace = Path.GetFileNameWithoutExtension(VirtualxlgFilePath);
            if (Namespace.ToUpper().EndsWith(".GLOVE"))
                Namespace = Namespace.Substring(0, Namespace.Length - 6);
            VDirName = Namespace;

            try
            {
                AppDomainAppPath = System.Web.HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                if (!string.IsNullOrEmpty(SettingsFilePath))
                    AppDomainAppPath = Path.GetDirectoryName(SettingsFilePath);
                else
                    AppDomainAppPath = Path.GetDirectoryName(xlgFilename);
            }

            if (!string.IsNullOrEmpty(SettingsFilePath) && SettingsFilePath.ToLower().Contains(".config"))
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = SettingsFilePath;
                DataService.ConnectionStrings = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings;
            }
        }

        public string OutputFolder;

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string Code
        {
            get
            {
                string xlgXsl = GetVirtualFile(xlgFilename);
                if (xlgXsl == null || xlgXsl.Length < 5)
                    throw new Exception("xlg.xsl missing (1).");
                    //xlgXsl = MetX.Data.xlg.xsl;
                XlgInstanceID = Guid.NewGuid();
                CodeXmlDocument = DataXml;
                return Helper.GenerateViaXsl(CodeXmlDocument, xlgXsl).ToString();
            }
        }

        public XmlDocument CodeXmlDocument = null;

        /// <summary>Causes generation and returns the code/contents generated</summary>
        public string RegenerateCode(XmlDocument xmlDoc)
        {
            string xlgXsl = GetVirtualFile(xlgFilename);
            if (xlgXsl == null || xlgXsl.Length < 5)
                throw new Exception("xlg.xsl missing (2).");
                //xlgXsl = MetX.Data.xlg.xsl;
            return Helper.GenerateViaXsl(xmlDoc, xlgXsl).ToString();
        }

        #region "Helper Functions"
        /// <summary>Loads a file from the virutal file system relative to VirtualPath</summary>
        /// <param name="VirtualFilename">The Virtual file to load</param>
        /// <returns>The contents of the virtual file</returns>
        public string GetVirtualFile(string VirtualFilename)
        {
            if (VirtualPath != null)
            {
                if (Token.First(VirtualFilename, ":/") != string.Empty  || VirtualFilename.Replace("\\", "/").StartsWith(VirtualPath))
                    return Helper.GetVirtualFile(VirtualFilename);
                else
                    return Helper.GetVirtualFile(VirtualPath + "/" + VirtualFilename);
            }
            return null;
        }

        /// <summary>
        /// Helper function for loading a virtual file from the virtual file system
        /// </summary>
        public static class Helper
        {
            /// <summary>Load a file from the virtual file system</summary>
            /// <param name="VirtualFilename">The virtual path and file to load</param>
            /// <returns>The contents of the virtual file</returns>
            public static string GetVirtualFile(string VirtualFilename)
            {
                try
                {
                    if (File.Exists(VirtualFilename))
                    {
                        return MetX.IO.FileSystem.FileToString(VirtualFilename);
                    }
                    else
                    {
                        using (Stream inFile = VirtualPathProvider.OpenFile(VirtualFilename))
                        {
                            StreamReader rdr = new StreamReader(inFile);
                            string contents = rdr.ReadToEnd();
                            rdr.Close();
                            rdr.Dispose();
                            return contents;
                        }
                    }
                }
                catch { }
                return null;
            }

            /// <summary>Returns a pysical list of files given a virtual path. Only files that acually exist in the virtual path will be returned. An empty list will be returned if the virtual path could not be mapped physically.</summary>
            /// <param name="VirtualPath">The virtual path to retrieve a physical list of files</param>
            /// <returns>The physical list of files</returns>
            public static string[] GetPhysicalFileListFromVirtual(string VirtualPath)
            {
                try
                {
                    if (VirtualPath != null)
                        return Directory.GetFiles(VirtualPathToPhysical(VirtualPath));
                }
                catch { }
                return new string[] { };
            }

            /// <summary>Returns a pysical list of sub directories physically residing in the physical path equivalent to a given virtual path. Only folders that acually exist in the virtual path will be returned. An empty list will be returned if the virtual path could not be mapped physically.</summary>
            /// <param name="VirtualPath">The virtual path to retrieve a physical list of sub directories</param>
            /// <returns>The physical list of files</returns>
            public static string[] GetPhysicalFolderListFromVirtual(string VirtualPath)
            {
                try
                {
                    if (VirtualPath != null)
                        return Directory.GetDirectories(VirtualPathToPhysical(VirtualPath));
                }
                catch { }
                return new string[] { };
            }

            /// <summary>Attemptes to convert a virtual path into a physical one. Physical path is not guarenteed to exist.</summary>
            /// <param name="VirtualPath">The virtual path to map</param>
            /// <returns>The physical file system path represented by VirtualPath</returns>
            public static string VirtualPathToPhysical(string VirtualPath)
            {
                return VirtualPath.Replace("/", @"\").Replace("~", AppDomainAppPath).Replace(@"\\", @"\");
            }

            /// <summary>Performs a simple XSL transformation on a XmlDocument object</summary>
            /// <param name="XmlDoc">The XmlDocument to convert</param>
            /// <param name="sXsl">The XSLT contents to use in the conversion.</param>
            /// <returns>The rendered content</returns>
            public static StringBuilder GenerateViaXsl(XmlDocument XmlDoc, string sXsl)
            {
                StringBuilder sOut = new StringBuilder();
                try
                {
                    //xml Transformer = new xml();
                    //sOut = Transformer.xslTransform(XmlDoc, sXsl);

                    MvpXslTransform xslt = new MvpXslTransform();
                    xslt.SupportedFunctions = Mvp.Xml.Exslt.ExsltFunctionNamespace.All;
                    xslt.MultiOutput = true;
                    System.Xml.Xsl.XsltArgumentList xal = new System.Xml.Xsl.XsltArgumentList();
                    xal.AddExtensionObject("urn:xlg", new MetX.Urn.XlgUrn());
                    xslt.Load(System.Xml.XmlReader.Create(new StringReader(sXsl)));
                    using(StringWriter sw = new StringWriter(sOut))
                        xslt.Transform(new XmlInput(XmlDoc), xal, new XmlOutput(sw));

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

		private string DAV(XmlDocument x, string Name, string DefaultValue)
		{
			string ret = null;
			if (x.DocumentElement.Attributes[Name] != null)
			{
				ret = x.DocumentElement.Attributes[Name].Value;
				if (string.IsNullOrEmpty(ret))
					return DefaultValue;
			}
			else
				return DefaultValue;
			return ret;
		}

        private void ParseDataXml()
        {
            xlgDataXmlDoc = new XmlDocument();
            if (xlgDataXml == null || xlgDataXml.StartsWith("*"))
                xlgDataXmlDoc.LoadXml(MetX.Data.DefaultXlg.xml.Replace("[Default]", Namespace));
            else
                xlgDataXmlDoc.LoadXml(xlgDataXml);

            TablesToRender = (XmlElement)xlgDataXmlDoc.SelectSingleNode("/*/Render/Tables");
            StoredProceduresToRender = (XmlElement)xlgDataXmlDoc.SelectSingleNode("/*/Render/StoredProcedures");
            XslsToRender = (XmlElement)xlgDataXmlDoc.SelectSingleNode("/*/Render/Xsls");

            string ConnectionStringName = DAV(xlgDataXmlDoc, "ConnectionStringName", "Default");
			//if (xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"] == null)
			//    ConnectionStringName = "Default";
			//else
			//    ConnectionStringName = xlgDataXmlDoc.DocumentElement.Attributes["ConnectionStringName"].Value;
            DataService.Instance = DataService.GetDataService(ConnectionStringName);

            AddElement(XslsToRender, "Exclude", "Name", "~/security/xsl/xlg");
            AddElement(XslsToRender, "Exclude", "Name", "~/App_Code");
            AddElement(XslsToRender, "Exclude", "Name", "~/App_Data");
            AddElement(XslsToRender, "Exclude", "Name", "~/theme");
            AddElement(XslsToRender, "Exclude", "Name", "~/bin");
            AddElement(XslsToRender, "Exclude", "Name", "_svn");
            AddElement(XslsToRender, "Exclude", "Name", ".svn");
            AddElement(XslsToRender, "Exclude", "Name", "_vti_pvt");
            AddElement(XslsToRender, "Exclude", "Name", "_vti_cnf");
            AddElement(XslsToRender, "Exclude", "Name", "_vti_script");
            AddElement(XslsToRender, "Exclude", "Name", "_vti_txt");

			AddElement(StoredProceduresToRender, "Exclude", "Name", "sp_*");
			AddElement(StoredProceduresToRender, "Exclude", "Name", "dt_*");
        }

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

				foreach (XmlAttribute CurrAttribute in xlgDataXmlDoc.DocumentElement.Attributes)
					root.SetAttribute(CurrAttribute.Name, CurrAttribute.Value);

				root.SetAttribute("Namespace", Namespace);
                root.SetAttribute("VDirName", VDirName);
                root.SetAttribute("DatabaseProvider", DataService.Instance.Provider.Name);
                root.SetAttribute("ConnectionStringName", DataService.Instance.Settings.Name);
                root.SetAttribute("OutputFolder", OutputFolder);
                root.SetAttribute("ProviderName", DataService.Instance.Settings.ProviderName);
                root.SetAttribute("Now", DateTime.Now.ToString("s"));
                root.SetAttribute("XlgInstanceID", XlgInstanceID.ToString().ToUpper());

                root.SetAttribute("MetXObjectName", DataService.Instance.MetXObjectName);
                root.SetAttribute("MetXAssemblyString", MetXAssemblyString);
                root.SetAttribute("MetXProviderAssemblyString", DataService.Instance.MetXProviderAssemblyString);
                root.SetAttribute("ProviderAssemblyString", DataService.Instance.ProviderAssemblyString);
                
                xmlDoc.AppendChild(root);
                foreach (XmlAttribute CurrAttribute in xlgDataXmlDoc.DocumentElement.Attributes)
                    root.SetAttribute(CurrAttribute.Name, CurrAttribute.Value);
                if (TablesToRender != null)
                    TablesXml(xmlDoc);
                if (StoredProceduresToRender != null)
                    StoredProceduresXml(xmlDoc);
                if (XslsToRender != null)
                    XslXml(xmlDoc);
                foreach (XmlElement CurrChild in xlgDataXmlDoc.DocumentElement.ChildNodes)
                    root.AppendChild(xmlDoc.ImportNode(CurrChild, true));

                // AddAttribute(root, "xmlDoc", xmlDoc.InnerXml.Replace("><", ">\n<")); 
                return xmlDoc;
            }
        }

        private string GetxlgPath(string Path)
        {
            return Path.Replace("/xsl/", "/").ToLower(); ;
        }

        XmlDocument XslXml(XmlDocument xmlDoc)
        {
            string RenderPath = XslsToRender.GetAttribute("Path");
            if (RenderPath == null || RenderPath.Length == 0)
                RenderPath = "~";

            UrlExtension = XslsToRender.GetAttribute("UrlExtension");
            if (UrlExtension == null || UrlExtension.Length == 0)
                UrlExtension = "aspx";
            else if (UrlExtension.StartsWith("."))
                UrlExtension = UrlExtension.Substring(1);

            XmlElement root = xmlDoc.DocumentElement;
            string path = Helper.VirtualPathToPhysical(RenderPath);

            XmlElement xmlXsls = xmlDoc.CreateElement("XslEndpoints");
            AddAttribute(xmlXsls, "VirtualPath", RenderPath);
            AddAttribute(xmlXsls, "xlgPath", GetxlgPath("/" + VDirName));
            AddAttribute(xmlXsls, "VirtualDir", string.Empty);
            AddAttribute(xmlXsls, "Path", path);
            AddAttribute(xmlXsls, "Folder", Token.Last(path, @"\"));
            root.AppendChild(xmlXsls);

            foreach (XmlElement CurrVirtual in XslsToRender.SelectNodes("Virtual"))
            {
                string xslFile = CurrVirtual.GetAttribute("Name");
                string classname = xslFile.Replace(" ", "_").Replace("/", ".");
                XmlElement xmlXsl = xmlDoc.CreateElement("XslEndpoint");

				if (CSKeywords.Contains(classname))
                    classname = "_" + classname;

				if (xmlDoc.SelectSingleNode("/*/Tables/Table[@ClassName=\"" + xml.AttributeEncode(classname) + "\"]") != null ||
                    xmlDoc.SelectSingleNode("/*/StoredProcedures[@ClassName=\"" + xml.AttributeEncode(classname) + "\"]") != null)
                    classname += "PageHandler";

                AddAttribute(xmlXsl, "xlgPath", GetxlgPath("/" + VDirName + "/" + xslFile + "." + UrlExtension));
                AddAttribute(xmlXsl, "VirtualPath", RenderPath + "/" + xslFile + "." + UrlExtension);
                AddAttribute(xmlXsl, "ClassName", classname);
                AddAttribute(xmlXsl, "Filepart", xslFile);
                AddAttribute(xmlXsl, "IsVirtual", "true");

                xmlXsls.AppendChild(xmlXsl);
            }

            ProcessXslPath(xmlDoc, RenderPath, "/" + VDirName.ToLower(), path, xmlXsls);

            return xmlDoc;
        }

        private void ProcessXslPath(XmlDocument xmlDoc, string RenderPath, string xlgPath, string path, XmlElement Parent)
        {
            foreach (string xslFile in Directory.GetFiles(path))
            {
                if (xslFile != null && xslFile.Length > 0 && Path.GetExtension(xslFile) == ".xsl" && !xslFile.EndsWith(".xlg.xsl") && isIncluded(XslsToRender, xslFile) && isIncluded(XslsToRender, RenderPath + "/" + Path.GetFileNameWithoutExtension(xslFile)))
                {
                    XmlElement xmlXsl = xmlDoc.CreateElement("XslEndpoint");
                    string classname = Path.GetFileNameWithoutExtension(xslFile).Replace(" ", "_");

					if (CSKeywords.Contains(classname))
						classname = "_" + classname;

					if (xmlDoc.SelectSingleNode("/*/Tables/Table[@ClassName=\"" + xml.AttributeEncode(classname) + "\"]") != null ||
                        xmlDoc.SelectSingleNode("/*/StoredProcedures[@ClassName=\"" + xml.AttributeEncode(classname) + "\"]") != null)
                        classname += "PageHandler";

                    AddAttribute(xmlXsl, "ClassName", classname);
                    AddAttribute(xmlXsl, "xlgPath", GetxlgPath(xlgPath + "/" + Path.GetFileNameWithoutExtension(xslFile) + "." + UrlExtension));
                    AddAttribute(xmlXsl, "FilePath", xslFile);
                    AddAttribute(xmlXsl, "Path", Path.GetDirectoryName(xslFile));
                    AddAttribute(xmlXsl, "Filename", Path.GetFileName(xslFile));
                    AddAttribute(xmlXsl, "Filepart", Path.GetFileNameWithoutExtension(xslFile));
                    AddAttribute(xmlXsl, "Extension", Path.GetExtension(xslFile));
                    AddAttribute(xmlXsl, "IsVirtual", "false");

                    Parent.AppendChild(xmlXsl);
                }
            }
            foreach (string xslFolder in Directory.GetDirectories(path))
            {
                string FolderName = Token.Last(xslFolder, @"\");
                if (isIncluded(XslsToRender, FolderName) && isIncluded(XslsToRender, RenderPath + "/" + FolderName))
                {
                    XmlElement xmlXsls = xmlDoc.CreateElement("XslEndpoints");
                    AddAttribute(xmlXsls, "VirtualPath", RenderPath + "/" + FolderName);
                    AddAttribute(xmlXsls, "xlgPath", GetxlgPath(xlgPath + "/" + FolderName));
                    AddAttribute(xmlXsls, "VirtualDir", FolderName);
                    AddAttribute(xmlXsls, "Path", xslFolder);
                    AddAttribute(xmlXsls, "Folder", FolderName);
                    Parent.AppendChild(xmlXsls);
                    ProcessXslPath(xmlDoc, RenderPath + "/" + FolderName, GetxlgPath(xlgPath + "/" + FolderName), xslFolder, xmlXsls);
                }
            }
        }
        
        XmlDocument TablesXml(XmlDocument xmlDoc)
        {
            XmlElement root = xmlDoc.DocumentElement;
            XmlElement xmlTables = xmlDoc.CreateElement("Tables");
            root.AppendChild(xmlTables);
			string[] tables = DataService.Instance.GetTables();
            foreach (string table in tables)
            {
                if (table != null && table.Length > 0 && isInList(table)) // && !isEnum(table))
                {
					TableSchema.Table tbl = null;
					try
					{
						tbl = DataService.Instance.GetTableSchema(table);
					}
					catch (Exception ex)
					{
						if (GUI != null)
						{
							switch (MessageBox.Show((IWin32Window) GUI, "Unable to get a schema for table: " + table + "\n\n\tAdd table to skip list and continue ?", "CONTINUE ?", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
							{
								case DialogResult.Yes:
									AddElement(TablesToRender, "Exclude", "Name", table);
									break;

								case DialogResult.No:
									break;
							}
						}
						tbl = null;
					}

					if (tbl != null)
					{
						XmlElement xmlTable = xmlDoc.CreateElement("Table");
						AddAttribute(xmlTable, "TableName", tbl.Name);
						AddAttribute(xmlTable, "ClassName", GetClassName(tbl.Name));
						if (tbl.PrimaryKey != null)
							AddAttribute(xmlTable, "PrimaryKeyColumnName", GetProperName(tbl.Name, tbl.PrimaryKey.ColumnName, "Field"));
						else
							AddAttribute(xmlTable, "PrimaryKeyColumnName", string.Empty);

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
							AddAttribute(xmlColumn, "VBVariableType", GetVBVariableType(col.DataType));
							AddAttribute(xmlColumn, "AuditField", isAuditField(col.ColumnName).ToString());
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
										AddAttribute(xmlKeyColumn, "Related", col.Related);
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
								AddAttribute(xmlIndex, "SingleColumnIndex", (index.Columns.Count == 1 ? "True" : "False"));
								AddAttribute(xmlIndex, "PropertyName", GetProperName(tbl.Name, index.Name, "Index"));
								XmlElement xmlIndexColumns = xmlDoc.CreateElement("IndexColumns");
								xmlIndex.AppendChild(xmlIndexColumns);
								foreach (string IndexColumn in index.Columns)
								{
									XmlElement xmlIndexColumn = xmlDoc.CreateElement("IndexColumn");
									AddAttribute(xmlIndexColumn, "IndexColumnName", IndexColumn);
									AddAttribute(xmlIndexColumn, "PropertyName", GetProperName(tbl.Name, IndexColumn, "Field"));
									xmlIndexColumns.AppendChild(xmlIndexColumn);
								}
								xmlIndexes.AppendChild(xmlIndex);
							}
						}
						xmlTables.AppendChild(xmlTable);
					}
                }
            }
            return xmlDoc;
        }
        XmlDocument StoredProceduresXml(XmlDocument xmlDoc)
        {
            //get the SP list from the DB
            string[] SPs = DataService.Instance.GetSPList();
            IDataReader paramReader = null;

            XmlElement root = xmlDoc.DocumentElement;

            XmlElement xmlStoredProcedures = xmlDoc.CreateElement("StoredProcedures");
            AddAttribute(xmlStoredProcedures, "ClassName", spClassName);
            root.AppendChild(xmlStoredProcedures);

            foreach (string spName in SPs)
            {
                // Make sure there is a stored proc to process 
                //  (is blank when there are no stored procedures in the database)
                if (spName.Length > 0 && !spName.StartsWith("dt_") && isIncluded(StoredProceduresToRender, spName))
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

						if (CSKeywords.Contains(paramName))
							paramName = "_" + paramName;

						bool IsInput = false, IsOutput = false;
                        switch (paramReader["ParamType"].ToString())
                        {
                            case "IN": case "1":  IsInput = true; break;
                            case "OUT": case "2": IsOutput = true; break;
                            case "": IsInput = true; break;
                            default: IsInput = true; IsOutput = true; break;
                        }
                        AddAttribute(xmlParameter, "DataType", dbType.ToString());
                        AddAttribute(xmlParameter, "CSharpVariableType", GetCSharpVariableType(dbType));
                        AddAttribute(xmlParameter, "CovertToPart", GetConvertToPart(dbType));
                        AddAttribute(xmlParameter, "VBVariableType", GetVBVariableType(dbType));
                        AddAttribute(xmlParameter, "IsDotNetObject", GetIsDotNetObject(dbType).ToString());
                        AddAttribute(xmlParameter, "ParameterName", paramName);
                        AddAttribute(xmlParameter, "VariableName", GetProperName(paramName.Replace("@", string.Empty).Replace(" ", string.Empty)));
                        AddAttribute(xmlParameter, "IsInput", IsInput.ToString());
                        AddAttribute(xmlParameter, "IsOutput", IsOutput.ToString());
                        xmlParameters.AppendChild(xmlParameter);
                    }
                    paramReader.Close();
                    xmlStoredProcedures.AppendChild(xmlStoredProcedure);
                }
            }
            return xmlDoc;
        }

        #region "Support Functions"
        bool isInList(string tableName)
        {
            return tableName != "dtproperties" && isIncluded(TablesToRender, tableName);
        }

        bool isExcluded(XmlElement ToCheck, string ToFind)
        {
            bool ret = false;
            if (ToCheck != null)
            {
				if (ToFind.EndsWith("*"))
				{
					ToFind = ToFind.Substring(0, ToFind.Length - 1);
					ret = (ToCheck.SelectSingleNode(
						"Exclude[@Name='*" +
						"' or starts-with(@Name,'" + ToFind +
						"') or starts-with(@Name,'" + Path.GetFileName(ToFind) +
						"') or starts-with(@Name,'" + Path.GetFileNameWithoutExtension(ToFind) + "')]") != null);
				}
				else
				{
					if (ToCheck.SelectSingleNode("Include[@Name='" + ToFind + "']") != null)
					{
						// Specifically included
						ret = false;
					}
					else
						ret = (ToCheck.SelectSingleNode(
							"Exclude[@Name='*" +
							"' or @Name='" + ToFind +
							"' or @Name='" + Path.GetFileName(ToFind) +
							"' or @Name='" + Path.GetFileNameWithoutExtension(ToFind) + "']") != null);
				}
            }
            return ret;
        }

        Dictionary<string, Regex> m_Patterns = new Dictionary<string, Regex>();

        bool isIncluded(XmlElement ToCheck, string ToFind)
        {
            if (ToCheck.ChildNodes.Count == 0)
                return true;
            if (isExcluded(ToCheck, ToFind))
                return false;
            if (ToCheck.SelectSingleNode("Include[@Name=\"*\"]") != null)
                return true;

            XmlNodeList xmlNodeList = ToCheck.SelectNodes("Include");
            if (xmlNodeList == null) { return false; }
            
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
                    regex = m_Patterns[name.Value];
                if (regex != null) { return regex.IsMatch(ToFind); }
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

        bool isAuditField(string colName)
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
        /// <param name="TableName">The name of the table, stored procedure, etc to translate</param>
        /// <returns>The translated class name</returns>
        public static string GetClassName(string TableName)
        {
            string className = GetProperName(TableName.Replace(" ", string.Empty));
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

        public static string UnderscoreToCamelcase(string ToConvert)
        {
            if (ToConvert != null && ToConvert.Length > 1)
            {
                ToConvert = ToConvert.Replace("$", "_");
                ToConvert = ToConvert.Replace("#", "_");
                ToConvert = ToConvert.Replace("!", "_");
                ToConvert = ToConvert.Replace("@", "_");
                ToConvert = ToConvert.Replace("%", "_");
                ToConvert = ToConvert.Replace("^", "_");
                ToConvert = ToConvert.Replace("&", "_");
                ToConvert = ToConvert.Replace("*", "_");
                ToConvert = ToConvert.Replace("(", "_");
                ToConvert = ToConvert.Replace(")", "_");

                if (ToConvert.ToLower().StartsWith("tb"))
                    ToConvert = ToConvert.Substring(2);

                ToConvert = ToConvert.Substring(0, 1).ToUpper() + ToConvert.Substring(1, ToConvert.Length - 1);

                ToConvert = ToConvert.Replace("____", "_")
                    .Replace("___", "_")
                    .Replace("__", "_")
                    .Replace("__", "_")
                    .Replace("__", "_");

                while (ToConvert.IndexOf("_") > -1)
                {
                    string af = Token.After(ToConvert, 1, "_");
                    if (af.Length > 0)
                    {
                        af = af[0].ToString().ToUpper() + af.Substring(1);
                        ToConvert = Token.First(ToConvert, "_") + af;
                    }
                    else
                        ToConvert = Token.First(ToConvert, "_");
                }
                if (ToConvert == "Type")
                    ToConvert = "TypeTable";
                if (ToConvert[0] >= '0' && ToConvert[0] <= '9')
                    ToConvert = "_" + ToConvert;
            }
            return ToConvert;
        }

        /// <summary>Simplified way of adding an XmlElement to a XmlDocument</summary>
        /// <param name="Target">The XmlDocment to add the Element onto</param>
        /// <param name="ElementName">The node name of the element</param>
        /// <param name="AttributeName">Name of an attribute to add</param>
        /// <param name="AttributeValue">Value of the attribute</param>
        /// <returns>The XmlElement added</returns>
        public XmlElement AddElement(XmlElement Target, string ElementName, string AttributeName, string AttributeValue)
        {
            if (Target == null) return null;
            XmlElement x = Target.OwnerDocument.CreateElement(ElementName);
            AddAttribute(x, AttributeName, AttributeValue);
            Target.AppendChild(x);
            return x;
        }

        /// <summary>Simplified way of adding an attribute to a XmlElement</summary>
        /// <param name="Target">The XmlElement to add the attribute to</param>
        /// <param name="AttributeName">The name of the attribute to add</param>
        /// <param name="AttributeValue">The value of the attribute to add</param>
        public void AddAttribute(XmlElement Target, string AttributeName, string AttributeValue)
        {
            if (Target == null) return;
            XmlAttribute ret = Target.OwnerDocument.CreateAttribute(AttributeName);
            ret.Value = AttributeValue;
            Target.Attributes.Append(ret);
        }

        // Anytime a database column is named any of these words, it causes a code issue. 
        //  Make sure a suffix is added to property names in these cases
        static List<string> TypeNames = new List<string>(new string[] { "guid", "int", "string", "timespan", "double", "single", "float", "decimal", "array" });

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <param name="sIn">The string to proper case</param>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string TableName, string FieldName, string Suffix)
        {
            if (FieldName != null && FieldName.Length > 1)
            {
                string propertyName = UnderscoreToCamelcase(FieldName);
                string cleanTableName = UnderscoreToCamelcase(TableName);
                string classTableName = GetClassName(TableName); 
                
                if (propertyName.EndsWith("TypeCode")) propertyName = propertyName.Substring(0, propertyName.Length - 4);
				if (TableName == FieldName || TableName == propertyName || cleanTableName == FieldName || cleanTableName == propertyName || propertyName == "TableName" || TypeNames.Contains(propertyName.ToLower()))
                    propertyName += Suffix;
                else if (classTableName == FieldName || classTableName == propertyName || TypeNames.Contains(classTableName.ToLower()))
                    propertyName += Suffix;
                return propertyName;
            }
            return FieldName;
        }

        /// <summary>Generates a proper case representation of a string (so "fred" becomes "Fred")</summary>
        /// <param name="sIn">The string to proper case</param>
        /// <returns>The proper case translation</returns>
        public static string GetProperName(string FieldName)
        {
            if (FieldName != null && FieldName.Length > 1)
            {
                string propertyName = UnderscoreToCamelcase(FieldName);
                if (propertyName.EndsWith("TypeCode")) propertyName = propertyName.Substring(0, propertyName.Length - 4);
                return propertyName;
            }
            return FieldName;
        }

        /// <summary>Translates a DbType into the VB.NET equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The VB.NET type string</returns>
        public static string GetVBVariableType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "String";
                case DbType.AnsiStringFixedLength: return "String";
                case DbType.Binary: return "Byte()";
                case DbType.Boolean: return "Bool";
                case DbType.Byte: return "Byte";
                case DbType.Currency: return "Decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                case DbType.Decimal: return "Decimal";
                case DbType.Double: return "Double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "Short";
                case DbType.Int32: return "Int";
                case DbType.Int64: return "Long";
                case DbType.Object: return "Object";
                case DbType.SByte: return "sbyte";
                case DbType.Single: return "Float";
                case DbType.String: return "String";
                case DbType.StringFixedLength: return "String";
                case DbType.Time: return "TimeSpan";
                case DbType.UInt16: return "UShort";
                case DbType.UInt32: return "UInt";
                case DbType.UInt64: return "ULong";
                case DbType.VarNumeric: return "decimal";
                default: return "String";
            }
        }

        /// <summary>Translates a DbType into the C# equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The C# type string</returns>
        public static string GetCSharpVariableType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool";
                case DbType.Byte: return "byte";
                case DbType.Currency: return "decimal";
                case DbType.Date: return "DateTime";
                case DbType.DateTime: return "DateTime";
                case DbType.Decimal: return "decimal";
                case DbType.Double: return "double";
                case DbType.Guid: return "Guid";
                case DbType.Int16: return "short";
                case DbType.Int32: return "int";
                case DbType.Int64: return "long";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte";
                case DbType.Single: return "float";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan";
                case DbType.UInt16: return "ushort";
                case DbType.UInt32: return "uint";
                case DbType.UInt64: return "ulong";
                case DbType.VarNumeric: return "decimal";
                default: return "string";
            }
        }

        /// <summary>Translates a DbType into the C# equivalent type (so "Currency" becomes "Decimal")</summary>
        /// <param name="dbType">The DbType to convert</param>
        /// <returns>The C# type string</returns>
        public static bool GetIsDotNetObject(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Boolean: return false;
                case DbType.Byte: return false;
                case DbType.Currency: return false;
                case DbType.Date: return false;
                case DbType.DateTime: return false;
                case DbType.Decimal: return false;
                case DbType.Double: return false;
                case DbType.Int16: return false;
                case DbType.Int32: return false;
                case DbType.Int64: return false;
                case DbType.SByte: return false;
                case DbType.Single: return false;
                case DbType.Time: return false;
                case DbType.UInt16: return false;
                case DbType.UInt32: return false;
                case DbType.UInt64: return false;
                case DbType.VarNumeric: return false;
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
                case DbType.AnsiString: return "Worker.nzString";
                case DbType.AnsiStringFixedLength: return "Worker.nzString";
                case DbType.Binary: return "Worker.nzByteArray";
                case DbType.Boolean: return "Worker.nzBoolean";
                case DbType.Byte: return "Worker.nzByte";
                case DbType.Currency: return "Worker.nzDecimal";
                case DbType.Date: return "Worker.nzDateTime";
                case DbType.DateTime: return "Worker.nzDateTime";
                case DbType.Decimal: return "Worker.nzDecimal";
                case DbType.Double: return "Worker.nzDouble";
                case DbType.Guid: return "Worker.nzGuid";
                case DbType.Int16: return "Worker.nzShort";
                case DbType.Int32: return "Worker.nzInteger";
                case DbType.Int64: return "Worker.nzLong";
                case DbType.Object: return string.Empty;
                case DbType.SByte: return "Worker.nzSByte";
                case DbType.Single: return "Worker.nzFloat";
                case DbType.String: return "Worker.nzString";
                case DbType.StringFixedLength: return "Worker.nzString";
                case DbType.Time: return "Worker.nzTimeSpan";
                case DbType.UInt16: return "Worker.nzUShort";
                case DbType.UInt32: return "Worker.nzUInt";
                case DbType.UInt64: return "Worker.nzULong";
                case DbType.VarNumeric: return "Worker.nzDecimal";
                default: return "Worker.nzString";
            }
        }
        #endregion
    }
}

