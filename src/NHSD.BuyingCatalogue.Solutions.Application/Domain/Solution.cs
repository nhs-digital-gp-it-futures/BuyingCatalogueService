using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    /// <summary>
    /// A product and/or service provided by a supplier.
    /// </summary>
    internal sealed class Solution
    {
        public Solution()
        {
            PublishedStatus = PublishedStatus.Draft;
        }

        internal Solution(
            ISolutionResult solutionResult,
            IEnumerable<ISolutionCapabilityListResult> solutionCapabilityListResult,
            IEnumerable<IMarketingContactResult> contactResult,
            ISolutionSupplierResult solutionSupplierResult,
            IDocumentResult documentResult,
            IEnumerable<ISolutionEpicListResult> solutionEpicListResults)
        {
            var contactResultList = contactResult.ToList();
            var solutionEpicsByCapability = solutionEpicListResults?.ToLookup(e => e.CapabilityId);

            Id = solutionResult.Id;
            Name = solutionResult.Name;
            LastUpdated = GetLatestLastUpdated(solutionResult, contactResultList);
            Summary = solutionResult.Summary;
            Description = solutionResult.Description;
            Features = string.IsNullOrWhiteSpace(solutionResult.Features)
                ? new List<string>()
                : JsonConvert.DeserializeObject<IEnumerable<string>>(solutionResult.Features);

            Integrations = new Integrations
            {
                Url = solutionResult.IntegrationsUrl,
                DocumentName = documentResult?.IntegrationDocumentName,
            };

            ImplementationTimescales = new ImplementationTimescales
            {
                Description = solutionResult.ImplementationTimescales,
            };

            AboutUrl = solutionResult.AboutUrl;

            RoadMap = new RoadMap
            {
                Summary = solutionResult.RoadMap,
                DocumentName = documentResult?.RoadMapDocumentName,
            };

            ClientApplication = string.IsNullOrWhiteSpace(solutionResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(solutionResult.ClientApplication);

            IsFoundation = solutionResult.IsFoundation;
            Capabilities = solutionCapabilityListResult.Select(
                c => new ClaimedCapability(c, solutionEpicsByCapability?[c.CapabilityId]));

            Contacts = contactResultList.Select(c => new Contact(c));
            PublishedStatus = solutionResult.PublishedStatus;
            FrameworkShortNames = solutionResult.FrameworkShortNames;
            Hosting = string.IsNullOrWhiteSpace(solutionResult.Hosting)
                ? new Hosting()
                : JsonConvert.DeserializeObject<Hosting>(solutionResult.Hosting);

            Supplier = solutionSupplierResult is not null
                ? new SolutionSupplier(solutionSupplierResult)
                : new SolutionSupplier();

            SolutionDocument = new SolutionDocument(documentResult?.SolutionDocumentName);
        }

        /// <summary>
        /// Gets or sets the id of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the record of the latest date the solution was modified.
        /// </summary>
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
        /// Gets or sets a list of features.
        /// </summary>
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// Gets or sets a road map description.
        /// </summary>
        public RoadMap RoadMap { get; set; }

        /// <summary>
        /// Gets or sets an integration.
        /// </summary>
        public Integrations Integrations { get; set; }

        /// <summary>
        /// Gets or sets a link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Gets or sets the marketing information related to the clients application.
        /// </summary>
        public ClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Gets or sets the capabilities claimed by the solution.
        /// </summary>
        public IEnumerable<ClaimedCapability> Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the contacts for the solution.
        /// </summary>
        public IEnumerable<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the publishing status of the solution.
        /// </summary>
        public PublishedStatus PublishedStatus { get; set; }

        /// <summary>
        /// Gets or sets the hosting of the solution.
        /// </summary>
        public Hosting Hosting { get; set; }

        /// <summary>
        /// Gets or sets the supplier of the solution.
        /// </summary>
        public SolutionSupplier Supplier { get; set; }

        /// <summary>
        /// Gets or sets an implementation timescales.
        /// </summary>
        public ImplementationTimescales ImplementationTimescales { get; set; }

        /// <summary>
        /// Gets or sets the solution document for the solution.
        /// </summary>
        public SolutionDocument SolutionDocument { get; set; }

        /// <summary>
        /// Gets or sets the solution framework short names.
        /// </summary>
        public IEnumerable<string> FrameworkShortNames { get; set; }

        private static DateTime GetLatestLastUpdated(
            ISolutionResult solutionResult,
            IList<IMarketingContactResult> contactResult) =>
            new List<DateTime>
            {
                solutionResult.LastUpdated,
                solutionResult.SolutionDetailLastUpdated,
                contactResult?.Any() == true ? contactResult.Max(r => r.LastUpdated) : DateTime.MinValue,
            }.Max();
    }
}
