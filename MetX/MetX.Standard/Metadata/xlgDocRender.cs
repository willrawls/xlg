using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class xlgDocRender
    {
        [XmlElement("Xsls", Form = XmlSchemaForm.Unqualified)]
        public List<xlgDocRenderXsls> Xsls;

        [XmlElement("Tables")] public List<Tables> Tables;
        [XmlElement("StoredProcedures")] public List<StoredProcedures> StoredProcedures;
    }
}