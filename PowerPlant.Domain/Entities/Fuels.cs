namespace PowerPlant.Domain.Entities;

public class Fuels
{
    public decimal GasPricePerMWh { get; set; }

    public decimal KerosinePricePerMWh { get; set; }

    public decimal Co2PricePerTon { get; set; }

    public decimal WindPercentage { get; set; }
}