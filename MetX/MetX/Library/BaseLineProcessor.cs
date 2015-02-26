using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MetX.Library
{
    public abstract class BaseLineProcessor
    {
        public readonly StringBuilder Output = new StringBuilder();
        public int LineCount;
        public abstract void Start();
        public abstract bool ProcessLine(string line, int number);
        public abstract void Finish();
    }
}
