using System;
using Microsoft.CodeAnalysis;

namespace MetX.Scripts
{
    public static class ScriptExtensions
    {
        public static int Line(this Location location)
        {
            return location == null 
                ? 0 
                : location.GetMappedLineSpan().StartLinePosition.Line;
        }
    }
}