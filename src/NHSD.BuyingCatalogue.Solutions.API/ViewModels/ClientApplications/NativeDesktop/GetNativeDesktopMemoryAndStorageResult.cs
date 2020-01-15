using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopMemoryAndStorageResult
    {
        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; set; }

        [JsonProperty("storage-requirements-description")]
        public string StorageRequirementsDescription { get; set; }

        [JsonProperty("minimum-cpu")]
        public string MinimumCpu { get; set; }

        [JsonProperty("recommended-resolution")]
        public string RecommendedResolution { get; set; }

        public GetNativeDesktopMemoryAndStorageResult(INativeDesktopMemoryAndStorage memoryAndStorage)
        {
            MinimumMemoryRequirement = memoryAndStorage?.MinimumMemoryRequirement;
            StorageRequirementsDescription = memoryAndStorage?.StorageRequirementsDescription;
            MinimumCpu = memoryAndStorage?.MinimumCpu;
            RecommendedResolution = memoryAndStorage?.RecommendedResolution;
        }
    }
}
