namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class xlgDoc {
        
        private Tables[] tablesField;
        
        private StoredProcedures[] storedProceduresField;
        
        private XslEndpoints[] xslEndpointsField;
        
        private xlgDocRender[] renderField;
        
        private string includeNamespaceField;
        
        private string connectionStringNameField;
        
        private string namespaceField;
        
        private string vDirNameField;
        
        private string databaseProviderField;
        
        private string providerNameField;
        
        private string metXObjectNameField;
        
        private string metXProviderAssemblyStringField;
        
        private string providerAssemblyStringField;
        
        private string outputFolderField;
        
        private string nowField;
        
        private string xlgInstanceIDField;
        
        private string metXAssemblyStringField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Tables")]
        public Tables[] Tables {
            get {
                return this.tablesField;
            }
            set {
                this.tablesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("StoredProcedures")]
        public StoredProcedures[] StoredProcedures {
            get {
                return this.storedProceduresField;
            }
            set {
                this.storedProceduresField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XslEndpoints")]
        public XslEndpoints[] XslEndpoints {
            get {
                return this.xslEndpointsField;
            }
            set {
                this.xslEndpointsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Render", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public xlgDocRender[] Render {
            get {
                return this.renderField;
            }
            set {
                this.renderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IncludeNamespace {
            get {
                return this.includeNamespaceField;
            }
            set {
                this.includeNamespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ConnectionStringName {
            get {
                return this.connectionStringNameField;
            }
            set {
                this.connectionStringNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Namespace {
            get {
                return this.namespaceField;
            }
            set {
                this.namespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VDirName {
            get {
                return this.vDirNameField;
            }
            set {
                this.vDirNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DatabaseProvider {
            get {
                return this.databaseProviderField;
            }
            set {
                this.databaseProviderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ProviderName {
            get {
                return this.providerNameField;
            }
            set {
                this.providerNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MetXObjectName {
            get {
                return this.metXObjectNameField;
            }
            set {
                this.metXObjectNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MetXProviderAssemblyString {
            get {
                return this.metXProviderAssemblyStringField;
            }
            set {
                this.metXProviderAssemblyStringField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ProviderAssemblyString {
            get {
                return this.providerAssemblyStringField;
            }
            set {
                this.providerAssemblyStringField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OutputFolder {
            get {
                return this.outputFolderField;
            }
            set {
                this.outputFolderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Now {
            get {
                return this.nowField;
            }
            set {
                this.nowField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XlgInstanceID {
            get {
                return this.xlgInstanceIDField;
            }
            set {
                this.xlgInstanceIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MetXAssemblyString {
            get {
                return this.metXAssemblyStringField;
            }
            set {
                this.metXAssemblyStringField = value;
            }
        }
    }
}