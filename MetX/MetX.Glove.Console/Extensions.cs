using System;
using System.Windows.Forms;
using MetX.Standard.Pipelines;

namespace XLG.Pipeliner
{
    public static class Extensions
    {
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out object? translated))
            {
                return translated == null 
                    ? default(TEnumOut)
                    : (TEnumOut) translated;
            }

            return default(TEnumOut);
        }

    }
}