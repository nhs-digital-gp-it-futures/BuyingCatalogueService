using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
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
        /// Date when the solution was last modified, as displayed to the user
        /// </summary>
        DateTime LastUpdated { get; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Name of the supplier, as displayed to a user.
        /// </summary>
        string SupplierName { get; }

        /// <summary>
        /// Gets or sets a list of features.
        /// </summary>
        IEnumerable<string> Features { get; }

        /// <summary>
        /// Gets or sets the Road Map.
        /// </summary>
        IRoadMap RoadMap { get; }

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
        /// Marketing information related to the hosting
        /// </summary>
        IHosting Hosting { get; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        bool IsFoundation { get; }

        /// <summary>
        /// Capabilities claimed by the solution
        /// </summary>
        IEnumerable<string> Capabilities { get; }

        /// <summary>
        /// The contacts for the solution
        /// </summary>
        IEnumerable<IContact> Contacts { get; }

        /// <summary>
        /// The publish status of the solution
        /// </summary>
        PublishedStatus PublishedStatus { get;  }

        /// <summary>
        /// Marketing information related to the supplier
        /// </summary>
        ISupplier Supplier { get; }
    }
}
