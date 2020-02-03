using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSectionAnswers
    {
        [JsonProperty("link")]
        public string Integrations { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(Integrations);

        public IntegrationsSectionAnswers(string integrationsUrl)
        {
            Integrations = integrationsUrl;
        }
    }
}
