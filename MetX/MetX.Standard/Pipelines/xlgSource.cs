using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using MetX.Standard.Data;
using MetX.Standard.Interfaces;
using MetX.Standard.Library;

namespace MetX.Standard.Pipelines
{
    /// <summary>
    /// Represents a library to generate
    /// </summary>
    [Serializable, XmlType(Namespace = "", AnonymousType = true)]
    public class XlgSource
    {
        [XmlAttribute]
        public string BasePath;
        [XmlAttribute]
        public string ParentNamespace;
        [XmlAttribute]
        public string ConnectionName;
        [XmlAttribute]
        public string DisplayName;

        [XmlAttribute]
        public string XlgDocFilename;
        [XmlAttribute]
        public string XslFilename;
        //[XmlAttribute] public string ConfigFilename;
        [XmlAttribute]
        public string OutputFilename;
        [XmlAttribute]
        public string OutputXml;

        [XmlAttribute]
        public string ConnectionString;
        [XmlAttribute]
        public string ProviderName;

        [XmlAttribute]
        public bool Selected;

        [XmlAttribute]
        public DateTime DateCreated;
        [XmlAttribute]
        public DateTime DateModified;
        [XmlAttribute]
        public DateTime LastGenerated;
        [XmlAttribute]
        public DateTime LastRegenerated;
        [XmlAttribute]
        public Guid LastXlgInstanceId;

        [XmlAttribute]
        public bool RegenerateOnly;
        [XmlAttribute]
        public string SqlToXml;

        private bool _mGenInProgress;
        private object _mSyncRoot = new();

        public XmlDocument LoadXlgDoc()
        {
            var ret = new XmlDocument();
            ret.Load(OutputXml);
            return ret;
        }

        [XmlIgnore]
        public string OutputPath
        {
            get
            {
                if (!string.IsNullOrEmpty(OutputFilename))
                    return OutputFilename.TokensBeforeLast(@"\") + @"\";
                return BasePath + ConnectionName;
            }
        }

        private class OpParams
        {
            public int Op;
            public IGenerationHost Gui;

            public OpParams(int op, IGenerationHost gui)
            {
                Op = op;
                Gui = gui;
            }
        }

        private void InternalOp(object @params) { var o = (OpParams)@params; if (o.Op == 1) Regenerate(o.Gui); else Generate(o.Gui); }

        public void RegenerateAsynch(IGenerationHost gui) { ThreadPool.QueueUserWorkItem(InternalOp, new OpParams(1, gui)); }

        public void GenerateAsynch(IGenerationHost gui) { ThreadPool.QueueUserWorkItem(InternalOp, new OpParams(2, gui)); }

        public int Regenerate(IGenerationHost gui)
        {
            if (_mGenInProgress) return 0;
            lock (_mSyncRoot)
            {
                if (_mGenInProgress) return 0;
                _mGenInProgress = true;
                var originalDirectory = Environment.CurrentDirectory;
                try
                {
                    Environment.CurrentDirectory = OutputPath;
                    if (string.IsNullOrEmpty(XlgDocFilename))
                        XlgDocFilename = OutputPath + ConnectionName + ".xlgd";
                    var gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, gui);
                    File.WriteAllText(OutputFilename, gen.RegenerateCode(LoadXlgDoc()));
                    LastRegenerated = DateTime.Now;
                    return 1;
                }
                catch (Exception ex)
                {
                    gui.MessageBox.Show(ex.ToString());
                }
                finally
                {
                    _mGenInProgress = false;
                    Environment.CurrentDirectory = originalDirectory;
                }
            }
            return -1;
        }

