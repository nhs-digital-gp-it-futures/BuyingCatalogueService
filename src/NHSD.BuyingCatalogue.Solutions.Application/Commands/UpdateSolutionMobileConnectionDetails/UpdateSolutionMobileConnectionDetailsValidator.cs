using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileConnectionDetails
{
    class UpdateSolutionMobileConnectionDetailsValidator
    {
        public MaxLengthResult Validate(UpdateSolutionMobileConnectionDetailsViewModel updateSolutionFeaturesViewModel)
        {
            var validator = new MaxLengthValidator();
            validator.Validate(updateSolutionFeaturesViewModel.ConnectionRequirementsDescription, 300, "connection-requirements-description");
            return validator.Result();
        }
    }
}
