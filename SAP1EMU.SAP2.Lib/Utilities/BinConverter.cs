﻿using System;

namespace SAP1EMU.SAP2.Lib.Utilities
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
        /// Converts a integer into a 16-bit binary string.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string IntToBin16(int i)
        {
            string val = Convert.ToString(i, 2);
            if (val.Length <= 16)
            {
                val = val.PadLeft(16, '0');
            }
            else
            {
                val = val.Substring(val.Length - 16, 16);
            }

            return val;
        }

        public static string IntToBinary(int value, int length)
        {
            string val = Convert.ToString(value, 2);
            if (val.Length <= length)
            {
                val = val.PadLeft(length, '0');
            }
            else
            {
                val = val.Substring(val.Length - length, length);
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