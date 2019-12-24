using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    public sealed class BrowserBasedResult
    {
        [JsonProperty("sections")]
        public BrowserBasedDashboardSections BrowserBasedDashboardSections { get; }

        public BrowserBasedResult (IClientApplication clientApplication)
        {
            BrowserBasedDashboardSections = new BrowserBasedDashboardSections(clientApplication);
        }
    }

    public class BrowserBasedDashboardSections
    {
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

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedDashboardSections"/> class.
        /// </summary>
        public BrowserBasedDashboardSections(IClientApplication clientApplication)
        {
            BrowsersSupportedSection = new BrowserBasedDashboardSection(clientApplication.IsBrowserSupportedComplete(), true);
            BrowserMobileFirstSection = new BrowserBasedDashboardSection(clientApplication.IsBrowserMobileFirstComplete(), true);
            PluginsOrExtensionsSection = new BrowserBasedDashboardSection(clientApplication.IsPluginsComplete(), true);
            ConnectivityAndResolutionSection = new BrowserBasedDashboardSection(clientApplication.IsConnectivityAndResolutionComplete(), true);
            HardwareRequirementsSection = new BrowserBasedDashboardSection(clientApplication.IsHardwareRequirementComplete(), false);
            BrowserAdditionalInformationSection = new BrowserBasedDashboardSection(clientApplication.IsAdditionalInformationComplete(), false);
        }
    }

    public class BrowserBasedDashboardSection
    {
        private readonly bool _mandatory;
        private readonly bool _complete;

        [JsonProperty("status")]
        public string Status => _complete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("requirement")]
        public string Requirement => _mandatory ? "Mandatory" : "Optional";

        public BrowserBasedDashboardSection(bool complete, bool mandatory)
        {
            _complete = complete;
            _mandatory = mandatory;
        }
    }
}
