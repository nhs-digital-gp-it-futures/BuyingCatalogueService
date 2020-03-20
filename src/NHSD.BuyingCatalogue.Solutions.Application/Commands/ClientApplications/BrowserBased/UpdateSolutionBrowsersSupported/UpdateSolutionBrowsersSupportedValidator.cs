using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator : IValidator<UpdateSolutionBrowsersSupportedCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowsersSupportedCommand command)
        => new RequiredValidator()
                .Validate(command.Data.BrowsersSupported, "supported-browsers")
                .Validate(command.Data.MobileResponsive, "mobile-responsive")
                .Result();
    }
}
