using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems
{
    internal sealed class UpdateSolutionNativeDesktopOperatingSystemsValidator : IValidator<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeDesktopOperatingSystemsCommand command)
            => new RequiredMaxLengthResult(
        new RequiredValidator().Validate(command.NativeDesktopOperatingSystemsDescription, "operating-systems-description").Result(),
        new MaxLengthValidator().Validate(command.NativeDesktopOperatingSystemsDescription, 1000, "operating-systems-description").Result());
    }
}
