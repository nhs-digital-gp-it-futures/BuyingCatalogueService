using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile
{
    public class NativeMobileResult
    {
        [JsonProperty("sections")]
        public NativeMobileSections NativeMobileSections { get; }

        public NativeMobileResult(IClientApplication clientApplication)
        {
            NativeMobileSections = new NativeMobileSections(clientApplication);
        }
    }

    public class NativeMobileSections
    {
        [JsonProperty("native-mobile-operating-systems")]
        public DashboardSection MobileOperatingSystems { get; }

        [JsonProperty("native-mobile-first")]
        public DashboardSection MobileFirst { get; }
        
        [JsonProperty("native-mobile-memory-and-storage")]
        public DashboardSection MobileMemoryStorage { get; }

        [JsonProperty("native-mobile-connection-details")]
        public DashboardSection MobileConnectionDetails { get; }

        [JsonProperty("native-mobile-components-and-device-capabilities")]
        public DashboardSection MobileComponentsDeviceCapabilities { get; }

        [JsonProperty("native-mobile-hardware-requirements")]
        public DashboardSection MobileHardwareRequirements { get; }

        [JsonProperty("native-mobile-third-party")]
        public DashboardSection MobileThirdPartySection { get; }

        [JsonProperty("native-mobile-additional-information")]
        public DashboardSection MobileAdditionalInformation { get; }

        public NativeMobileSections(IClientApplication clientApplication)
        {
            MobileOperatingSystems = DashboardSection.Mandatory(clientApplication.IsMobileOperatingSystems());
            MobileFirst = DashboardSection.Mandatory(clientApplication.IsNativeMobileFirstComplete());
            MobileMemoryStorage = DashboardSection.Mandatory(clientApplication.IsMobileMemoryAndStorageComplete());
            MobileConnectionDetails = DashboardSection.Optional(clientApplication.IsMobileConnectionDetailsComplete());
            MobileComponentsDeviceCapabilities = DashboardSection.Optional(false);
            MobileHardwareRequirements = DashboardSection.Optional(!String.IsNullOrWhiteSpace(clientApplication?.NativeMobileHardwareRequirements));
            MobileThirdPartySection = DashboardSection.Optional(clientApplication.IsMobileThirdPartyComplete());
            MobileAdditionalInformation = DashboardSection.Optional(false);
        }
    }
}
