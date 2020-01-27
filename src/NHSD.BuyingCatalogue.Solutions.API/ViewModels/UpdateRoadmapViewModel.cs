using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateRoadmapViewModel
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }
    }
}
