namespace GemSpaasPowerplant.Model
{
    public interface ICalculation
    {
        public IEnumerable<PowerLoad> GetProductionPlan(payload payload);
    }
}