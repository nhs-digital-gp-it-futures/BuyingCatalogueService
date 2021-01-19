using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class GetNativeDesktopMemoryAndStorageResult
    {
        public GetNativeDesktopMemoryAndStorageResult(INativeDesktopMemoryAndStorage memoryAndStorage)
        {
            MinimumMemoryRequirement = memoryAndStorage?.MinimumMemoryRequirement;
            StorageRequirementsDescription = memoryAndStorage?.StorageRequirementsDescription;
            MinimumCpu = memoryAndStorage?.MinimumCpu;
            RecommendedResolution = memoryAndStorage?.RecommendedResolution;
        }

        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; }

        [JsonProperty("storage-requirements-description")]
        public string StorageRequirementsDescription { get; }

        [JsonProperty("minimum-cpu")]
        public string MinimumCpu { get; }

        [JsonProperty("recommended-resolution")]
        public string RecommendedResolution { get; }
    }
}
