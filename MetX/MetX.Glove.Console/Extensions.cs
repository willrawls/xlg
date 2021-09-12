using System;

namespace XLG.Pipeliner
{
    public static class Extensions
    {
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out var translated))
            {
                return (TEnumOut?) translated ?? default;
            }

            return default;
        }

    }
}