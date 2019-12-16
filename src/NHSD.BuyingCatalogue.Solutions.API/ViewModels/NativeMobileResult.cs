using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class NativeMobileResult
    {
        [JsonProperty("sections")]
        public NativeMobileSections NativeMobileSections { get; }

        public NativeMobileResult()
        {
            NativeMobileSections = new NativeMobileSections();
        }
    }

    public class NativeMobileSections
    {
        [JsonProperty("mobile-operating-systems")]
        public NativeMobileDashboardSections MobileOperatingSystems { get; }

        [JsonProperty("mobile-first")]
        public NativeMobileDashboardSections MobileFirst { get; }
        
        [JsonProperty("mobile-memory-and-storage")]
        public NativeMobileDashboardSections MobileMemoryStorage { get; }

        [JsonProperty("mobile-connection-details")]
        public NativeMobileDashboardSections MobileConnectionDetails { get; }

        [JsonProperty("mobile-components-and-device-capabilities")]
        public NativeMobileDashboardSections MobileComponentsDeviceCapabilities { get; }

        [JsonProperty("mobile-hardware-requirements")]
        public NativeMobileDashboardSections MobileHardwareRequirements { get; }

        [JsonProperty("mobile-additional-information")]
        public NativeMobileDashboardSections MobileAdditionalInformation { get; }

        public NativeMobileSections()
        {
            MobileOperatingSystems = new NativeMobileDashboardSections(false, true);
            MobileFirst = new NativeMobileDashboardSections(true, false);
            MobileMemoryStorage = new NativeMobileDashboardSections(true, true);
            MobileConnectionDetails = new NativeMobileDashboardSections(false, true);
            MobileComponentsDeviceCapabilities = new NativeMobileDashboardSections(true, false);
            MobileHardwareRequirements = new NativeMobileDashboardSections(true, true);
            MobileAdditionalInformation = new NativeMobileDashboardSections(false, false);
        }
    }

    public class NativeMobileDashboardSections
    {
        private readonly bool _mandatory;
        private readonly bool _complete;

        [JsonProperty("status")]
        public string Status => _complete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("requirement")]
        public string Requirement => _mandatory ? "Mandatory" : "Optional";

        public NativeMobileDashboardSections(bool complete, bool mandatory)
        {
            _complete = complete;
            _mandatory = mandatory;
        }
    }
}
