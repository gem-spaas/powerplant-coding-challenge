
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

    }
}