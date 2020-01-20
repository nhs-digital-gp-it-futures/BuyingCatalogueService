using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetPublicCloudResult
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string URL { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> ConnectivityRequired { get; }

        public GetPublicCloudResult(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            URL = publicCloud?.URL;
            ConnectivityRequired = publicCloud?.ConnectivityRequired != null
                ? new HashSet<string> { publicCloud?.ConnectivityRequired }
                : new HashSet<string>();
        }
    }
}
