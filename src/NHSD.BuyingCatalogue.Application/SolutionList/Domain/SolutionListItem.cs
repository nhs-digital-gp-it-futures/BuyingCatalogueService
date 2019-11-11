using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Domain
{
    internal sealed class SolutionListItem
    {
        /// <summary>
        /// Unique ID of the entity.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Associated organisation.
        /// </summary>
        public SolutionListItemOrganisation Organisation { get; }

        /// <summary>
        /// A list of capabilities associated with the solution.
        /// </summary>
        public HashSet<SolutionListItemCapability> Capabilities { get; }

        public SolutionListItem(ISolutionListResult item)
        {
            Id = item.SolutionId;
            Name = item.SolutionName;
            Summary = item.SolutionSummary;
            Organisation = new SolutionListItemOrganisation(item);
            Capabilities = new HashSet<SolutionListItemCapability>();
        }
    }
}
