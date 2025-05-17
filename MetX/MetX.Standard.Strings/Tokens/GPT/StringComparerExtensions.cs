using System;

namespace MetX.Standard.Strings.Tokens.GPT;

public static class StringComparerExtensions
{
    public static StringComparer FromComparison(this StringComparer _, StringComparison comparison)
    {
        return comparison switch
        {
            StringComparison.Ordinal => StringComparer.Ordinal,
            StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
            StringComparison.CurrentCulture => StringComparer.CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
            StringComparison.InvariantCulture => StringComparer.InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
            _ => StringComparer.OrdinalIgnoreCase
        };
    }
}