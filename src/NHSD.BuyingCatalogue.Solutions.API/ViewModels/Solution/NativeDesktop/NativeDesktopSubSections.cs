using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopSubSections
    {
       [JsonProperty("native-desktop-hardware-requirements")]
       public NativeDesktopHardwareRequirementsSection HardwareRequirementsSection { get; }

       [JsonProperty("native-desktop-operating-systems")]
       public NativeDesktopOperatingSystemsSection OperatingSystemsSection { get; }

       [JsonProperty("native-desktop-connection-details")]
       public NativeDesktopConnectivityDetailsSection NativeDesktopConnectivityDetailsSection { get; }

       [JsonProperty("native-desktop-third-party")]
       public NativeDesktopThirdPartySection NativeDesktopThirdPartySection { get; }

       [JsonProperty("native-desktop-memory-and-storage")]
       public NativeDesktopMemoryAndStorageSection NativeDesktopMemoryAndStorageSection { get; }

        [JsonIgnore]
       public bool HasData => HardwareRequirementsSection.Answers.HasData ||
                              OperatingSystemsSection.Answers.HasData ||
                              NativeDesktopConnectivityDetailsSection.Answers.HasData ||
                              NativeDesktopThirdPartySection.Answers.HasData ||
                              NativeDesktopMemoryAndStorageSection.Answers.HasData;

       internal NativeDesktopSubSections(IClientApplication clientApplication)
       {
           HardwareRequirementsSection = new NativeDesktopHardwareRequirementsSection(clientApplication);
           OperatingSystemsSection = new NativeDesktopOperatingSystemsSection(clientApplication);
           NativeDesktopConnectivityDetailsSection = new NativeDesktopConnectivityDetailsSection(clientApplication);
           NativeDesktopThirdPartySection = new NativeDesktopThirdPartySection(clientApplication);
           NativeDesktopMemoryAndStorageSection = new NativeDesktopMemoryAndStorageSection(clientApplication);
       }
    }
}
