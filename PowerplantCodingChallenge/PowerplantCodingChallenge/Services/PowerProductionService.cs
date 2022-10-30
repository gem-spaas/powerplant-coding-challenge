using PowerplantCodingChallenge.Models;

namespace PowerplantCodingChallenge.Services
{
    public class PowerProductionService : IPowerProductionService
    {
        public PowerProductionService()
        {
        }

        public Task<List<PayloadResponse>> GetPowerSupply(Payload payload)
        {
            List<PayloadResponse> listResult = new List<PayloadResponse>();
            listResult.Add(new PayloadResponse() { P = 0, Name = "X" });
            return Task.FromResult(listResult);
        }
    }
}
