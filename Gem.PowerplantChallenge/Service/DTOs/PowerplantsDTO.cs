namespace Gem.PowerplantChallenge.Service.DTOs;

public class PowerplantsWithLoadDTO
{
    public double Load { get; set; }
    public FuelDataDTO Fuels { get; set; }
    public IList<PowerplantDTO> Powerplants { get; set; }
}