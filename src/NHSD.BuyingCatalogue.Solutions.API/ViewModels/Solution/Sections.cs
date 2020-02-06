using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class Sections
    {
        [JsonProperty("solution-description")]
        public SolutionDescriptionSection SolutionDescription { get; }

        public FeaturesSection Features { get; }

        [JsonProperty("roadmap")]
        public RoadMapSection RoadMap { get; }

        [JsonProperty("integrations")]
        public IntegrationsSection Integrations { get; }

        [JsonProperty("implementation-timescales")]
        public ImplementationTimescalesSection ImplementationTimescales { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesSection ClientApplicationTypes { get; }

        [JsonProperty("contact-details")]
        public ContactDetailsSection ContactDetails { get; }

        [JsonProperty("capabilities")]
        public CapabilitiesSection Capabilities { get; }

        [JsonProperty("hosting-type-public-cloud")]
        public PublicCloudSection PublicCloud { get; }

        [JsonProperty("hosting-type-private-cloud")]
        public PrivateCloudSection PrivateCloud { get; }

        [JsonProperty("hosting-type-on-premise")]
        public OnPremiseSection OnPremise { get; }

        [JsonProperty("hosting-type-hybrid")]
        public HybridHostingTypeSection HybridHostingType { get; }

        [JsonProperty("about-supplier")]
        public AboutSupplierSection AboutSupplier { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Sections"/> class.
        /// </summary>
        public Sections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescription = new SolutionDescriptionSection(solution).IfPopulated();
            Features = new FeaturesSection(solution.Features).IfPopulated();
            RoadMap = new RoadMapSection(solution.RoadMap).IfPopulated();
            Integrations = new IntegrationsSection(solution.Integrations).IfPopulated();
            ImplementationTimescales = new ImplementationTimescalesSection(solution.ImplementationTimescales).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesSection(solution.ClientApplication).IfPopulated();
            ContactDetails = new ContactDetailsSection(solution.Contacts).IfPopulated();
            Capabilities = new CapabilitiesSection(solution.Capabilities);
            PublicCloud = new PublicCloudSection(solution.Hosting).IfPopulated();
            PrivateCloud = new PrivateCloudSection(solution.Hosting).IfPopulated();
            OnPremise = new OnPremiseSection(solution.Hosting).IfPopulated();
            HybridHostingType = new HybridHostingTypeSection(solution.Hosting).IfPopulated();
            AboutSupplier = new AboutSupplierSection(solution?.Supplier).IfPopulated();
        }
    }
}
