using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeMobile
{
    public sealed class GetSolutionMemoryAndStorageResult
    {
        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; set; }

        [JsonProperty("storage-requirements-description")]
        public string Description { get; set; }

        public GetSolutionMemoryAndStorageResult(IMobileMemoryAndStorage mobileMemoryAndStorage)
        {
            MinimumMemoryRequirement = mobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = mobileMemoryAndStorage?.Description;
        }
    }
}
