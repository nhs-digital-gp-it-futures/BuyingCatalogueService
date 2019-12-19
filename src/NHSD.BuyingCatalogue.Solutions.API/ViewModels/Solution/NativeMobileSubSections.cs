using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class NativeMobileSubSections
    {
       [JsonProperty("mobile-operating-systems")]
       public MobileOperatingSystemsSection MobileOperatingSystemsSection { get; }

       [JsonIgnore]
       public bool HasData => MobileOperatingSystemsSection.Answers.HasData;

       internal NativeMobileSubSections(IClientApplication clientApplication)
       {
           MobileOperatingSystemsSection = new MobileOperatingSystemsSection(clientApplication);
       }
    }
}
