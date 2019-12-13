using System;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
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

        [JsonProperty("browser-mobile-first")]
        public BrowserBasedDashboardSection BrowserMobileFirstSection { get; }

        [JsonProperty("plug-ins-or-extensions")]
        public BrowserBasedDashboardSection PluginsOrExtensionsSection { get; }

        [JsonProperty("connectivity-and-resolution")]
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
            BrowsersSupportedSection = new BrowserBasedDashboardSection(IsBrowserSupportedComplete(clientApplication), true);
            BrowserMobileFirstSection = new BrowserBasedDashboardSection(IsMobileFirstComplete(clientApplication), true);
            PluginsOrExtensionsSection = new BrowserBasedDashboardSection(IsPluginsComplete(clientApplication?.Plugins), true);
            ConnectivityAndResolutionSection = new BrowserBasedDashboardSection(!String.IsNullOrWhiteSpace(clientApplication?.MinimumConnectionSpeed), true);
            HardwareRequirementsSection = new BrowserBasedDashboardSection(IsHardwareRequirementsComplete(clientApplication), false);
            BrowserAdditionalInformationSection = new BrowserBasedDashboardSection(IsBrowserAdditionalInformationComplete(clientApplication), false);
        }

        private bool IsBrowserSupportedComplete(IClientApplication clientApplication)
        {
            return clientApplication?.BrowsersSupported?.Any() == true && clientApplication?.MobileResponsive.HasValue == true;
        }

        private bool IsMobileFirstComplete(IClientApplication clientApplication)
        {
            return clientApplication?.MobileFirstDesign.HasValue == true;
        }

        private bool IsPluginsComplete(IPlugins plugins)
        {
            return plugins?.Required.HasValue == true;
        }

        private bool IsHardwareRequirementsComplete(IClientApplication clientApplication)
        {
            return !string.IsNullOrWhiteSpace(clientApplication?.HardwareRequirements);
        }

        private bool IsBrowserAdditionalInformationComplete(IClientApplication clientApplication)
        {
            return !string.IsNullOrWhiteSpace(clientApplication?.AdditionalInformation);
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
