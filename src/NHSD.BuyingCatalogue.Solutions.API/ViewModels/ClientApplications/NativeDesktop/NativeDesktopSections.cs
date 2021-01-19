﻿using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopSections
    {
        public NativeDesktopSections(IClientApplication clientApplication)
        {
            OperatingSystems = DashboardSection.Mandatory(clientApplication.IsNativeDesktopOperatingSystemsComplete());
            ConnectionDetails = DashboardSection.Mandatory(clientApplication.IsNativeDesktopConnectionDetailsComplete());
            MemoryAndStorage = DashboardSection.Mandatory(clientApplication.IsNativeDesktopMemoryAndStorageComplete());
            ThirdParty = DashboardSection.Optional(clientApplication.IsNativeDesktopThirdPartyComplete());
            HardwareRequirements = DashboardSection.Optional(!string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopHardwareRequirements));
            AdditionalInformation = DashboardSection.Optional(!string.IsNullOrWhiteSpace(clientApplication?.NativeDesktopAdditionalInformation));
        }

        [JsonProperty("native-desktop-operating-systems")]
        public DashboardSection OperatingSystems { get; }

        [JsonProperty("native-desktop-connection-details")]
        public DashboardSection ConnectionDetails { get; }

        [JsonProperty("native-desktop-memory-and-storage")]
        public DashboardSection MemoryAndStorage { get; }

        [JsonProperty("native-desktop-third-party")]
        public DashboardSection ThirdParty { get; }

        [JsonProperty("native-desktop-hardware-requirements")]
        public DashboardSection HardwareRequirements { get; }

        [JsonProperty("native-desktop-additional-information")]
        public DashboardSection AdditionalInformation { get; }
    }
}
