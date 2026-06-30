using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Application.ProductionPlanning;

public interface IProductionPlanService
{
    IReadOnlyList<ProductionPlanResponseItem> CalculateProductionPlan(ProductionPlanRequest request);
}
