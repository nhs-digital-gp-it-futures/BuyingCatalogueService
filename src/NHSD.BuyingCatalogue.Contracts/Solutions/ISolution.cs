using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Contracts.Solutions
{
    public interface ISolution
    {
        /// <summary>
        /// Id of the solution.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Name of the organisation, as displayed to a user.
        /// </summary>
        string OrganisationName { get; }

        /// <summary>
        /// Gets or sets a list of features.
        /// </summary>
        IEnumerable<string> Features { get; }

        /// <summary>
        /// A link to provide more information about a solution.
        /// </summary>
        string AboutUrl { get; }

        /// <summary>
        /// Status of this instance in relation to the supplier.
        /// </summary>
        SupplierStatus SupplierStatus { get; }

        /// <summary>
        /// Marketing information related to the clients application.
        /// </summary>
        IClientApplication ClientApplication { get; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        bool IsFoundation { get; }
    }
}
