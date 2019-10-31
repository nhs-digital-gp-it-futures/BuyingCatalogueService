using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class SubmitSolutionForReviewResult
    {
        private static readonly IDictionary<ValidationError, string> RequiredSectionMap = new Dictionary<ValidationError, string>
        {
            { SubmitSolutionForReviewErrors.SolutionSummaryIsRequired, "solution-description" },
            { SubmitSolutionForReviewErrors.ClientApplicationTypeIsRequired, "client-application-types" },
            { SubmitSolutionForReviewErrors.SupportedBrowserIsRequired, "browser-based" },
            { SubmitSolutionForReviewErrors.MobileResponsiveIsRequired, "browser-based" }
        };

        [JsonProperty("required")]
        public HashSet<string> RequiredSections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewResult"/> class.
        /// </summary>
        private SubmitSolutionForReviewResult(HashSet<string> requiredSections)
        {
            RequiredSections = requiredSections;
        }

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
            HashSet<string> invalidSectionList = null;

            if (errors.Any())
            {
                invalidSectionList = new HashSet<string>();

                foreach (ValidationError error in errors)
                {
                    if (RequiredSectionMap.TryGetValue(error, out string invalidSectionName))
                    {
                        invalidSectionList.Add(invalidSectionName);
                    }
                }
            }

            return invalidSectionList;
        }

        protected bool Equals(SubmitSolutionForReviewResult other)
        {
            return RequiredSections is null && other.RequiredSections is null ||
                   RequiredSections != null && RequiredSections.SequenceEqual(other.RequiredSections);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            SubmitSolutionForReviewResult item = obj as SubmitSolutionForReviewResult;
            if (item is null)
            {
                return false;
            }

            return Equals(item);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return RequiredSections != null ? RequiredSections.GetHashCode() : 0;
        }
    }
}
