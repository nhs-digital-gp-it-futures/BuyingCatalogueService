using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    public sealed class RoadMapResult
    {
        [JsonProperty("summary")]
        public string Summary { get; }

        public RoadMapResult(string summary)
        {
            Summary = summary;
        }
    }
}
