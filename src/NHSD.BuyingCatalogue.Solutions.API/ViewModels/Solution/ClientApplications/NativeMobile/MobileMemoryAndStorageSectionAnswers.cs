using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile
{
    public sealed class MobileMemoryAndStorageSectionAnswers
    {
        public MobileMemoryAndStorageSectionAnswers(IClientApplication clientApplication)
        {
            MinimumMemoryRequirement = clientApplication?.MobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = clientApplication?.MobileMemoryAndStorage?.Description;
        }

        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; }

        [JsonProperty("storage-requirements-description")]
        public string Description { get; }

        [JsonIgnore]
        public bool HasData => MinimumMemoryRequirement?.Any() == true && Description?.Any() == true;
    }
}
