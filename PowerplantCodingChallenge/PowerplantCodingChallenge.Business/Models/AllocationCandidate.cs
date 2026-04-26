using PowerplantCodingChallenge.Domain.Request.Enumeration;

namespace PowerplantCodingChallenge.Business.Models
{
    internal sealed class AllocationCandidate
    {
        public string Name { get; init; } = string.Empty;

        public PlantType Type { get; init; }

        /// <summary>
        /// Cost per MWh, used for sorting by merit-order
        /// </summary>
        public decimal MarginalCost { get; init; }

        /// <summary>
        /// Minimum production if plant is active (in tenths of MW)
        /// </summary>
        public int PowerMinTenths { get; init; }

        /// <summary>
        /// Maximum available production (in tenths of MW)
        /// </summary>
        public int PowerMaxTenths { get; init; }

        /// <summary>
        /// Computed allocated production (in tenths of MW)
        /// </summary>
        public int AllocatedPowerTenths { get; set; }
    }
}
