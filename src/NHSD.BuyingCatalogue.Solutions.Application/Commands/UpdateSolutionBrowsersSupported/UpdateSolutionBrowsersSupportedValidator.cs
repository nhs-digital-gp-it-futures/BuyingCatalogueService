using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator : IValidator<UpdateSolutionBrowsersSupportedCommand, RequiredResult>
    {
        public RequiredResult Validate(UpdateSolutionBrowsersSupportedCommand updateSolutionBrowsersSupportedCommand)
        => new RequiredValidator()
                .Validate(updateSolutionBrowsersSupportedCommand.UpdateSolutionBrowsersSupportedViewModel.BrowsersSupported, "supported-browsers")
                .Validate(updateSolutionBrowsersSupportedCommand.UpdateSolutionBrowsersSupportedViewModel.MobileResponsive, "mobile-responsive")
                .Result();
    }
}
