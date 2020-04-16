using System;
using System.Collections.Generic;
using System.Text;

using SAP1EMU.Lib.Utilities;

namespace SAP1EMU.Lib.Components
{
    public class ALU 
    {
        //************************************************************************************************************************
        public static void Exec() 
        {

            //TODO - check control word and set wbus if nessicary 
        }
        //************************************************************************************************************************



        //************************************************************************************************************************
        public static string Compute(string AReg, string BReg)
        {
            int ia = BinConverter.Bin8ToInt(AReg);
            int ib = BinConverter.Bin8ToInt(BReg);

            int result;

            // TODO ADD
            if (true)
            {
                result = ia + ib;
            }
            //TODO SUB
            else
            {
                result = ia - ib;

            }

            string val = BinConverter.IntToBin8(result);

            return val ;
        }
        //************************************************************************************************************************


        


    }
}
