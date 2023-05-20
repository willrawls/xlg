using MetX._471.Strings.Generics;
using MetX._471.Strings.Interfaces;
using System;
using System.Collections.Generic;

namespace MetX._471.Strings.Extensions;

public static class AssociativeArrayExtensions
{

    public static List<string> DistinctList<T>(this AssocWithInheritedItems<T> items, Func<T, string> matchNameLambda, bool returnSorted = true)
        where T : IAssocItem, new()

    {
        if (items == null || items.Count == 0)
            return default;

        var matches = new List<string>();
        foreach (var item in items)
        {
            var matchName = matchNameLambda(item);
            if (!matches.Contains(matchName))
            {
                matches.Add(matchName);
            }
        }

        if (returnSorted)
            matches.Sort();

        return matches;
    }

    public static List<string> DistinctList<T>(this List<T> items, Func<T, string> matchNameLambda, bool returnSorted = true)
        where T : IAssocItem, new()

    {
        if (items == null || items.Count == 0)
            return default;

        var matches = new List<string>();
        foreach (var item in items)
        {
            var matchName = matchNameLambda(item);
            if (!matches.Contains(matchName))
            {
                matches.Add(matchName);
            }
        }

        if (returnSorted)
            matches.Sort();

        return matches;
    }

    public static List<string> DistinctList<T>(this AssocWithInheritedItems<T> items, Func<T, bool> includeIfTrueLambda, Func<T, string> matchNameLambda, bool returnSorted = true)
        where T : IAssocItem, new()

    {
        if (includeIfTrueLambda == null || items == null || items.Count == 0)
            return default;

        var matches = new List<string>();
        foreach (var item in items)
        {
            var matchName = matchNameLambda(item);
            if (includeIfTrueLambda(item) && !matches.Contains(matchName))
            {
                matches.Add(matchName);
            }
        }

        if (returnSorted)
            matches.Sort();

        return matches;
    }
}