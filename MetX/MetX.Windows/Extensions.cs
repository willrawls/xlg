using System;

namespace MetX.Windows
{
    public static class Extensions
    {
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            // NOTE: This cannot be in a .net standard library
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out object translated))
            {
                return (TEnumOut?) translated ?? default(TEnumOut);
            }

            return default;
        }
    }
}