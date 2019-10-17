using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal static class ISolutionListResultExtensions
    {

        internal static IEnumerable<Solution> Map(this IEnumerable<ISolutionListResult> solutionListResults)
        {
            var solutions = solutionListResults.Select(s => s.MapToSolution()).ToArray().Distinct();// force eager execution

            foreach (var item in solutionListResults)
            {
                solutions.Single(s => s.Id == item.SolutionId).AddCapability(item.MapToSolutionCapability());
            }

            return solutions;
        }

        private static Solution MapToSolution(this ISolutionListResult item)
            => new Solution
            {
                Id = item.SolutionId,
                Name = item.SolutionName,
                Summary = item.SolutionSummary,
                Organisation = new Organisation
                {
                    Id = item.OrganisationId,
                    Name = item.OrganisationName
                }
            };

        private static Capability MapToSolutionCapability(this ISolutionListResult item)
            => new Capability
            {
                Id = item.CapabilityId,
                Name = item.CapabilityName,
                Description = item.CapabilityDescription
            };
    }
}
