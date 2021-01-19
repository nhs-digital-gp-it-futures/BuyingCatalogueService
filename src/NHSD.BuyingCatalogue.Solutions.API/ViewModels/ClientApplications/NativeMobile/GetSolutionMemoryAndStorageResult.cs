using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class GetSolutionMemoryAndStorageResult
    {
        public GetSolutionMemoryAndStorageResult(IMobileMemoryAndStorage mobileMemoryAndStorage)
        {
            MinimumMemoryRequirement = mobileMemoryAndStorage?.MinimumMemoryRequirement;
            Description = mobileMemoryAndStorage?.Description;
        }

        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; }

        [JsonProperty("storage-requirements-description")]
        public string Description { get; }
    }
}
