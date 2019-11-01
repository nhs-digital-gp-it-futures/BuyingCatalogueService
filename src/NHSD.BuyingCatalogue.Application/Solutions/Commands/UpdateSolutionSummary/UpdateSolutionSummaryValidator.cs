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

            if (updateSolutionSummaryViewModel.Summary.Length > 300)
            {
                validationResult.MaxLength.Add("summary");
            }

            if (updateSolutionSummaryViewModel.Description.Length > 1000)
            {
                validationResult.MaxLength.Add("description");
            }

            if (updateSolutionSummaryViewModel.Link.Length > 1000)
            {
                validationResult.MaxLength.Add("link");
            }

            return validationResult;
        }
    }
}
