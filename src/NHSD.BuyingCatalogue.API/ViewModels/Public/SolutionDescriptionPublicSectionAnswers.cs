using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class SolutionDescriptionPublicSectionAnswers
    {
        public string Summary { get; }

        public string Description { get; }

        public string Link { get; }

        [JsonIgnore]
        public bool HasData => !(string.IsNullOrWhiteSpace(Summary)
                                 && string.IsNullOrWhiteSpace(Description)
                                 && string.IsNullOrWhiteSpace(Link));

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionDescriptionPublicSectionAnswers"/> class.
        /// </summary>
        public SolutionDescriptionPublicSectionAnswers(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Summary = solution.Summary;
            Description = solution.Description;
            Link = solution.AboutUrl;
        }
    }
}
