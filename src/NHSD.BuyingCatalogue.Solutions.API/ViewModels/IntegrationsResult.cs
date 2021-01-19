using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class IntegrationsResult
    {
        public IntegrationsResult(string link)
        {
            Url = link;
        }

        [JsonProperty("link")]
        public string Url { get; }
    }
}
