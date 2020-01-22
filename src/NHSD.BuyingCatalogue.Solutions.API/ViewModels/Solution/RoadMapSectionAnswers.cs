using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class RoadMapSectionAnswers
    {
        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary);

        public RoadMapSectionAnswers(string summary)
        {
            Summary = summary;
        }
    }
}
