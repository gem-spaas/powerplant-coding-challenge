namespace Gem.PowerplantChallenge.Service.Models;

public abstract class Powerplant
{
    public string Name { get; set; }
    public double Efficiency { get; set; }
    public double PMin { get; set; }
    public double PMax { get; set; }
    public double PActual { get; set; }
    public double Cost { get; set; }

    public Powerplant(string name, double efficiency, double pMin, double pMax, double cost)
    {
        Name = name;
        Efficiency = efficiency;
        PMin = pMin;
        PMax = pMax;
        Cost = cost/efficiency;
    }

    public void SetPActualBasedOnNeeds(double powerNeeded)
    {
        if (powerNeeded > PMax)
        {
            PActual = PMax;
            return;
        }
        PActual = PMin > powerNeeded ? PMin : powerNeeded;
    }
}