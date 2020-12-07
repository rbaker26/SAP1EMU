namespace SAP1EMU.SAP2.Assembler
{
    public class Label
    {
        public int LineNumber { get; set; }
        public string Name { get; set; }

        public bool IsLabelValid()
        {
            return (Name.Length >= 1 && Name.Length <= 6) && char.IsLetter(Name[0]);
        }
    }
}
