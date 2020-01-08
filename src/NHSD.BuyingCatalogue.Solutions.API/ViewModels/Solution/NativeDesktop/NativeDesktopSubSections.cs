using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public sealed class NativeDesktopSubSections
    {
       [JsonProperty("native-desktop-hardware-requirements")]
       public NativeDesktopHardwareRequirementsSection HardwareRequirementsSection { get; }

       [JsonProperty("native-desktop-connection-details")]
       public NativeDesktopConnectivityDetailsSection NativeDesktopConnectivityDetailsSection { get; }

       [JsonIgnore]
       public bool HasData => HardwareRequirementsSection.Answers.HasData ||
                              NativeDesktopConnectivityDetailsSection.Answers.HasData;

       internal NativeDesktopSubSections(IClientApplication clientApplication)
       {
           HardwareRequirementsSection = new NativeDesktopHardwareRequirementsSection(clientApplication);
           NativeDesktopConnectivityDetailsSection = new NativeDesktopConnectivityDetailsSection(clientApplication);
       }
    }
}
