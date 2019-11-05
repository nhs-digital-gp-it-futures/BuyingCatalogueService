using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Domain
{
    /// <summary>
    /// A product and/or service provided by an ‘organisation’.
    /// </summary>
    internal class Solution
    {
        internal Solution(ISolutionResult solutionResult)
        {
            Id = solutionResult.Id;
            Name = solutionResult.Name;
            Summary = solutionResult.Summary;
            OrganisationName = solutionResult.OrganisationName;
            Description = solutionResult.Description;
            Features = string.IsNullOrWhiteSpace(solutionResult.Features)
                ? new List<string>()
                : JsonConvert.DeserializeObject<IEnumerable<string>>(solutionResult.Features);
            AboutUrl = solutionResult.AboutUrl;
            ClientApplication = string.IsNullOrWhiteSpace(solutionResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(solutionResult.ClientApplication);
        }

        /// <summary>
        /// Id of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

 		/// <summary>
        /// Name of the organisation, as displayed to a user.
        /// </summary>
        public string OrganisationName { get; set; }

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
        public SupplierStatus SupplierStatus { get; }

        /// <summary>
        /// Marketing information related to the clients application.
        /// </summary>
        public ClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution()
        {
            SupplierStatus = SupplierStatus.Draft;
        }
    }
}
