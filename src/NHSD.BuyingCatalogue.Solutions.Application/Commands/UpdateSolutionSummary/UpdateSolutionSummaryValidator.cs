using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    internal sealed class UpdateSolutionSummaryValidator : IValidator<UpdateSolutionSummaryCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionSummaryCommand command)
            => new RequiredMaxLengthResult(
                new RequiredValidator().Validate(command.Data.Summary, "summary").Result(),
                new MaxLengthValidator()
                    .Validate(command.Data.Summary, 300, "summary")
                    .Validate(command.Data.Description, 1000, "description")
                    .Validate(command.Data.Link, 1000, "link")
                    .Result()
                );
    }
}
