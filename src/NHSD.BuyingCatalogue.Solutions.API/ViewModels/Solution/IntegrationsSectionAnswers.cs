using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSectionAnswers
    {
        [JsonProperty("link")]
        public string IntegrationsUrl { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(IntegrationsUrl);

        public IntegrationsSectionAnswers(string integrationsUrl)
        {
            IntegrationsUrl = integrationsUrl;
        }
    }
}
