namespace powerplant_coding_challenge_implementation.Models
{
    public class PowerPlant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public float Efficiency { get; set; }
        public int PMin { get; set; }
        public int PMax { get; set; }
        public int PActual { get; set; }

        public PowerPlant(string name, string type, float efficiency, int pMin, int pMax, int pActual)
        {
            Name=name;
            Type=type;
            Efficiency=efficiency;
            PMin=pMin;
            PMax=pMax;
            PActual=pActual;
        }
    }
}