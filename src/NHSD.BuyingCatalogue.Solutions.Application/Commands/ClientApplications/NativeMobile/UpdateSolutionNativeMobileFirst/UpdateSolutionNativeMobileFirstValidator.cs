using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileFirst
{
    internal sealed class UpdateSolutionNativeMobileFirstValidator : IValidator<UpdateSolutionNativeMobileFirstCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeMobileFirstCommand command)
            => new RequiredValidator()
                .Validate(command.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}
