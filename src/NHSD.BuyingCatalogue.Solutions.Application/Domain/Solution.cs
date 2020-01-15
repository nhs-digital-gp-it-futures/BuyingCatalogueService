using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    /// <summary>
    /// A product and/or service provided by an ‘organisation’.
    /// </summary>
    internal class Solution
    {
        /// <summary>
        /// Id of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Record of the latest date the solution was modified
        /// </summary>
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
        public SupplierStatus SupplierStatus { get; }

        /// <summary>
        /// Marketing information related to the clients application.
        /// </summary>
        public ClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Capabilities claimed by the solution
        /// </summary>
        public HashSet<string> Capabilities { get; set; }

        /// <summary>
        /// The contacts for the solution
        /// </summary>
        public IEnumerable<Contact> Contacts { get; set; }

        /// <summary>
        /// The publishing status of the solution
        /// </summary>
        public PublishedStatus PublishedStatus { get; set; }

        public Hosting Hosting { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution"/> class.
        /// </summary>
        internal Solution(ISolutionResult solutionResult,
            IEnumerable<ISolutionCapabilityListResult> solutionCapabilityListResult,
            IEnumerable<IMarketingContactResult> contactResult)
        {
            Id = solutionResult.Id;
            Name = solutionResult.Name; 
            LastUpdated = GetLatestLastUpdated(solutionResult, contactResult);
            Summary = solutionResult.Summary;
            SupplierName = solutionResult.SupplierName;
            Description = solutionResult.Description;
            Features = string.IsNullOrWhiteSpace(solutionResult.Features)
                ? new List<string>()
                : JsonConvert.DeserializeObject<IEnumerable<string>>(solutionResult.Features);
            AboutUrl = solutionResult.AboutUrl;
            ClientApplication = string.IsNullOrWhiteSpace(solutionResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(solutionResult.ClientApplication);
            IsFoundation = solutionResult.IsFoundation;
            Capabilities = new HashSet<string>(solutionCapabilityListResult.Select(c => c.CapabilityName));
            Contacts = contactResult.Select(c => new Contact(c));
            PublishedStatus = solutionResult.PublishedStatus;

            Hosting = string.IsNullOrWhiteSpace(solutionResult.Hosting)
                ? new Hosting()
                : JsonConvert.DeserializeObject<Hosting>(solutionResult.Hosting);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution()
        {
            SupplierStatus = SupplierStatus.Draft;
            PublishedStatus = PublishedStatus.Draft;
        }

        private DateTime GetLatestLastUpdated(ISolutionResult solutionResult, IEnumerable<IMarketingContactResult> contactResult) =>
            new List<DateTime>
            {
                solutionResult.LastUpdated,
                solutionResult.SolutionDetailLastUpdated,
                contactResult?.Any() == false ? DateTime.MinValue : contactResult.Max(x => x.LastUpdated)
            }.Max();
    }
}
