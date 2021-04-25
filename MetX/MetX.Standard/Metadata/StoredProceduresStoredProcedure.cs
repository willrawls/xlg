namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class StoredProceduresStoredProcedure {
        
        private StoredProceduresStoredProcedureParametersParameter[][] parametersField;
        
        private string storedProcedureNameField;
        
        private string methodNameField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Parameter", typeof(StoredProceduresStoredProcedureParametersParameter), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public StoredProceduresStoredProcedureParametersParameter[][] Parameters {
            get {
                return this.parametersField;
            }
            set {
                this.parametersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StoredProcedureName {
            get {
                return this.storedProcedureNameField;
            }
            set {
                this.storedProcedureNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MethodName {
            get {
                return this.methodNameField;
            }
            set {
                this.methodNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
    }
}