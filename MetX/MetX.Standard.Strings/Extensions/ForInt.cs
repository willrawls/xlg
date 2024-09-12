using System.Diagnostics;

namespace MetX.Standard.Strings.Extensions
{
    public static class ForInt
    {
        public static int InRange(this int x, int lo, int hi)
        {
            Debug.Assert(lo <= hi);
            return x < lo ? lo : x > hi ? hi : x;
        }

        public static bool IsInRange(this int x, int lo, int hi)
        {
            return x >= lo && x <= hi;
        }
    }
}