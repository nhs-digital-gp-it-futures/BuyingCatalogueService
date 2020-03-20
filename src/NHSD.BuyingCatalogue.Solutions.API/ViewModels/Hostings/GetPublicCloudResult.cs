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
        public string Link { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiredHscn { get; }

        public GetPublicCloudResult(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            Link = publicCloud?.Link;
            RequiredHscn = publicCloud?.RequiresHSCN != null
                ? new HashSet<string> { publicCloud?.RequiresHSCN }
                : new HashSet<string>();
        }
    }
}
