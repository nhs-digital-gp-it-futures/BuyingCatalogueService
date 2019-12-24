using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeMobile
{
    public class NativeMobileSubSections
    {
       [JsonProperty("native-mobile-operating-systems")]
       public MobileOperatingSystemsSection MobileOperatingSystemsSection { get; }

       [JsonProperty("native-mobile-first")]
       public NativeMobileFirstSection NativeMobileFirstSection { get; }

       [JsonProperty("native-mobile-connection-details")]
       public MobileConnectionDetailsSection MobileConnectionDetailsSection { get; }

       [JsonProperty("native-mobile-memory-and-storage")]
       public MobileMemoryAndStorageSection MobileMemoryAndStorageSection { get; }

       [JsonIgnore]
       public bool HasData => MobileOperatingSystemsSection.Answers.HasData
                              || NativeMobileFirstSection.Answers.HasData
                              || MobileConnectionDetailsSection.Answers.HasData
                              || MobileMemoryAndStorageSection.Answers.HasData;

       internal NativeMobileSubSections(IClientApplication clientApplication)
       {
           MobileOperatingSystemsSection = new MobileOperatingSystemsSection(clientApplication);
           NativeMobileFirstSection = new NativeMobileFirstSection(clientApplication);
           MobileConnectionDetailsSection = new MobileConnectionDetailsSection(clientApplication);
           MobileMemoryAndStorageSection = new MobileMemoryAndStorageSection(clientApplication);
       }
    }
}
