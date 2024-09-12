using System.Collections.Generic;
using System.Text;

namespace MetX.Standard.Strings.Extensions
{
    public static class AsStringExtensions
    {
        public static string AsStringFromBytes(byte[]  arrInput)
        {
            
            List<List<string>> fred = new();
            var response = AsStringFromArray(fred);

            int i;
            var result = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++) result.Append(arrInput[i].ToString("X2"));
            return result.ToString();
        }

        public static string AsStringFromArray<T>(this IList<T> input, string separator = "\n", string postfix = "") 
        {
            var sb = new StringBuilder();

            foreach (var item in input)
            {
                sb.Append(item);
                if(separator.IsNotEmpty())
                    sb.Append(separator);
            }

            if (postfix.IsNotEmpty())
                sb.Append(postfix);

            return sb.ToString();
        }

        /*
        public static string AsStringFromArrayOfArrays<TOuterArray, TInnerArray>(this IList<TOuterArray> outList, string separator = "\n", string postfix = "") 
        {
            var sb = new StringBuilder();

            foreach (TOuterArray item in outList)
            {
                if(item is TInnerArray)
                sb.Append(item);
                if(separator.IsNotEmpty())
                    sb.Append(separator);
            }

            if (postfix.IsNotEmpty())
                sb.Append(postfix);

            return sb.ToString();
        }
    */
    }
}