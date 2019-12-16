using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    internal sealed class UpdateSolutionClientApplicationTypesValidator
    {
        public RequiredResult Validate(UpdateSolutionClientApplicationTypesViewModel updateSolutionClientApplicationTypesViewModel)
            => new RequiredValidator()
                .Validate(updateSolutionClientApplicationTypesViewModel.FilteredClientApplicationTypes, "client-application-types")
                .Result();
    }
}
