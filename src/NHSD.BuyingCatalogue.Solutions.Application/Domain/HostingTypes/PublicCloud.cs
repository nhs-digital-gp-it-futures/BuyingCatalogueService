using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.HostingTypes
{
    internal sealed class PublicCloud
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        [JsonProperty("RequiresHSCN")]
        public string RequiresHscn { get; set; }
    }
}
