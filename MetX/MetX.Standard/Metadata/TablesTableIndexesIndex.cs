namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TablesTableIndexesIndex {
        
        private TablesTableIndexesIndexIndexColumnsIndexColumn[][] indexColumnsField;
        
        private string indexNameField;
        
        private string isClusteredField;
        
        private string singleColumnIndexField;
        
        private string propertyNameField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("IndexColumn", typeof(TablesTableIndexesIndexIndexColumnsIndexColumn), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TablesTableIndexesIndexIndexColumnsIndexColumn[][] IndexColumns {
            get {
                return this.indexColumnsField;
            }
            set {
                this.indexColumnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IndexName {
            get {
                return this.indexNameField;
            }
            set {
                this.indexNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IsClustered {
            get {
                return this.isClusteredField;
            }
            set {
                this.isClusteredField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SingleColumnIndex {
            get {
                return this.singleColumnIndexField;
            }
            set {
                this.singleColumnIndexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyName {
            get {
                return this.propertyNameField;
            }
            set {
                this.propertyNameField = value;
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