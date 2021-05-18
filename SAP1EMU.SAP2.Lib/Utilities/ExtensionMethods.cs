using System;
namespace SAP1EMU.SAP2.Lib.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsActiveLow(this string value)
        {
            return string.Equals(value, "0", StringComparison.Ordinal);
        }

        public static bool IsActiveHigh(this string value)
        {
            return string.Equals(value, "1", StringComparison.Ordinal);
        }
    }
}
