using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails
{
    internal sealed class UpdateSolutionNativeDesktopConnectivityDetailsValidator : IValidator<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeDesktopConnectivityDetailsCommand command)
            => new RequiredValidator()
                .Validate(command.NativeDesktopMinimumConnectionSpeed, "minimum-connection-speed")
                .Result();
    }
}
