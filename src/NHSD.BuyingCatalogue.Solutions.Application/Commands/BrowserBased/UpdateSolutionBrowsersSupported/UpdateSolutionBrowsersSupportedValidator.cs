using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowsersSupported
{
    internal sealed class UpdateSolutionBrowsersSupportedValidator : IValidator<UpdateSolutionBrowsersSupportedCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowsersSupportedCommand updateSolutionBrowsersSupportedCommand)
        => new RequiredValidator()
                .Validate(updateSolutionBrowsersSupportedCommand.Data.BrowsersSupported, "supported-browsers")
                .Validate(updateSolutionBrowsersSupportedCommand.Data.MobileResponsive, "mobile-responsive")
                .Result();
    }
}
