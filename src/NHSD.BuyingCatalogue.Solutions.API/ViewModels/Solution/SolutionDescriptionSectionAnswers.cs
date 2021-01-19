using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public sealed class SolutionDescriptionSectionAnswers
    {
        public SolutionDescriptionSectionAnswers(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Summary = solution.Summary;
            Description = solution.Description;
            Link = solution.AboutUrl;
        }

        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }

        [JsonIgnore]
        public bool HasData => !(string.IsNullOrWhiteSpace(Summary)
            && string.IsNullOrWhiteSpace(Description)
            && string.IsNullOrWhiteSpace(Link));
    }
}
