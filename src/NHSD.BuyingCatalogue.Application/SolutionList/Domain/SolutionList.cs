using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Application.SolutionList.Persistence;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Domain
{
    internal sealed class SolutionList
    {
        public List<SolutionListItem> SolutionListItems { get; }

        internal SolutionList(ISet<Guid> capabilityIdList, IEnumerable<ISolutionListResult> solutionListResults)
        {
            var solutions = solutionListResults.Select(s => new SolutionListItem(s)).ToArray().Distinct(new SolutionListItemComparer());// force eager execution

            foreach (var item in solutionListResults)
            {
                solutions.Single(s => s.Id == item.SolutionId).Capabilities.Add(new SolutionListItemCapability(item));
            }

            SolutionListItems = CapabilityFilter(capabilityIdList, solutions).ToList();
        }

        private IEnumerable<SolutionListItem> CapabilityFilter(ISet<Guid> capabilityIdList, IEnumerable<SolutionListItem> solutions)
        {
            return capabilityIdList.Any() ?
                solutions.Where(solution => capabilityIdList.Intersect(solution.Capabilities.Select(capability => capability.Id)).Count() == capabilityIdList.Count()) :
                solutions;
        }
    }
}
