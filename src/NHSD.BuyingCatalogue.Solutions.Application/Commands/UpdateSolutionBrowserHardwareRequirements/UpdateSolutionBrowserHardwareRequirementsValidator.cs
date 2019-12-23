using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsValidator : IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult>
    {
        public MaxLengthResult Validate(UpdateSolutionBrowserHardwareRequirementsCommand updateSolutionHardwareRequirementsCommand)
            => new MaxLengthValidator()
                .Validate(updateSolutionHardwareRequirementsCommand.UpdateSolutionHardwareRequirementsViewModel.HardwareRequirements, 500, "hardware-requirements-description")
                .Result();
    }
}
