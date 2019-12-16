using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator
    {
        public RequiredResult Validation(UpdateSolutionBrowsersSupportedViewModel updateSolutionBrowsersSupportedViewModel)
        => new RequiredValidator()
                .Validate(updateSolutionBrowsersSupportedViewModel.BrowsersSupported, "supported-browsers")
                .Validate(updateSolutionBrowsersSupportedViewModel.MobileResponsive, "mobile-responsive")
                .Result();
    }
}
