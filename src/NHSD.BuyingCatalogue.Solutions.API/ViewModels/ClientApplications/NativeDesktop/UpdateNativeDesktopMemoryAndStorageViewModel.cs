using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
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
    }
}
