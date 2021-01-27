using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class PrivateCloud
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public string HostingModel { get; set; }

        [JsonProperty("RequiresHSCN")]
        public string RequiresHscn { get; set; }
    }
}
