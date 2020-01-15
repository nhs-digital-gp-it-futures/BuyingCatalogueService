using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileHardwareRequirements
{
    internal sealed class UpdateSolutionNativeMobileHardwareRequirementsValidator : IValidator<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeMobileHardwareRequirementsCommand command)
            => new MaxLengthValidator()
                .Validate(command.HardwareRequirements, 500, "hardware-requirements")
                .Result();
    }
}
