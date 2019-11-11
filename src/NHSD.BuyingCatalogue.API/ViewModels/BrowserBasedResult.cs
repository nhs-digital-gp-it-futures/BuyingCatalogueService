using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
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
        [JsonProperty("browsers-supported")]
        public BrowserBasedDashboardSection BrowsersSupportedSection { get; }

        [JsonProperty("plug-ins-or-extensions")]
        public BrowserBasedDashboardSection PluginsOrExtensionsSection { get; }

        [JsonProperty("connectivity-and-resolution")]
        public BrowserBasedDashboardSection ConnectivityAndResolutionSection { get; }

        [JsonProperty("hardware-requirements")]
        public BrowserBasedDashboardSection HardwareRequirementsSection { get; }

        [JsonProperty("additional-information")]
        public BrowserBasedDashboardSection AdditionalInformationSection { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedDashboardSections"/> class.
        /// </summary>
        public BrowserBasedDashboardSections(IClientApplication clientApplication)
        {
            BrowsersSupportedSection = new BrowserBasedDashboardSection(IsBrowserSupportedComplete(clientApplication), true);
            PluginsOrExtensionsSection = new BrowserBasedDashboardSection(false, true);
            ConnectivityAndResolutionSection = new BrowserBasedDashboardSection(false, true);
            HardwareRequirementsSection = new BrowserBasedDashboardSection(false, false);
            AdditionalInformationSection = new BrowserBasedDashboardSection(false, false);
        }

        private bool IsBrowserSupportedComplete(IClientApplication clientApplication)
        {
            return clientApplication?.BrowsersSupported?.Any() == true && clientApplication?.MobileResponsive.HasValue == true;
        }

        private bool IsPluginsComplete(IPlugins plugins)
        {
            return plugins?.Required.HasValue == true;
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
