using System;

namespace MetX.Standard.Generation
{
    public enum GenFramework
    {
        Unknown,
        Net50Windows,
        Net50,
        Core31,
        Standard20
    }

    public static class ForGenFramework
    {
        public static string ToTargetFramework(this GenFramework target)
        {
            switch(target)
            {
                case GenFramework.Net50Windows: return "net5.0-windows";
                case GenFramework.Net50: return "net5.0";
                case GenFramework.Core31: return "core3.1";
                case GenFramework.Standard20: return "netstandard2.0";
                case GenFramework.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
            return null;
        }
    }
}