using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class GetHostingTypeHybridResult
    {
        [JsonProperty("summary")] public string Summary { get; set; }

        [JsonProperty("link")] public string Url { get; set; }

        [JsonProperty("hosting-model")] public string HostingModel { get; set; }

        [JsonProperty("requires-hscn")] public HashSet<string> ConnectivityRequired { get; set; }

        public GetHostingTypeHybridResult(IHostingTypeHybrid hostingTypeHybrid)
        {
            Summary = hostingTypeHybrid?.Summary;
            Url = hostingTypeHybrid?.Url;
            HostingModel = hostingTypeHybrid?.HostingModel;
            ConnectivityRequired = hostingTypeHybrid?.ConnectivityRequired != null
                ? new HashSet<string> {hostingTypeHybrid?.ConnectivityRequired}
                : new HashSet<string>();
        }
    }
}
