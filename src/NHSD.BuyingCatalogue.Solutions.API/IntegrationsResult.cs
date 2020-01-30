using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API
{
    public sealed class IntegrationsResult
    {
        [JsonProperty("link")]
        public string Url { get; }

        public IntegrationsResult(string link)
        {
            Url = link;
        }
    }
}
