using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Application.SolutionList.Domain;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Persistence
{
    internal static class ISolutionListResultExtensions
    {

        internal static IEnumerable<SolutionListItem> Map(this IEnumerable<ISolutionListResult> solutionListResults)
        {
            var solutions = solutionListResults.Select(s => s.MapToSolution()).ToArray().Distinct(new SolutionListItemComparer());// force eager execution

            foreach (var item in solutionListResults)
            {
                solutions.Single(s => s.Id == item.SolutionId).AddCapability(item.MapToSolutionCapability());
            }

            return solutions;
        }

        private static SolutionListItem MapToSolution(this ISolutionListResult item)
            => new SolutionListItem
            {
                Id = item.SolutionId,
                Name = item.SolutionName,
                Summary = item.SolutionSummary,
                Organisation = new SolutionListItemOrganisation
                {
                    Id = item.OrganisationId,
                    Name = item.OrganisationName
                }
            };

        private static SolutionListItemCapability MapToSolutionCapability(this ISolutionListResult item)
            => new SolutionListItemCapability
            {
                Id = item.CapabilityId,
                Name = item.CapabilityName,
                Description = item.CapabilityDescription
            };
    }
}
