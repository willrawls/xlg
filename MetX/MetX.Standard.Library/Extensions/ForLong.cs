using System.Diagnostics;

namespace MetX.Standard.Library.Extensions
{
    public static class ForLong
    {
        public static long InRange(this long x, long lo, long hi)
        {
            Debug.Assert(lo <= hi);
            return x < lo ? lo : x > hi ? hi : x;
        }

        public static bool IsInRange(this long x, long lo, long hi)
        {
            return x >= lo && x <= hi;
        }
    }
}