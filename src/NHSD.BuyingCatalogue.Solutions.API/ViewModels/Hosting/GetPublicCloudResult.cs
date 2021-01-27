using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hosting;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hosting
{
    public sealed class GetPublicCloudResult
    {
        public GetPublicCloudResult(IPublicCloud publicCloud)
        {
            Summary = publicCloud?.Summary;
            Link = publicCloud?.Link;
            RequiredHscn = publicCloud?.RequiresHscn is not null
                ? new HashSet<string> { publicCloud.RequiresHscn }
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
