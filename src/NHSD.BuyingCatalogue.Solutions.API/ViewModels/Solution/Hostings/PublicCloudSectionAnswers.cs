using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Hostings
{
    public sealed class PublicCloudSectionAnswers
    {
        [JsonProperty("summary")] public string Summary { get; set; }

        [JsonProperty("link")] public string URL { get; set; }

        [JsonProperty("requires-hscn")] public string ConnectivityRequired { get; set; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Summary) ||
                               !string.IsNullOrWhiteSpace(URL) ||
                               !string.IsNullOrWhiteSpace(ConnectivityRequired);

        public PublicCloudSectionAnswers(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            URL = publicCloud?.URL;
            ConnectivityRequired = publicCloud?.ConnectivityRequired;
        }
    }
}
