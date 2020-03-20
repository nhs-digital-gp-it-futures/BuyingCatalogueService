using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class LearnMoreSectionAnswers
    {
        public LearnMoreSectionAnswers(ISolutionDocument solutionDocument) => DocumentName = solutionDocument?.Name;

        [JsonProperty("document-name")]
        public string DocumentName { get; }

        internal bool HasData => !string.IsNullOrWhiteSpace(DocumentName);
    }
}
