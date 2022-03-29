using System.Collections.Generic;
using MetX.Standard.Library.Extensions;

namespace MetX.Standard.Primary.Scripts
{
    public class GenArea
    {
        public readonly IList<string> Lines;
        public readonly string Name;
        public readonly int Indent;

        public GenArea(string name, int indent, string lines = null)
        {
            Name = name;
            Indent = indent;
            Lines = string.IsNullOrEmpty(lines) ? new List<string>() : lines.LineList();
        }

        public GenArea(string name)
        {
            Name = name;
            Indent = 12;
            Lines = new List<string>();
        }

        public override string ToString()
        {
            return Lines.IsEmpty() ? "" : string.Join("\n", Lines);
        }
    }
}