using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator : IValidator<UpdateSolutionClientApplicationTypesCommand, RequiredResult>
    {
        public RequiredResult Validate(UpdateSolutionClientApplicationTypesCommand updateSolutionClientApplicationTypesCommand)
            => new RequiredValidator()
                .Validate(updateSolutionClientApplicationTypesCommand.UpdateSolutionClientApplicationTypesViewModel.FilteredClientApplicationTypes, "client-application-types")
                .Result();
    }
}
