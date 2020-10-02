namespace SAP1EMU.Lib.Components
{
    public class Flags
    {
        private static Flags _instance; // Singleton Pattern

        public byte Overflow;
        public byte Underflow;

        public void Clear()
        {
            Overflow = 0;
            Underflow = 0;
        }

        private Flags()
        {
            Overflow = 0;
            Underflow = 0;
        }

        public static Flags Instance()
        {
            if (_instance == null)
            {
                _instance = new Flags();
            }
            return _instance;
        }
    }
}