using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MetX;
using MetX.Library;

namespace MetX
{
    public class ClipScriptProcessor : IProcessForClipScript
    {
        public bool ProcessLine(StringBuilder sb, string line, int number, int lineCount, Dictionary<string, string> d)
        {
            if (string.IsNullOrEmpty(line) && number > -1) return true;

//~~ProcessLine~~//

            return true;
        }
    }
}