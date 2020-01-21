using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateRoadmapViewModel
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
