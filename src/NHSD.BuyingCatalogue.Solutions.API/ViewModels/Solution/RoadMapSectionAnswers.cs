using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class RoadMapSectionAnswers
    {
        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonProperty("documentName")]
        public string DocumentName { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary) || !string.IsNullOrWhiteSpace(DocumentName);

        public RoadMapSectionAnswers(IRoadMap roadMap)
        {
            Summary = roadMap?.Summary;
            DocumentName = roadMap?.DocumentName;
        }
    }
}
