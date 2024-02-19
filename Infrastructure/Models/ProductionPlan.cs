namespace powerplant_coding_challenge.Infrastructure.Models
{
    public class ProductionPlan
    {
        public List<Plan> Plan { get; set; } = null!;        
    }

    public class Plan
    {
        public string Name { get; set; } = null!;

        private float _p;
        public float P
        {
            get => (float)Math.Round(_p, 2); // Enforce precision to 2 decimal places
            set => _p = value;
        }
    }
}
