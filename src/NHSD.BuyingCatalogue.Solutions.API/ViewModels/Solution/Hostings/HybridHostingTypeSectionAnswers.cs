using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class HybridHostingTypeSectionAnswers
    {
        public HybridHostingTypeSectionAnswers(IHybridHostingType hybridHostingType)
        {
            Summary = hybridHostingType?.Summary;
            Link = hybridHostingType?.Link;
            HostingModel = hybridHostingType?.HostingModel;
            RequiresHSCN = hybridHostingType?.RequiresHSCN;
        }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public string RequiresHSCN { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary)
            || !string.IsNullOrWhiteSpace(Link)
            || !string.IsNullOrWhiteSpace(HostingModel)
            || !string.IsNullOrWhiteSpace(RequiresHSCN);
    }
}
