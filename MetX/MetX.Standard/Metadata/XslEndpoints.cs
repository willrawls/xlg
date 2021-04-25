namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class XslEndpoints {
        
        private XslEndpoints[] xslEndpoints1Field;
        
        private string virtualPathField;
        
        private string xlgPathField;
        
        private string virtualDirField;
        
        private string pathField;
        
        private string folderField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XslEndpoints")]
        public XslEndpoints[] XslEndpoints1 {
            get {
                return this.xslEndpoints1Field;
            }
            set {
                this.xslEndpoints1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VirtualPath {
            get {
                return this.virtualPathField;
            }
            set {
                this.virtualPathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xlgPath {
            get {
                return this.xlgPathField;
            }
            set {
                this.xlgPathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VirtualDir {
            get {
                return this.virtualDirField;
            }
            set {
                this.virtualDirField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Path {
            get {
                return this.pathField;
            }
            set {
                this.pathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Folder {
            get {
                return this.folderField;
            }
            set {
                this.folderField = value;
            }
        }
    }
}