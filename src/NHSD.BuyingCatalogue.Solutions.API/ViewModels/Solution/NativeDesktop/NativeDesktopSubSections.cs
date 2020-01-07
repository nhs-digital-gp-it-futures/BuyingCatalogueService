using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop
{
    public class NativeDesktopSubSections
    {
       [JsonProperty("native-desktop-hardware-requirements")]
       public NativeDesktopHardwareRequirementsSection HardwareRequirementsSection { get; }

       [JsonIgnore]
       public bool HasData => HardwareRequirementsSection.Answers.HasData;

       internal NativeDesktopSubSections(IClientApplication clientApplication)
       {
           HardwareRequirementsSection = new NativeDesktopHardwareRequirementsSection(clientApplication);
       }
    }
}
