using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetPrivateCloudResult
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiredHscn { get; }

        public GetPrivateCloudResult(IPrivateCloud privateCloud)
        {
            Summary = privateCloud?.Summary;
            Link = privateCloud?.Link;
            HostingModel = privateCloud?.HostingModel;
            RequiredHscn = privateCloud?.RequiresHSCN != null
                ? new HashSet<string> { privateCloud?.RequiresHSCN }
                : new HashSet<string>();
        }
    }
}
