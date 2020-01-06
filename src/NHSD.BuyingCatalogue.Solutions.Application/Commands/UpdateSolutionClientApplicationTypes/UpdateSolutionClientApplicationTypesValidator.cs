using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator : IValidator<UpdateSolutionClientApplicationTypesCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionClientApplicationTypesCommand updateSolutionClientApplicationTypesCommand)
            => new RequiredValidator()
                .Validate(updateSolutionClientApplicationTypesCommand.Data.FilteredClientApplicationTypes, "client-application-types")
                .Result();
    }
}
