using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetOnPremiseResult
    {
        public GetOnPremiseResult(IOnPremise premise)
        {
            Summary = premise?.Summary;
            Link = premise?.Link;
            HostingModel = premise?.HostingModel;
            RequiresHSCN = premise?.RequiresHSCN is not null
                ? new HashSet<string> { premise.RequiresHSCN }
                : new HashSet<string>();
        }

        [JsonProperty("summary")]
        public string Summary { get; }

        [JsonProperty("link")]
        public string Link { get; }

        [JsonProperty("hosting-model")]
        public string HostingModel { get; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> RequiresHSCN { get; }
    }
}
