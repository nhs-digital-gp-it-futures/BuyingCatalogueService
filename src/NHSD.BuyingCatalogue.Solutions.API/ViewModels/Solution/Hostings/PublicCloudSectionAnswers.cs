using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class PublicCloudSectionAnswers
    {
        public PublicCloudSectionAnswers(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            Link = publicCloud?.Link;
            RequiresHSCN = publicCloud?.RequiresHSCN;
        }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("requires-hscn")]
        public string RequiresHSCN { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary)
            || !string.IsNullOrWhiteSpace(Link)
            || !string.IsNullOrWhiteSpace(RequiresHSCN);
    }
}
