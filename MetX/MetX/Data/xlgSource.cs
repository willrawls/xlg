using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;


namespace MetX.Data
{
	[Serializable, XmlRoot(Namespace="", IsNullable=false)]
	public class XlgSettings
	{
		static XmlSerializer xs = new XmlSerializer(typeof(XlgSettings));

		[XmlAttribute] public string Filename;
        [XmlAttribute] public string DefaultConnectionString;
        [XmlAttribute] public string DefaultProviderName;

		[XmlArray("Sources", Namespace="", IsNullable=false), XmlArrayItem("Source", Namespace="",IsNullable=false)]
		public List<xlgSource> Sources = new List<xlgSource>();

		[XmlIgnore]
		public System.Windows.Forms.Form GUI;

		public XlgSettings() { /* XmlSerilizer */ }

		public XlgSettings(System.Windows.Forms.Form GUI) { this.GUI = GUI; }

		public static XlgSettings FromXml(string xmldoc)
		{
			return (XlgSettings)xs.Deserialize(new StringReader(xmldoc));
		}

		public string OuterXml()
		{
			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
				xs.Serialize(sw, this);
			return sb.ToString();
		}

		public void Save()
		{
			File.WriteAllText(Filename, OuterXml());
		}

		public static XlgSettings Load(string Filename)
		{
			return FromXml(File.ReadAllText(Filename));
		}

		public int Generate(System.Windows.Forms.Form GUI)
		{
			int GenCount = 0;
			int LastGen = 0;
			foreach (xlgSource CurrSource in Sources)
			{
				if (CurrSource.Selected)
				{
                    if(CurrSource.RegenerateOnly)
                        LastGen = CurrSource.Regenerate(GUI);
                    else
					    LastGen = CurrSource.Generate(GUI);
					if (LastGen == -1)
						return -GenCount;
					GenCount++;
				}
			}
			return GenCount;
		}

		public int Regenerate(System.Windows.Forms.Form GUI)
		{
			int GenCount = 0;
			int LastGen = 0;
			foreach (xlgSource CurrSource in Sources)
			{
				if (CurrSource.Selected)
				{
					LastGen = CurrSource.Regenerate(GUI);
					if (LastGen == -1)
						return -GenCount;
					GenCount++;
				}
			}
			return GenCount;
		}
	}

	/// <summary>
	/// Represents a library to generate
	/// </summary>
	[Serializable, XmlType(Namespace="",AnonymousType=true)]
	public class xlgSource
	{
		[XmlAttribute] public string BasePath;
		[XmlAttribute] public string ParentNamespace;
		[XmlAttribute] public string ConnectionName;
		[XmlAttribute] public string DisplayName;

		[XmlAttribute] public string XlgDocFilename;
		[XmlAttribute] public string XslFilename;
		//[XmlAttribute] public string ConfigFilename;
		[XmlAttribute] public string OutputFilename;
        [XmlAttribute] public string OutputXml;

        [XmlAttribute] public string ConnectionString;
        [XmlAttribute] public string ProviderName;

		[XmlAttribute] public bool Selected;
		
		[XmlAttribute] public DateTime DateCreated;
		[XmlAttribute] public DateTime DateModified;
		[XmlAttribute] public DateTime LastGenerated;
		[XmlAttribute] public DateTime LastRegenerated;
        [XmlAttribute] public Guid     LastXlgInstanceID;

        [XmlAttribute] public bool     RegenerateOnly;
        [XmlAttribute] public string   SqlToXml;

		private bool GenInProgress;
		private object SyncRoot = new object();

		public XmlDocument LoadXlgDoc()
		{
			XmlDocument ret = new XmlDocument();
            ret.Load(OutputXml);
			return ret;
		}

        [XmlIgnore]
        public string OutputPath
        {
            get 
            {
                if(!string.IsNullOrEmpty(OutputFilename))
                    return Token.BeforeLast(OutputFilename, @"\") + @"\";
                return BasePath + ConnectionName;
            }
        }

		private class opParams
		{
			public int op;
			public System.Windows.Forms.Form GUI;
			public opParams(int op, System.Windows.Forms.Form GUI)
			{
				this.op = op;
				this.GUI = GUI;
			}
		}

		private void internalOp(object Params) { opParams o = (opParams)Params; if ((int)o.op == 1) Regenerate(o.GUI); else Generate(o.GUI); }
		public void RegenerateAsynch(System.Windows.Forms.Form GUI) { ThreadPool.QueueUserWorkItem(new WaitCallback(internalOp), new opParams(1, GUI)); }
		public void GenerateAsynch(System.Windows.Forms.Form GUI) { ThreadPool.QueueUserWorkItem(new WaitCallback(internalOp), new opParams(2, GUI)); }
		public int Regenerate(System.Windows.Forms.Form GUI)
		{
			if (GenInProgress) return 0;
			lock (SyncRoot)
			{
				if (GenInProgress) return 0;
				GenInProgress = true;
				try
				{
                    if (string.IsNullOrEmpty(XlgDocFilename))
                        XlgDocFilename = OutputPath + ConnectionName + ".xlgd";
                    CodeGenerator Gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, GUI);
					File.WriteAllText(OutputFilename, Gen.RegenerateCode(LoadXlgDoc()));
					LastRegenerated = DateTime.Now;
					return 1;
				}
				catch (Exception ex)
				{
					System.Windows.Forms.MessageBox.Show(ex.ToString());
				}
				finally
				{
					GenInProgress = false;
				}
			}
			return -1;
		}

