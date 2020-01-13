using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsValidator : IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowserHardwareRequirementsCommand updateSolutionHardwareRequirementsCommand)
            => new MaxLengthValidator()
                .Validate(updateSolutionHardwareRequirementsCommand.HardwareRequirements, 500, "hardware-requirements-description")
                .Result();
    }
}
