using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hosting
{
    public sealed class PublicCloudSectionAnswers
    {
        public PublicCloudSectionAnswers(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            Link = publicCloud?.Link;
            RequiresHscn = publicCloud?.RequiresHscn;
        }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("requires-hscn")]
        public string RequiresHscn { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary)
            || !string.IsNullOrWhiteSpace(Link)
            || !string.IsNullOrWhiteSpace(RequiresHscn);
    }
}
