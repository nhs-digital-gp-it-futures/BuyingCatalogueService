using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class UpdateIntegrationsViewModel
    {
        [JsonProperty("link")]
        public string Url { get; set; }
    }
}
