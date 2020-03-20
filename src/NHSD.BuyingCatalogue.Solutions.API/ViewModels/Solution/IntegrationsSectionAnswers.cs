using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class IntegrationsSectionAnswers
    {
        [JsonProperty("link")]
        public string IntegrationsUrl { get; }

        [JsonProperty("document-name")]
        public string DocumentName { get; }

        [JsonIgnore]
        public bool HasData => !string.IsNullOrWhiteSpace(IntegrationsUrl) || !string.IsNullOrWhiteSpace(DocumentName);

        public IntegrationsSectionAnswers(IIntegrations integration)
        {
            IntegrationsUrl = integration?.Url;
            DocumentName = integration?.DocumentName;
        }
    }
}
