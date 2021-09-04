namespace MetX.Standard.Library
{
    public static class Extensions
    {
        public static string RemoveAll(this string target, string[] stringsToRemove)
        {
            if (target.IsEmpty())
                return string.Empty;
            if (stringsToRemove.IsEmpty())
                return target;

            foreach (var stringToRemove in stringsToRemove)
                target = target.Replace(stringToRemove, string.Empty);
            return target;
        }

        public static string RemoveAll(this string target, char[] charsToRemove)
        {
            if (target.IsEmpty())
                return string.Empty;
            if (charsToRemove.IsEmpty())
                return target;

            foreach (var charToRemove in charsToRemove)
                target = target.Replace(charToRemove.ToString(), string.Empty);
            return target;
        }

        public static string ReplaceAll(this string target, string[] stringsToReplace, string replacementText)
        {
            if (target.IsEmpty())
                return string.Empty;
            if (stringsToReplace.IsEmpty() || replacementText.IsEmpty())
                return target;

            foreach (var stringToReplace in stringsToReplace)
                target = target.Replace(stringToReplace, replacementText);
            return target;
        }
    }
}