using System;

namespace MetX.Standard.Generation
{
    public static class ForGenFramework
    {
        public static string ToTargetFramework(this GenFramework target) =>
            target switch
            {
                GenFramework.Net50Windows => "net6.0-windows",
                GenFramework.Net50 => "net6.0",
                GenFramework.Core31 => "core3.1",
                GenFramework.Standard20 => "netstandard2.0",
                GenFramework.Standard21 => "netstandard2.1",
                GenFramework.Unknown => throw new ArgumentOutOfRangeException(nameof(target), target, null),
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
    }
}