using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator : IValidator<UpdateSolutionSummaryCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionSummaryCommand updateSolutionSummaryCommand)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionSummaryCommand.UpdateSolutionSummaryViewModel.Summary, "summary").Result(),
                new MaxLengthValidator()
                    .Validate(updateSolutionSummaryCommand.UpdateSolutionSummaryViewModel.Summary, 300, "summary")
                    .Validate(updateSolutionSummaryCommand.UpdateSolutionSummaryViewModel.Description, 1000, "description")
                    .Validate(updateSolutionSummaryCommand.UpdateSolutionSummaryViewModel.Link, 1000, "link")
                    .Result()
                );
    }
}
