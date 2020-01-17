using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator : IValidator<UpdateSolutionClientApplicationTypesCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionClientApplicationTypesCommand command)
            => new RequiredValidator()
                .Validate(command.Data.FilteredClientApplicationTypes, "client-application-types")
                .Result();
    }
}
