using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class UpdateNativeDesktopMemoryAndStorageViewModel : IUpdateNativeDesktopMemoryAndStorageData
    {
        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; set; }

        [JsonProperty("storage-requirements-description")]
        public string StorageRequirementsDescription { get; set; }

        [JsonProperty("minimum-cpu")]
        public string MinimumCpu { get; set; }

        [JsonProperty("recommended-resolution")]
        public string RecommendedResolution { get; set; }

        public IUpdateNativeDesktopMemoryAndStorageData Trim()
        {
            return new UpdateNativeDesktopMemoryAndStorageViewModel
            {
                MinimumMemoryRequirement = MinimumMemoryRequirement?.Trim(),
                StorageRequirementsDescription = StorageRequirementsDescription?.Trim(),
                MinimumCpu = MinimumCpu?.Trim(),
                RecommendedResolution = RecommendedResolution?.Trim()
            };
        }
    }
}
