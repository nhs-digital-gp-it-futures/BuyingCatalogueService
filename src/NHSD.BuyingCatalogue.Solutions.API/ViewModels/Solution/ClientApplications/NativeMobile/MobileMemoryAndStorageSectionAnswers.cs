using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileMemoryAndStorageSectionAnswers
    {
        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; set; }

        [JsonProperty("storage-requirements-description")]
        public string Description { get; set; }

        [JsonIgnore]
        public bool HasData => MinimumMemoryRequirement?.Any() == true && Description?.Any() == true;

        public MobileMemoryAndStorageSectionAnswers(IClientApplication clientApplication)
        {
            MinimumMemoryRequirement = clientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = clientApplication?.MobileMemoryAndStorage?.Description;
        }
    }
}
