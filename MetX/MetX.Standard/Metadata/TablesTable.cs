namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TablesTable {
        
        private ColumnsColumn[][] columnsField;
        
        private TablesTableKeysKey[][] keysField;
        
        private TablesTableIndexesIndex[][] indexesField;
        
        private string tableNameField;
        
        private string classNameField;
        
        private string primaryKeyColumnNameField;
        
        private string rowCountField;
        
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
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Key", typeof(TablesTableKeysKey), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TablesTableKeysKey[][] Keys {
            get {
                return this.keysField;
            }
            set {
                this.keysField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Index", typeof(TablesTableIndexesIndex), Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TablesTableIndexesIndex[][] Indexes {
            get {
                return this.indexesField;
            }
            set {
                this.indexesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TableName {
            get {
                return this.tableNameField;
            }
            set {
                this.tableNameField = value;
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PrimaryKeyColumnName {
            get {
                return this.primaryKeyColumnNameField;
            }
            set {
                this.primaryKeyColumnNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RowCount {
            get {
                return this.rowCountField;
            }
            set {
                this.rowCountField = value;
            }
        }
    }
}