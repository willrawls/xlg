namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class xlgDocRender {
        
        private xlgDocRenderXsls[] xslsField;
        
        private Tables[] tablesField;
        
        private StoredProcedures[] storedProceduresField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Xsls", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public xlgDocRenderXsls[] Xsls {
            get {
                return this.xslsField;
            }
            set {
                this.xslsField = value;
            }
        }
        
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
    }
}