using System.Collections.Generic;
using MetX.Library;

namespace MetX.Scripts
{
    public class GenArea
    {
        public readonly List<string> Lines = new List<string>();
        public readonly string Name;
        public int Indent = 12;

        public GenArea(string name, int indent, string lines = null)
        {
            Name = name;
            Indent = indent;
            if (!string.IsNullOrEmpty(lines))
            {
                Lines = lines.LineList();
            }
        }

        public GenArea(string name)
        {
            Name = name;
            Indent = 12;
        }
    }
}