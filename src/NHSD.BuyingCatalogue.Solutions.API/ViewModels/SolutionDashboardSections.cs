using System;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SolutionDashboardSections
    {
        public SolutionDashboardSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescriptionSection = DashboardSection.Mandatory(!string.IsNullOrWhiteSpace(solution.Summary));
            FeaturesSection = DashboardSection.Optional(
                solution.Features?.Any(feature => !string.IsNullOrWhiteSpace(feature)) == true);

            ClientApplicationTypesSection = DashboardSection.MandatoryWithSubSection(
                solution.ClientApplication?.ClientApplicationTypes?.Any() == true,
                new ClientApplicationTypesSubSections(solution.ClientApplication));

            ContactDetailsSection = DashboardSection.Optional(new ContactAnswerSection(solution.Contacts).HasData());
            HostingTypePublicCloudSection = DashboardSection.Optional(solution.Hosting.IsPublicCloudComplete());
            HostingTypePrivateCloudSection = DashboardSection.Optional(solution.Hosting.IsPrivateCloudComplete());
            HostingTypeHybridSection = DashboardSection.Optional(solution.Hosting.IsHybridHostingTypeComplete());
            HostingTypeOnPremiseSection = DashboardSection.Optional(solution.Hosting.IsOnPremiseComplete());
            AboutSupplierSection = DashboardSection.Optional(solution.Supplier.IsSupplierComplete());
            RoadMapSection = DashboardSection.Optional(solution.IsRoadMapComplete());
            IntegrationsSection = DashboardSection.Optional(solution.IsIntegrationsComplete());
            ImplementationTimescalesSection = DashboardSection.Optional(solution.IsImplementationTimescalesComplete());
        }

        [JsonProperty("solution-description")]
        public DashboardSection SolutionDescriptionSection { get; }

        [JsonProperty("features")]
        public DashboardSection FeaturesSection { get; }

        [JsonProperty("client-application-types")]
        public DashboardSection ClientApplicationTypesSection { get; }

        [JsonProperty("contact-details")]
        public DashboardSection ContactDetailsSection { get; }

        [JsonProperty("hosting-type-public-cloud")]
        public DashboardSection HostingTypePublicCloudSection { get; }

        [JsonProperty("hosting-type-private-cloud")]
        public DashboardSection HostingTypePrivateCloudSection { get; }

        [JsonProperty("hosting-type-hybrid")]
        public DashboardSection HostingTypeHybridSection { get; }

        [JsonProperty("hosting-type-on-premise")]
        public DashboardSection HostingTypeOnPremiseSection { get; }

        [JsonProperty("about-supplier")]
        public DashboardSection AboutSupplierSection { get; }

        // ReSharper disable once StringLiteralTypo (published model)
        [JsonProperty("roadmap")]
        public DashboardSection RoadMapSection { get; }

        [JsonProperty("integrations")]
        public DashboardSection IntegrationsSection { get; }

        [JsonProperty("implementation-timescales")]
        public DashboardSection ImplementationTimescalesSection { get; }
    }
}
