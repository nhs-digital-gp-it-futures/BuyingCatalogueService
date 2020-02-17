using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class SolutionDocumentSectionAnswers
    {
        public SolutionDocumentSectionAnswers(ISolutionDocument solutionDocument) => Name = solutionDocument?.Name;

        [JsonProperty("document-name")]
        public string Name { get; }

        internal bool HasData => !string.IsNullOrWhiteSpace(Name);
    }
}
