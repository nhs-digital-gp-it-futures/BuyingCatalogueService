using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class RoadMapResult
    {
        [JsonProperty("description")]
        public string Description { get; }

        public RoadMapResult(string description)
        {
            Description = description;
        }
    }
}
