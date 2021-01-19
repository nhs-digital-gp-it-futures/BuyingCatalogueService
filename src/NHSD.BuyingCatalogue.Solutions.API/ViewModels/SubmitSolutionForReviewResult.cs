using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.SubmitForReview;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public sealed class SubmitSolutionForReviewResult
    {
        private static readonly IDictionary<ValidationError, string> RequiredSectionMap = new Dictionary<ValidationError, string>
        {
            { SubmitSolutionForReviewErrors.SolutionSummaryIsRequired, "solution-description" },
            { SubmitSolutionForReviewErrors.ClientApplicationTypeIsRequired, "client-application-types" },
            { SubmitSolutionForReviewErrors.SupportedBrowserIsRequired, "browser-based" },
            { SubmitSolutionForReviewErrors.MobileResponsiveIsRequired, "browser-based" },
            { SubmitSolutionForReviewErrors.PluginRequirementIsRequired, "browser-based" },
        };

        private SubmitSolutionForReviewResult(HashSet<string> requiredSections)
        {
            RequiredSections = requiredSections;
        }

        [JsonProperty("required")]
        public HashSet<string> RequiredSections { get; }

        public static SubmitSolutionForReviewResult Create(IReadOnlyCollection<ValidationError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            return new SubmitSolutionForReviewResult(Map(errors));
        }

        private static HashSet<string> Map(IReadOnlyCollection<ValidationError> errors)
        {
            if (!errors.Any())
                return null;

            var requiredSectionList = new HashSet<string>();

            foreach (ValidationError error in errors)
            {
                if (RequiredSectionMap.TryGetValue(error, out string requiredSectionName))
                {
                    requiredSectionList.Add(requiredSectionName);
                }
            }

            return requiredSectionList;
        }
    }
}
