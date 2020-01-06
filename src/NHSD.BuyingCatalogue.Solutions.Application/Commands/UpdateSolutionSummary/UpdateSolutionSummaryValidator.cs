using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator : IValidator<UpdateSolutionSummaryCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionSummaryCommand updateSolutionSummaryCommand)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(updateSolutionSummaryCommand.Data.Summary, "summary").Result(),
                new MaxLengthValidator()
                    .Validate(updateSolutionSummaryCommand.Data.Summary, 300, "summary")
                    .Validate(updateSolutionSummaryCommand.Data.Description, 1000, "description")
                    .Validate(updateSolutionSummaryCommand.Data.Link, 1000, "link")
                    .Result()
                );
    }
}
