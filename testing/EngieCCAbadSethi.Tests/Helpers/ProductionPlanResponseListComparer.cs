using CalculatePowerGenerationByPowerPlants.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalculatePowerGeneratedByPlants.Tests.Helpers
{
    internal class ProductionPlanResponseListComparer : IEqualityComparer<ProductionPlanResponse>
    {
        public static readonly ProductionPlanResponseListComparer Instance
            = new ProductionPlanResponseListComparer();

        private ProductionPlanResponseListComparer()
        {
        }

        public bool Equals(ProductionPlanResponse x, ProductionPlanResponse y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.Name == y.Name
                && x.P == y.P;
        }

        public int GetHashCode([DisallowNull] ProductionPlanResponse obj)
        {

            return (obj.Name ?? string.Empty).GetHashCode()
                ^ (obj.P.GetHashCode());
        }
    }
}
