namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class NewDataSet {
        
        private object[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Columns", typeof(Columns))]
        [System.Xml.Serialization.XmlElementAttribute("Exclude", typeof(Exclude))]
        [System.Xml.Serialization.XmlElementAttribute("Include", typeof(Include))]
        [System.Xml.Serialization.XmlElementAttribute("StoredProcedures", typeof(StoredProcedures))]
        [System.Xml.Serialization.XmlElementAttribute("Tables", typeof(Tables))]
        [System.Xml.Serialization.XmlElementAttribute("XslEndpoints", typeof(XslEndpoints))]
        [System.Xml.Serialization.XmlElementAttribute("xlgDoc", typeof(xlgDoc))]
        public object[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
}