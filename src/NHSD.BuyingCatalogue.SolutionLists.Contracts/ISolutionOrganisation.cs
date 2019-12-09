using System;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts
{
    public interface ISolutionOrganisation
    {
        /// <summary>
        /// Identifier of organisation.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of organisation.
        /// </summary>
        string Name { get; }
    }
}
