using System;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItemOrganisation
    {
        public SolutionListItemOrganisation(ISolutionListResult item)
        {
            Id = item.OrganisationId;
            Name = item.OrganisationName;
        }

        /// <summary>
        /// Identifier of the organisation.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name of the organisation.
        /// </summary>
        public string Name { get; }
    }
}
