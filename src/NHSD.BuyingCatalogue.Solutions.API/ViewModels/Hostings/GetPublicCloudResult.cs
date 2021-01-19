using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetPublicCloudResult
    {
        public GetPublicCloudResult(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            Link = publicCloud?.Link;
            RequiredHscn = publicCloud?.RequiresHSCN is not null
                ? new HashSet<string> { publicCloud.RequiresHSCN }
                : new HashSet<string>();
        }

        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonProperty("link")]
        public string Link { get; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiredHscn { get; }
    }
}
