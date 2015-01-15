using System;
using System.IO;
using System.Text;
using MetX;
using MetX.Library;

namespace MetX
{
    public class ClipScriptProcessor : IProcessForClipScript
    {
        public bool ProcessLine(StringBuilder sb, string line, int number)
        {
            if (string.IsNullOrEmpty(line)) return true;

            ~~ProcessLine~~

            return true;
        }
    }
}