using System;

namespace SAP1EMU.Lib.Utilities
{
    public static class BinConverter
    {
        //************************************************************************************************************************
        /// <summary>
        /// Converts a integer into a 8-bit binary string.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string IntToBin8(int i)
        {
            string val = Convert.ToString(i, 2);
            if (val.Length <= 8)
            {
                val = val.PadLeft(8, '0');
            }
            else
            {
                val = val.Substring(val.Length - 8, 8);
            }

            return val;
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        /// <summary>
        /// Converts a 8-bit binary string into an int
        /// </summary>
        /// <param name="s"></param>
        public static int Bin8ToInt(string s)
        {
            return (int)(Convert.ToUInt32(s, 2));
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        /// <summary>
        /// Converts a integer into a 8-bit binary string.
        /// </summary>
        /// <param name="i"></param>
        public static string IntToBin4(int i)
        {
            string val = Convert.ToString(i, 2);
            if (val.Length <= 4)
            {
                val = val.PadLeft(4, '0');
            }
            else
            {
                val = val.Substring(val.Length - 4, 4);
            }

            return val;
        }

        //************************************************************************************************************************

        //************************************************************************************************************************
        /// <summary>
        /// Converts a 8-bit binary string into an int
        /// </summary>
        /// <param name="s"></param>
        public static int Bin4ToInt(string s)
        {
            return Bin8ToInt(s);
        }

        //************************************************************************************************************************
    }
}