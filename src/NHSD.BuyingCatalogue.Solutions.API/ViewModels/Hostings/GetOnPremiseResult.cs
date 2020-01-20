using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetOnPremiseResult
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiresHSCN { get; }

        public GetOnPremiseResult(IOnPremise premise)
        {
            Summary = premise?.Summary;
            Link = premise?.Link;
            HostingModel = premise?.HostingModel;
            RequiresHSCN = premise?.RequiresHSCN != null
                ? new HashSet<string> { premise?.RequiresHSCN }
                : new HashSet<string>();
        }
    }
}
