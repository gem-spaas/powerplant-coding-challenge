using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiloWattNavigator.Domain
{
    public class ProductionPlanResponse
    {
        public ICollection<ProductionPlanResponseItem> Powerplants { get; set; } = new List<ProductionPlanResponseItem>();

        public void AddPowerplant(string name, double power)
        {
            Powerplants.Add(new ProductionPlanResponseItem { Name = name, P = power});
        }
    }
}
