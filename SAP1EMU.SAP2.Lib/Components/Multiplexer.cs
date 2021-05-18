namespace SAP1EMU.SAP2.Lib.Components
{
    class Multiplexer
    {
        private static Multiplexer _instance; // Singleton Pattern

        public void PassThroughToBus(string bits, bool isUpperByte = false, bool clearByte = true)
        {
            string test = Wbus.Instance().Value[8..];

            // If we want solely the 8 bit value to go out on bus and get rid of the other bytes value in bus
            if (clearByte)
            {
                if(isUpperByte)
                {
                    Wbus.Instance().Value = bits.PadRight(16, '0');
                }
                else
                {
                    Wbus.Instance().Value = bits.PadLeft(16, '0');
                }
            }
            else
            {
                Wbus.Instance().Value = isUpperByte ? bits + Wbus.Instance().Value[8..] : Wbus.Instance().Value[0..8] + bits;
            }
        }

        public string PassThroughToRegister(bool isUpperByte = false)
        {
            return isUpperByte ? Wbus.Instance().Value[0..8] : Wbus.Instance().Value[8..];
        }

        public static Multiplexer Instance()
        {
            if (_instance == null)
            {
                _instance = new Multiplexer();
            }
            return _instance;
        }
    }
}
