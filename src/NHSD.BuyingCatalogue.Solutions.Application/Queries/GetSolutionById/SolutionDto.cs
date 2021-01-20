using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// A product and/or service provided by a supplier.
    /// </summary>
    internal sealed class SolutionDto : ISolution
    {
        /// <summary>
        /// Gets or sets the ID of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the name of the supplier, as displayed to a user.
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// Gets or sets a list of features.
        /// </summary>
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// Gets or sets a road map.
        /// </summary>
        public IRoadMap RoadMap { get; set; }

        /// <summary>
        /// Gets or sets a link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Gets or sets the status of this instance in relation to the supplier.
        /// </summary>
        public SupplierStatus SupplierStatus { get; set; }

        /// <summary>
        /// Gets or sets the marketing information related to the clients application.
        /// </summary>
        public IClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Gets or sets the marketing information related to the hosting.
        /// </summary>
        public IHosting Hosting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Gets or sets the capabilities claimed by the solution.
        /// </summary>
        public IEnumerable<IClaimedCapability> Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the contacts for the solution.
        /// </summary>
        public IEnumerable<IContact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the publishing status of the solution.
        /// </summary>
        public PublishedStatus PublishedStatus { get; set; }

        /// <summary>
        /// Gets or sets the supplier info for the solution.
        /// </summary>
        public ISolutionSupplier Supplier { get; set; }

        /// <summary>
        /// Gets or sets an integration.
        /// </summary>
        public IIntegrations Integrations { get; set; }

        /// <summary>
        /// Gets or sets an implementation timescales.
        /// </summary>
        public IImplementationTimescales ImplementationTimescales { get; set; }

        /// <summary>
        /// Gets or sets the solution document for the solution.
        /// </summary>
        public ISolutionDocument SolutionDocument { get; set; }
    }
}
