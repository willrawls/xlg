namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class StoredProcedures {
        
        private StoredProceduresStoredProcedure[] storedProcedureField;
        
        private Include[] includeField;
        
        private Exclude[] excludeField;
        
        private string classNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("StoredProcedure", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StoredProceduresStoredProcedure[] StoredProcedure {
            get {
                return this.storedProcedureField;
            }
            set {
                this.storedProcedureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Include")]
        public Include[] Include {
            get {
                return this.includeField;
            }
            set {
                this.includeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Exclude")]
        public Exclude[] Exclude {
            get {
                return this.excludeField;
            }
            set {
                this.excludeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ClassName {
            get {
                return this.classNameField;
            }
            set {
                this.classNameField = value;
            }
        }
    }
}