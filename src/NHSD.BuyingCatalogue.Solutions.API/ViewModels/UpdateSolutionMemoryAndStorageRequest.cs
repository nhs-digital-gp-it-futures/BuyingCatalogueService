using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateSolutionMemoryAndStorageRequest
    {
        [JsonProperty("minimum-memory-requirement")]
        public string MinimumMemoryRequirement { get; set; }

        [JsonProperty("storage-requirements-description")]
        public string Description { get; set; }
    }
}
