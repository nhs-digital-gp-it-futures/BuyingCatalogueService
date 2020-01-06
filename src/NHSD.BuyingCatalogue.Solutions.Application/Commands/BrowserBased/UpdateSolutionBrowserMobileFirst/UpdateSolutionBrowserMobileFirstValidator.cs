using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstValidator : IValidator<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowserMobileFirstCommand updateSolutionBrowserMobileFirstCommand)
            => new RequiredValidator()
                .Validate(updateSolutionBrowserMobileFirstCommand.UpdateSolutionBrowserMobileFirstViewModel.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}
