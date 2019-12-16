using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator
    {
        public RequiredMaxLengthResult Validate(UpdateSolutionSummaryViewModel updateSolutionSummaryViewModel)
        {
            var validationResult = new RequiredMaxLengthResult();

            if (string.IsNullOrWhiteSpace(updateSolutionSummaryViewModel.Summary))
            {
                validationResult.Required.Add("summary");
            }

            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Summary, 300, "summary");
            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Description, 1000, "description");
            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Link, 1000, "link");

            return validationResult;
        }

        private static void ValidateLength(HashSet<string> maxLength, string candidate, int length, string description)
        {
            if ((candidate?.Length ?? 0) > length)
            {
                maxLength.Add(description);
            }
        }
    }
}
