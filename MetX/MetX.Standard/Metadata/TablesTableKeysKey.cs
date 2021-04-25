namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TablesTableKeysKey {
        
        private ColumnsColumn[][] columnsField;
        
        private string nameField;
        
        private string isPrimaryField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Column", typeof(ColumnsColumn), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ColumnsColumn[][] Columns {
            get {
                return this.columnsField;
            }
            set {
                this.columnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsPrimary {
            get {
                return this.isPrimaryField;
            }
            set {
                this.isPrimaryField = value;
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