		public int Generate(System.Windows.Forms.Form GUI)
		{
			if (GenInProgress) return 0;
			lock (SyncRoot)
			{
				if (GenInProgress) return 0;
				GenInProgress = true;
				try
				{
                    if (string.IsNullOrEmpty(XlgDocFilename))
                        XlgDocFilename = OutputPath + ConnectionName + ".xlgd";
                    DataService.Instance = DataService.GetDataServiceManually(ConnectionName, ConnectionString, ProviderName);
                    CodeGenerator Gen = null;
                    StringBuilder sb = null;
                    string Output = null;
                    switch (DataService.Instance.ProviderType)
                    {
                        case ProviderTypeEnum.DataAndGather:
                            if (string.IsNullOrEmpty(SqlToXml))
                            {
                                Gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, GUI);
                                Gen.OutputFolder = IO.FileSystem.InsureFolderExists(OutputFilename, true);
                                if (string.IsNullOrEmpty(Gen.OutputFolder))
                                    return -1;  // User chose not to create output folder
                                File.WriteAllText(OutputFilename, Gen.Code);
                            }
                            else
                            {
                                sb = new StringBuilder();
                                DataService.Instance.Gatherer.GatherNow(sb, new string[] { ConnectionName, ConnectionString, SqlToXml });
                                Output = sb.ToString();
                                if (Output.StartsWith("<?xml "))
                                {
									Gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, GUI);
                                    Gen.CodeXmlDocument = new XmlDocument();
                                    Gen.CodeXmlDocument.LoadXml(Output);
                                    Gen.CodeXmlDocument.Save(OutputXml);
                                    File.WriteAllText(OutputFilename, Gen.RegenerateCode(Gen.CodeXmlDocument));
                                }
                                else
                                {
                                    File.WriteAllText(OutputFilename, Output);
                                }
                                LastRegenerated = DateTime.Now;
                            }
                            break;

                        case ProviderTypeEnum.Data:
							Gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, GUI);
                            Gen.OutputFolder = IO.FileSystem.InsureFolderExists(OutputFilename, true);
                            if (string.IsNullOrEmpty(Gen.OutputFolder))
                                return -1;  // User chose not to create output folder
                            File.WriteAllText(OutputFilename, Gen.Code);
                            break;

                        case ProviderTypeEnum.Gather:
                            sb = new StringBuilder();
                            DataService.Instance.Gatherer.GatherNow(sb, new string[] { ConnectionName, ConnectionString, SqlToXml });
                            Output = sb.ToString();
                            if (Output.StartsWith("<?xml "))
                            {
								Gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, GUI);
                                Gen.CodeXmlDocument = new XmlDocument();
                                Gen.CodeXmlDocument.LoadXml(Output);
                                File.WriteAllText(OutputFilename, Gen.RegenerateCode(Gen.CodeXmlDocument));
                            }
                            else
                            {
                                File.WriteAllText(OutputFilename, Output);
                            }
                            LastRegenerated = DateTime.Now;
                            break;
                    }

                    if (Gen != null)
                    {
                        if (string.IsNullOrEmpty(OutputXml))
                        {
                            OutputXml = Path.ChangeExtension(OutputFilename, ".xml");
                        }
                        using (StreamWriter sw = File.CreateText(OutputXml))
                        {
                            using (XmlWriter xw = xml.Writer(sw))
                                Gen.CodeXmlDocument.WriteTo(xw);
                        }
                    }
					LastGenerated = DateTime.Now;
                    LastXlgInstanceID = (Gen != null ? Gen.XlgInstanceID : Guid.NewGuid() );
					return 1;
				}
				catch (Exception ex)
				{
					System.Windows.Forms.MessageBox.Show(ex.ToString());
				}
				finally
				{
					GenInProgress = false;
				}
			}
			return -1;
		}

		public xlgSource() { /* XmlSerializer */ }

        public xlgSource(string BasePath, string ParentNamespace, string DisplayName, string ConnectionName, string XlgDocFilename, string XslFilename, string ConfigFilename, string OutputFilename)
		{
			if (!BasePath.EndsWith(@"\")) BasePath += @"\";
			this.BasePath = BasePath;
			this.ParentNamespace = ParentNamespace;
			this.ConnectionName = ConnectionName;
			this.DisplayName = DisplayName;
            this.XlgDocFilename = XlgDocFilename;
			this.XslFilename = XslFilename;
			//this.ConfigFilename = ConfigFilename;
			this.OutputFilename = OutputFilename;
			DateCreated = DateTime.Now;
		}

		public xlgSource(string BasePath, string ParentNamespace, string DisplayName, string ConnectionName, bool Selected)
			: this(BasePath, ParentNamespace, DisplayName, ConnectionName)
		{
			this.Selected = Selected;
		}

		public xlgSource(string BasePath, string ParentNamespace, string DisplayName, string ConnectionName)
		{
			if (!BasePath.EndsWith(@"\")) BasePath += @"\";
			this.BasePath = BasePath;
			this.ParentNamespace = ParentNamespace;
			this.DisplayName = DisplayName;
			this.ConnectionName = ConnectionName;
            this.XlgDocFilename = BasePath + ParentNamespace + "." + ConnectionName + @"\" + ConnectionName + ".xlgd";
			this.XslFilename = BasePath + @"Support\app.xlg.xsl";
			//this.ConfigFilename = BasePath + @"Support\app.config";
			this.OutputFilename = BasePath + ParentNamespace + "." + ConnectionName + @"\" + ConnectionName + ".Glove.cs";
			DateCreated = DateTime.Now;
		}

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
