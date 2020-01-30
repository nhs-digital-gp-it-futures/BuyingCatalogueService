using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class RoadMapSectionAnswers
    {
        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonProperty("hasDocument")]
        public bool? HasDocument { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary);

        public RoadMapSectionAnswers(IRoadMap roadMap)
        {
            Summary = roadMap?.Summary;
            HasDocument = roadMap?.HasDocument;
        }
    }
}