        public int Generate(IGenerationHost gui)
        {
            if (_mGenInProgress) return 0;
            lock (_mSyncRoot)
            {
                if (_mGenInProgress) return 0;
                _mGenInProgress = true;
                var originalDirectory = Environment.CurrentDirectory;
                try
                {
                    if (string.IsNullOrEmpty(XlgDocFilename))
                        XlgDocFilename = OutputPath + ConnectionName + ".xlgd";
                    DataService.Instance = DataService.GetDataServiceManually(ConnectionName, ConnectionString, ProviderName);
                    CodeGenerator gen = null;
                    StringBuilder sb;
                    string output;
                    Environment.CurrentDirectory = OutputPath;
                    switch (DataService.Instance.ProviderType)
                    {
                        case ProviderTypeEnum.DataAndGather:
                            if (string.IsNullOrEmpty(SqlToXml))
                            {
                                gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, gui)
                                {
                                    OutputFolder = IO.FileSystem.InsureFolderExists(gui, OutputFilename, true)
                                };
                                if (string.IsNullOrEmpty(gen.OutputFolder))
                                    return -1;  // User chose not to create output folder

                                var generatedCode = gen.GenerateCode();
                                if (generatedCode == null)
                                    return -1;
                                File.WriteAllText(OutputFilename, generatedCode);
                            }
                            else
                            {
                                sb = new StringBuilder();
                                DataService.Instance.Gatherer.GatherNow(sb, new[] { ConnectionName, ConnectionString, SqlToXml });
                                output = sb.ToString();
                                if (output.StartsWith("<?xml "))
                                {
                                    gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, gui);
                                    gen.CodeXmlDocument = new XmlDocument();
                                    gen.CodeXmlDocument.LoadXml(output);
                                    gen.CodeXmlDocument.Save(OutputXml);

                                    var generatedCode = gen.RegenerateCode(gen.CodeXmlDocument);
                                    if (string.IsNullOrEmpty(generatedCode))
                                        return -1;
                                    File.WriteAllText(OutputFilename, generatedCode);
                                }
                                else
                                {
                                    File.WriteAllText(OutputFilename, output);
                                }
                                LastRegenerated = DateTime.Now;
                            }
                            break;

                        case ProviderTypeEnum.Data:
                            gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, gui);
                            gen.OutputFolder = IO.FileSystem.InsureFolderExists(gui, OutputFilename, true);
                            if (string.IsNullOrEmpty(gen.OutputFolder))
                                return -1;  // User chose not to create output folder
                            File.WriteAllText(OutputFilename, gen.GenerateCode());
                            break;

                        case ProviderTypeEnum.Gather:
                            sb = new StringBuilder();
                            DataService.Instance.Gatherer.GatherNow(sb, new[] { ConnectionName, ConnectionString, SqlToXml });
                            output = sb.ToString();
                            if (output.StartsWith("<?xml "))
                            {
                                gen = new CodeGenerator(XlgDocFilename, XslFilename, OutputPath, gui);
                                gen.CodeXmlDocument = new XmlDocument();
                                gen.CodeXmlDocument.LoadXml(output);
                                File.WriteAllText(OutputFilename, gen.RegenerateCode(gen.CodeXmlDocument));
                            }
                            else
                            {
                                File.WriteAllText(OutputFilename, output);
                            }
                            LastRegenerated = DateTime.Now;
                            break;
                    }

                    if (gen != null)
                    {
                        if (string.IsNullOrEmpty(OutputXml))
                        {
                            OutputXml = Path.ChangeExtension(OutputFilename, ".xml");
                        }
                        using (var sw = File.CreateText(OutputXml ?? string.Empty))
                        {
                            using (var xw = Xml.Writer(sw))
                                gen.CodeXmlDocument.WriteTo(xw);
                        }
                    }
                    LastGenerated = DateTime.Now;
                    LastXlgInstanceId = gen != null ? gen.XlgInstanceId : Guid.NewGuid();
                    return 1;
                }
                catch (Exception ex)
                {
                    Host.MessageBox.Show(ex.ToString());
                }
                finally
                {
                    _mGenInProgress = false;
                    Environment.CurrentDirectory = originalDirectory;
                }
            }
            return -1;
        }

        public IGenerationHost Host { get; set; } = new GenerationHost();

        public XlgSource() { /* XmlSerializer */ }

        public XlgSource(string basePath, string parentNamespace, string displayName, string connectionName, string xlgDocFilename, string xslFilename, string outputFilename)
        {
            if (!basePath.EndsWith(@"\")) basePath += @"\";
            BasePath = basePath;
            ParentNamespace = parentNamespace;
            ConnectionName = connectionName;
            DisplayName = displayName;
            XlgDocFilename = xlgDocFilename;
            XslFilename = xslFilename;
            //this.ConfigFilename = ConfigFilename;
            OutputFilename = outputFilename;
            DateCreated = DateTime.Now;
        }

        public XlgSource(string basePath, string parentNamespace, string displayName, string connectionName, bool selected)
            : this(basePath, parentNamespace, displayName, connectionName)
        {
            Selected = selected;
        }

        public XlgSource(string basePath, string parentNamespace, string displayName, string connectionName)
        {
            if (!basePath.EndsWith(@"\")) basePath += @"\";
            BasePath = basePath;
            ParentNamespace = parentNamespace;
            DisplayName = displayName;
            ConnectionName = connectionName;
            XlgDocFilename = basePath + parentNamespace + "." + connectionName + @"\" + connectionName + ".xlgd";
            XslFilename = basePath + @"Support\app.xlg.xsl";
            //this.ConfigFilename = BasePath + @"Support\app.config";
            OutputFilename = basePath + parentNamespace + "." + connectionName + @"\" + connectionName + ".Glove.cs";
            DateCreated = DateTime.Now;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}