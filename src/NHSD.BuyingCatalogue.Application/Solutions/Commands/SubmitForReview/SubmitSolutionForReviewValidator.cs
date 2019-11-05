using System;
using System.Linq;
using NHSD.BuyingCatalogue.Application.Solutions.Domain;
using NHSD.BuyingCatalogue.Contracts;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview
{
    internal class SubmitSolutionForReviewValidator
    {
        public Solution Solution { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubmitSolutionForReviewValidator"/> class.
        /// </summary>
        public SubmitSolutionForReviewValidator(Solution solution)
        {
            Solution = solution ?? throw new ArgumentNullException(nameof(solution));
        }

        /// <summary>
        /// Validates the details of the current context.
        /// </summary>
        /// <returns>A list of errors.</returns>
        internal ValidationResult Validate()
        {
            ValidationResult validationResult = new ValidationResult();

            validationResult.Add(ValidateSolutionSummary());
            validationResult.Add(ValidateClientApplicationTypes());
            validationResult.Add(ValidateBrowserBased());

            return validationResult;
        }

        private ValidationResult ValidateSolutionSummary()
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Solution.Summary))
            {
                result.Add(SubmitSolutionForReviewErrors.SolutionSummaryIsRequired);
            }

            return result;
        }

        private ValidationResult ValidateClientApplicationTypes()
        {
            ValidationResult result = new ValidationResult();

            var clientApplication = Solution.ClientApplication;
            if (clientApplication == null || !clientApplication.ClientApplicationTypes.Any())
            {
                result.Add(SubmitSolutionForReviewErrors.ClientApplicationTypeIsRequired);
            }

            return result;
        }

        private ValidationResult ValidateBrowserBased()
        {
            ValidationResult result = new ValidationResult();

            var clientApplication = Solution.ClientApplication;
            if (clientApplication != null && clientApplication.ClientApplicationTypes.Contains("browser-based"))
            {
                result.Add(ValidateSupportedBrowsers(clientApplication))
                      .Add(ValidateMobileResponsive(clientApplication));
            }

            return result;
        }

        private ValidationResult ValidateSupportedBrowsers(IClientApplication clientApplication)
        {
            ValidationResult result = new ValidationResult();

            if (!clientApplication.BrowsersSupported.Any())
            {
                result.Add(SubmitSolutionForReviewErrors.SupportedBrowserIsRequired);
            }

            return result;
        }

        private ValidationResult ValidateMobileResponsive(IClientApplication clientApplication)
        {
            ValidationResult result = new ValidationResult();

            if (!clientApplication.MobileResponsive.HasValue)
            {
                result.Add(SubmitSolutionForReviewErrors.MobileResponsiveIsRequired);
            }

            return result;
        }
    }
}
