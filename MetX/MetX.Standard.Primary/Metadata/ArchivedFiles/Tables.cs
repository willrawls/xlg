using System;
using System.Collections.Generic;

namespace MetX.Standard.Metadata
{
    [Serializable]
    public class Tables : List<Table>
    {
        public List<Include> Include;
    }
}