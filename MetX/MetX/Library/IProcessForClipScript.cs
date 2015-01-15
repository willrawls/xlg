using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MetX.Library
{
    public interface IProcessForClipScript
    {
        bool ProcessLine(StringBuilder sb, string line, int number);
    }
}
