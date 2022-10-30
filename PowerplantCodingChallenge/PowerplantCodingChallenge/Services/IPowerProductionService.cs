using PowerplantCodingChallenge.Models;


namespace PowerplantCodingChallenge.Services
{
    public interface IPowerProductionService
    {
        public Task<List<PayloadResponse>> GetPowerSupply(Payload payload);
    }
}