using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator
    {
        public RequiredMaxLengthResult Validate(UpdateSolutionSummaryViewModel updateSolutionSummaryViewModel)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionSummaryViewModel.Summary, "summary").Result(),
                new MaxLengthValidator()
                    .Validate(updateSolutionSummaryViewModel.Summary, 300, "summary")
                    .Validate(updateSolutionSummaryViewModel.Description, 1000, "description")
                    .Validate(updateSolutionSummaryViewModel.Link, 1000, "link")
                    .Result()
                );
    }
}
