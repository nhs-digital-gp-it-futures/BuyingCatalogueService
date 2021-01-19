﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class NativeMobileSections
    {
        public NativeMobileSections(IClientApplication clientApplication)
        {
            MobileOperatingSystems = DashboardSection.Mandatory(clientApplication.IsNativeMobileOperatingSystemsComplete());
            MobileFirst = DashboardSection.Mandatory(clientApplication.IsNativeMobileFirstComplete());
            MobileMemoryStorage = DashboardSection.Mandatory(clientApplication.IsNativeMobileMemoryAndStorageComplete());
            MobileConnectionDetails = DashboardSection.Optional(clientApplication.IsMobileConnectionDetailsComplete());
            MobileComponentsDeviceCapabilities = DashboardSection.Optional(false);
            MobileHardwareRequirements = DashboardSection.Optional(!string.IsNullOrWhiteSpace(clientApplication?.NativeMobileHardwareRequirements));
            MobileThirdPartySection = DashboardSection.Optional(clientApplication.IsMobileThirdPartyComplete());
            MobileAdditionalInformation = DashboardSection.Optional(clientApplication.IsNativeMobileAdditionalInformationComplete());
        }

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
    }
}
