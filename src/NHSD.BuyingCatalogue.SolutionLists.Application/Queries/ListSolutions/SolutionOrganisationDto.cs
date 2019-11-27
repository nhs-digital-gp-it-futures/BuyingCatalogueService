using System;
using NHSD.BuyingCatalogue.Contracts.SolutionList;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Represents the details of an organisation specific for the view.
    /// </summary>
    internal sealed class SolutionOrganisationDto : ISolutionOrganisation
    {
        /// <summary>
        /// Identifier of organisation.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of organisation.
        /// </summary>
        public string Name { get; set; }
    }
}
