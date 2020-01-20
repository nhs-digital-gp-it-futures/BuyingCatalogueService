using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetHybridHostingTypeResult
    {
        [JsonProperty("summary")] public string Summary { get; set; }

        [JsonProperty("link")] public string Url { get; set; }

        [JsonProperty("hosting-model")] public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")] public HashSet<string> ConnectivityRequired { get; }

        public GetHybridHostingTypeResult(IHybridHostingType hybridHostingType)
        {
            Summary = hybridHostingType?.Summary;
            Url = hybridHostingType?.Url;
            HostingModel = hybridHostingType?.HostingModel;
            ConnectivityRequired = hybridHostingType?.ConnectivityRequired != null
                ? new HashSet<string> {hybridHostingType?.ConnectivityRequired}
                : new HashSet<string>();
        }
    }
}
