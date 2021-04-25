namespace MetX.Standard.Metadata
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class xlgDocRenderXsls {
        
        private Include[] includeField;
        
        private Exclude[] excludeField;
        
        private string pathField;
        
        private string urlExtensionField;
        
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
        public string UrlExtension {
            get {
                return this.urlExtensionField;
            }
            set {
                this.urlExtensionField = value;
            }
        }
    }
}