using PowerplantCodingChallenge.Business.Models;

namespace PowerplantCodingChallenge.Business.Extensions
{
    internal static class CollectionExtensions
    {
        internal static IEnumerable<AllocationCandidate> SortByMeritOrder(this IEnumerable<AllocationCandidate> candidates)
            => candidates
                .OrderBy(c => c.MarginalCost)
                .ThenBy(c => c.PowerMaxTenths);
    }
}
