using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class NativeMobileSubSections
    {
        internal NativeMobileSubSections(IClientApplication clientApplication)
        {
            MobileOperatingSystemsSection = new MobileOperatingSystemsSection(clientApplication);
            NativeMobileFirstSection = new NativeMobileFirstSection(clientApplication);
            MobileConnectionDetailsSection = new MobileConnectionDetailsSection(clientApplication);
            MobileMemoryAndStorageSection = new MobileMemoryAndStorageSection(clientApplication);
            HardwareRequirementsSection = new MobileHardwareRequirementsSection(clientApplication);
            MobileThirdPartySection = new MobileThirdPartySection(clientApplication);
            MobileAdditionalInformationSection = new MobileAdditionalInformationSection(clientApplication);
        }

        [JsonProperty("native-mobile-operating-systems")]
        public MobileOperatingSystemsSection MobileOperatingSystemsSection { get; }

        [JsonProperty("native-mobile-first")]
        public NativeMobileFirstSection NativeMobileFirstSection { get; }

        [JsonProperty("native-mobile-connection-details")]
        public MobileConnectionDetailsSection MobileConnectionDetailsSection { get; }

        [JsonProperty("native-mobile-memory-and-storage")]
        public MobileMemoryAndStorageSection MobileMemoryAndStorageSection { get; }

        [JsonProperty("native-mobile-hardware-requirements")]
        public MobileHardwareRequirementsSection HardwareRequirementsSection { get; }

        [JsonProperty("native-mobile-third-party")]
        public MobileThirdPartySection MobileThirdPartySection { get; }

        [JsonProperty("native-mobile-additional-information")]
        public MobileAdditionalInformationSection MobileAdditionalInformationSection { get; }

        [JsonIgnore]
        public bool HasData => MobileOperatingSystemsSection.Answers.HasData
            || NativeMobileFirstSection.Answers.HasData
            || MobileConnectionDetailsSection.Answers.HasData
            || MobileMemoryAndStorageSection.Answers.HasData
            || HardwareRequirementsSection.Answers.HasData
            || MobileThirdPartySection.Answers.HasData
            || MobileAdditionalInformationSection.Answers.HasData;
    }
}
