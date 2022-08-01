using System;

namespace MetX.Standard.XDString.Support;

internal static class Extensions
{
    internal static string AsString(this object value, string defaultValue = "")
    {
        if (value == null || value == DBNull.Value || value.Equals(string.Empty))
            return defaultValue;
        return value is Guid ? Convert.ToString(value) : value.ToString()?.Trim();
    }

    internal static bool IsEmpty(this string target)
    {
        return target == null || 0u >= (uint) target.Length;
    }

    internal static bool IsNotEmpty(this string target)
    {
        return !string.IsNullOrEmpty(target);
    }

    internal static string Left(this string target, int length)
    {
        if (string.IsNullOrEmpty(target) || length < 1) return string.Empty;
        return target.Length <= length
            ? target
            : target[..length];
    }

    internal static string Mid(this string target, int startAt)
    {
        if (string.IsNullOrEmpty(target) || startAt < 1 || startAt > target.Length) return string.Empty;
        return target[startAt..];
    }
}