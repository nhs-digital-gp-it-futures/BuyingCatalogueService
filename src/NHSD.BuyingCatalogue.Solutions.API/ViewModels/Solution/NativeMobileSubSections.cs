using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class NativeMobileSubSections
    {
       [JsonProperty("mobile-operating-systems")]
       public MobileOperatingSystemsSection MobileOperatingSystemsSection { get; }

       [JsonProperty("mobile-connection-details")]
       public MobileConnectionDetailsSection MobileConnectionDetailsSection { get; }

       [JsonProperty("mobile-memory-and-storage")]
       public MobileMemoryAndStorageSection MobileMemoryAndStorageSection { get; }

       [JsonIgnore]
       public bool HasData => MobileOperatingSystemsSection.Answers.HasData ||
                              MobileConnectionDetailsSection.Answers.HasData ||
                              MobileMemoryAndStorageSection.Answers.HasData;

       internal NativeMobileSubSections(IClientApplication clientApplication)
       {
           MobileOperatingSystemsSection = new MobileOperatingSystemsSection(clientApplication);
           MobileConnectionDetailsSection = new MobileConnectionDetailsSection(clientApplication);
           MobileMemoryAndStorageSection = new MobileMemoryAndStorageSection(clientApplication);
       }
    }
}
