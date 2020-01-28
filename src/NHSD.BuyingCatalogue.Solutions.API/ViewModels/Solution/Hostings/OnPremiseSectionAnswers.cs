using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class OnPremiseSectionAnswers
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public string RequiresHSCN { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary) ||
                               !string.IsNullOrWhiteSpace(Link) ||
                               !string.IsNullOrWhiteSpace(HostingModel) ||
                               !string.IsNullOrWhiteSpace(RequiresHSCN);

        public OnPremiseSectionAnswers(IOnPremise onPremise)
        {
            Summary = onPremise?.Summary;
            Link = onPremise?.Link;
            HostingModel = onPremise?.HostingModel;
            RequiresHSCN = onPremise?.RequiresHSCN;
        }
    }
}
