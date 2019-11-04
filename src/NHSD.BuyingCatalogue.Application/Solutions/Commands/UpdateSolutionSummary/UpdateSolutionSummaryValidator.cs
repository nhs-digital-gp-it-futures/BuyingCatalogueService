using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator
    {
        public UpdateSolutionSummaryValidationResult Validate(UpdateSolutionSummaryViewModel updateSolutionSummaryViewModel)
        {
            var validationResult = new UpdateSolutionSummaryValidationResult();

            if (string.IsNullOrWhiteSpace(updateSolutionSummaryViewModel.Summary))
            {
                validationResult.Required.Add("summary");
            }

            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Summary, 300, "summary");
            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Description, 1000, "description");
            ValidateLength(validationResult.MaxLength, updateSolutionSummaryViewModel.Link, 1000, "link");

            return validationResult;
        }

        private void ValidateLength(HashSet<string> maxLength, string candidate, int length, string description)
        {
            if ((candidate?.Length ?? 0) > length)
            {
                maxLength.Add(description);
            }
        }
    }
}
