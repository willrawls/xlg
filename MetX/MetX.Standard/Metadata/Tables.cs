using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MetX.Five.Metadata
{
    [Serializable]
    public class Tables
    {
        public List<Table> Table;
        public List<Include> Include;
    }
}