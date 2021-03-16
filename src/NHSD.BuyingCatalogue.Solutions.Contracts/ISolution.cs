using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface ISolution
    {
        /// <summary>
        /// Gets the ID of the solution.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the solution, as displayed to a user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the date when the solution was last modified, as displayed to the user.
        /// </summary>
        DateTime LastUpdated { get; }

        /// <summary>
        /// Gets the full description of the solution, as displayed to the user.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the summary of the solution, as displayed to a user.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Gets the name of the supplier, as displayed to a user.
        /// </summary>
        string SupplierName { get; }

        /// <summary>
        /// Gets the list of features.
        /// </summary>
        IEnumerable<string> Features { get; }

        /// <summary>
        /// Gets the road map.
        /// </summary>
        IRoadMap RoadMap { get; }

        /// <summary>
        /// Gets a link to provide more information about a solution.
        /// </summary>
        string AboutUrl { get; }

        /// <summary>
        /// Gets the marketing information related to the clients application.
        /// </summary>
        IClientApplication ClientApplication { get; }

        /// <summary>
        /// Gets the marketing information related to the hosting.
        /// </summary>
        IHosting Hosting { get; }

        /// <summary>
        /// Gets a value indicating whether this is a foundation solution.
        /// </summary>
        bool IsFoundation { get; }

        /// <summary>
        /// Gets the capabilities claimed by the solution.
        /// </summary>
        IEnumerable<IClaimedCapability> Capabilities { get; }

        /// <summary>
        /// Gets the contacts for the solution.
        /// </summary>
        IEnumerable<IContact> Contacts { get; }

        /// <summary>
        /// Gets the publication status of the solution.
        /// </summary>
        PublishedStatus PublishedStatus { get;  }

        /// <summary>
        /// Gets the marketing information related to the supplier.
        /// </summary>
        ISolutionSupplier Supplier { get; }

        /// <summary>
        /// Gets the integrations.
        /// </summary>
        IIntegrations Integrations { get; }

        /// <summary>
        /// Gets the implementation timescales.
        /// </summary>
        IImplementationTimescales ImplementationTimescales { get; }

        /// <summary>
        /// Gets the solution document.
        /// </summary>
        ISolutionDocument SolutionDocument { get; }

        /// <summary>
        /// Gets the solution framework short names.
        /// </summary>
        IEnumerable<string> FrameworkShortNames { get; }
    }
}
