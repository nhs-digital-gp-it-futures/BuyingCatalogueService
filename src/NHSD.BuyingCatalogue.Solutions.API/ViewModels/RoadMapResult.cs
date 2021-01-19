using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class RoadMapResult
    {
        public RoadMapResult(string summary)
        {
            Summary = summary;
        }

        [JsonProperty("summary")]
        public string Summary { get; }
    }
}
