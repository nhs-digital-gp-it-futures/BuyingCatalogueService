using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class SolutionDashboardResult
    {
        public string Id { get; }

        public string Name { get; }
        
        [JsonProperty("supplier-name")]
        public string SupplierName { get; }

        [JsonProperty("sections")]
        public SolutionDashboardSections SolutionDashboardSections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDashboardResult"/> class.
        /// </summary>
        public SolutionDashboardResult(ISolution solution)
        {
            if (solution != null)
            {
                Id = solution.Id;
                Name = solution.Name;
                SupplierName = solution.SupplierName;
                SolutionDashboardSections = new SolutionDashboardSections(solution);
            }
        }
    }

    public class SolutionDashboardSections
    {
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

        [JsonProperty("roadmap")]
        public DashboardSection RoadMapSection { get; }

        [JsonProperty("integrations")]
        public DashboardSection IntegrationsSection { get; }

        [JsonProperty("implementation-timescales")]
        public DashboardSection ImplementationTimescalesSection { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDashboardSections"/> class.
        /// </summary>
        public SolutionDashboardSections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescriptionSection = DashboardSection.Mandatory(!string.IsNullOrWhiteSpace(solution.Summary));
            FeaturesSection = DashboardSection.Optional(solution.Features?.Any(feature => !string.IsNullOrWhiteSpace(feature)) == true);
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
    }

    public class ClientApplicationTypesSubSections
    {
        [JsonProperty("browser-based")]
        public DashboardSection BrowserBasedSection { get; private set; }

        [JsonProperty("native-mobile")]
        public DashboardSection NativeMobileSection { get; private set; }

        [JsonProperty("native-desktop")]
        public DashboardSection NativeDesktopSection { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClientApplicationTypesSubSections"/> class.
        /// </summary>
        /// <param name="clientApplication"></param>
        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            HashSet<string> clientApplicationTypes = clientApplication?.ClientApplicationTypes ?? new HashSet<string>();

            SetIfSelected("browser-based", clientApplicationTypes,
                () => BrowserBasedSection = DashboardSection.Mandatory(clientApplication.IsBrowserBasedComplete()));
            SetIfSelected("native-mobile", clientApplicationTypes,
                () => NativeMobileSection = DashboardSection.Mandatory(clientApplication.IsNativeMobileComplete()));
            SetIfSelected("native-desktop", clientApplicationTypes,
                () => NativeDesktopSection = DashboardSection.Mandatory(clientApplication.IsNativeDesktopComplete()));
        }

        private static void SetIfSelected(string sectionName, HashSet<string> sections, Action setDashboardAction)
        {
            if (sections.Contains(sectionName))
            {
                setDashboardAction();
            }
        }
    }
}

