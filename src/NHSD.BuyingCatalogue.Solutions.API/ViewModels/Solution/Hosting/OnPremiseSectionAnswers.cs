using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hosting
{
    public sealed class OnPremiseSectionAnswers
    {
        public OnPremiseSectionAnswers(IOnPremise onPremise)
        {
            Summary = onPremise?.Summary;
            Link = onPremise?.Link;
            HostingModel = onPremise?.HostingModel;
            RequiresHscn = onPremise?.RequiresHscn;
        }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public string RequiresHscn { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary)
            || !string.IsNullOrWhiteSpace(Link)
            || !string.IsNullOrWhiteSpace(HostingModel)
            || !string.IsNullOrWhiteSpace(RequiresHscn);
    }
}
