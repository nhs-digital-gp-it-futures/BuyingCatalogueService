using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Domain
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
        public string Id { get; }

        /// <summary>
        /// Name of the organisation.
        /// </summary>
        public string Name { get; }
    }
}