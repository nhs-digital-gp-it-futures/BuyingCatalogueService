using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class BrowserBasedDashboardSections
    {
        public BrowserBasedDashboardSections(IClientApplication clientApplication)
        {
            BrowsersSupportedSection = new BrowserBasedDashboardSection(clientApplication.IsBrowserSupportedComplete(), true);
            BrowserMobileFirstSection = new BrowserBasedDashboardSection(clientApplication.IsBrowserMobileFirstComplete(), true);
            PluginsOrExtensionsSection = new BrowserBasedDashboardSection(clientApplication.IsPluginsComplete(), true);
            ConnectivityAndResolutionSection = new BrowserBasedDashboardSection(clientApplication.IsConnectivityAndResolutionComplete(), true);
            HardwareRequirementsSection = new BrowserBasedDashboardSection(clientApplication.IsHardwareRequirementComplete(), false);
            BrowserAdditionalInformationSection = new BrowserBasedDashboardSection(clientApplication.IsAdditionalInformationComplete(), false);
        }

        [JsonProperty("browser-browsers-supported")]
        public BrowserBasedDashboardSection BrowsersSupportedSection { get; }

        [JsonProperty("browser-mobile-first")]
        public BrowserBasedDashboardSection BrowserMobileFirstSection { get; }

        [JsonProperty("browser-plug-ins-or-extensions")]
        public BrowserBasedDashboardSection PluginsOrExtensionsSection { get; }

        [JsonProperty("browser-connectivity-and-resolution")]
        public BrowserBasedDashboardSection ConnectivityAndResolutionSection { get; }

        [JsonProperty("browser-hardware-requirements")]
        public BrowserBasedDashboardSection HardwareRequirementsSection { get; }

        [JsonProperty("browser-additional-information")]
        public BrowserBasedDashboardSection BrowserAdditionalInformationSection { get; }
    }
}
