using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Hostings
{
    public sealed class UpdatePublicCloudViewModel : IUpdatePublicCloudData
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("link")]
        public string URL { get; set; }

        [JsonProperty("requires-hscn")]
        public HashSet<string> ConnectivityRequired { get; internal set; } = new HashSet<string>();
    }
}
