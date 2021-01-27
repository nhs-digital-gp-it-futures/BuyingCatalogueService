using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting
{
    public sealed class GetPrivateCloudResult
    {
        public GetPrivateCloudResult(IPrivateCloud privateCloud)
        {
            Summary = privateCloud?.Summary;
            Link = privateCloud?.Link;
            HostingModel = privateCloud?.HostingModel;
            RequiredHscn = privateCloud?.RequiresHscn is not null
                ? new HashSet<string> { privateCloud.RequiresHscn }
                : new HashSet<string>();
        }

        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonProperty("link")]
        public string Link { get; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiredHscn { get; }
    }
}
