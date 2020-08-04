﻿using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionList
    {
        public List<SolutionListItem> Solutions { get; }

        internal SolutionList(IEnumerable<ICapabilityReference> capabilityReferences, IEnumerable<ISolutionListResult> solutionListResults)
        {
            var uniqueCapabilityReferences = new HashSet<string>(capabilityReferences.Select(x => x.Reference));
            var solutions = new Dictionary<string, SolutionListItem>();
            foreach (var result in solutionListResults)
            {
                if (!solutions.ContainsKey(result.SolutionId))
                {
                    solutions.Add(result.SolutionId, new SolutionListItem(result));
                }

                solutions[result.SolutionId].Capabilities.Add(new SolutionListItemCapability(result));
            }

            Solutions = solutions.Values
                .Where(s => s.Capabilities.Select(c => c.CapabilityReference).Intersect(uniqueCapabilityReferences).Count() ==
                            uniqueCapabilityReferences.Count)
                .ToList();
        }
    }
}
