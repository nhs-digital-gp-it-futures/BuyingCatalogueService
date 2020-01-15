using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    /// <summary>
    /// A product and/or service provided by an ‘organisation’.
    /// </summary>
    internal class SolutionDto : ISolution
    {
        /// <summary>
        /// Id of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

 		/// <summary>
        /// Name of the supplier, as displayed to a user.
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// Gets or sets a list of features.
        /// </summary>
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// A link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Status of this instance in relation to the supplier.
        /// </summary>
        public SupplierStatus SupplierStatus { get; set; }

        /// <summary>
        /// Marketing information related to the clients application.
        /// </summary>
        public IClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Marketing information related to the hosting.
        /// </summary>
        public IHosting Hosting { get; set; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Capabilities claimed by the solution
        /// </summary>
        public IEnumerable<string> Capabilities { get; set; }

        /// <summary>
        /// The contacts for the solution
        /// </summary>
        public IEnumerable<IContact> Contacts { get; set; }

        /// <summary>
        /// The publishing status of the solution
        /// </summary>
        public PublishedStatus PublishedStatus { get; set; }
    }
}
