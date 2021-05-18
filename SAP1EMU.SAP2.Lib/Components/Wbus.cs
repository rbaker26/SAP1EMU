using System.Linq;

namespace SAP1EMU.SAP2.Lib.Components
{
    public class Wbus
    {
        private static Wbus _instance; // Singleton Pattern

        // Value on the bus
        public string Value { get; set; }

        private Wbus()
        {
            Value = string.Concat(Enumerable.Repeat('0', 16));
        }

        public static Wbus Instance()
        {
            if (_instance == null)
            {
                _instance = new Wbus();
            }
            return _instance;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}