namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Columns {
        
        private ColumnsColumn[] columnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ColumnsColumn[] Column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
            }
        }
    }
}