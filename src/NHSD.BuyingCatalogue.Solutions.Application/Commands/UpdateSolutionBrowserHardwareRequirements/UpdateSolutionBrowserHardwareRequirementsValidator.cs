using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsValidator
    {
        public MaxLengthResult Validation(UpdateSolutionBrowserHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel)
            => new MaxLengthValidator()
                .Validate(updateSolutionHardwareRequirementsViewModel.HardwareRequirements, 500, "hardware-requirements-description")
                .Result();
    }
}
