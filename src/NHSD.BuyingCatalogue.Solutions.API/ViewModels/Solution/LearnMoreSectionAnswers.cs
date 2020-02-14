using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class LearnMoreSectionAnswers
    {
        public LearnMoreSectionAnswers(ILearnMore learnMore) => DocumentName = learnMore?.DocumentName;

        [JsonProperty("document-name")]
        public string DocumentName { get; }

        internal bool HasData => !string.IsNullOrWhiteSpace(DocumentName);
    }
}
